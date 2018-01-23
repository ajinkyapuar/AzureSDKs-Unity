using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.ApplicationInsights;
using System;
using UnityEngine.SceneManagement;
using System.Net;
#if !WINDOWS_UWP
using System.Security.Cryptography.X509Certificates;
#endif
using System.Net.Security;

public class AppInsightsLogger : MonoBehaviour
{

    [SerializeField]
    private string instrumentationKey;

    private TelemetryClient telemetry;

    public static AppInsightsLogger Instance;
    public bool LogSceneLoaded;
    public bool LogSceneUnloaded;
    public bool LogActiveSceneChanged;


    /// <summary>
    /// Expose telemetry for direct access. No sense to abstract all methods. 
    /// </summary>
    public TelemetryClient Telemetry
    {
        get
        {
            if (telemetry == null)
            {
                throw new NullReferenceException("Telemetry hasn't been instantiated yet");
            }
            return telemetry;
        }
    }



#if !WINDOWS_UWP
    /// <summary>
    /// Unity's bug that doesn't handle ssl correctly, at least as of v2017.2
    /// </summary>
    private class CustomCertificatePolicy : ICertificatePolicy
    {
        public bool CheckValidationResult(ServicePoint sp,
            X509Certificate certificate, WebRequest request, int error)
        {
            return true;
        }
    }


    public bool CheckValidCertificateCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool valid = true;
        
        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        valid = false;
                    }
                }
            }
        }
        return valid;
    }
#endif
    // Use this for initialization
    void Awake()
    {

#if !WINDOWS_UWP
        //This works, and one of these two options are required as Unity's TLS (SSL) support doesn't currently work like .NET
        //ServicePointManager.CertificatePolicy = new CustomCertificatePolicy();

        //This 'workaround' seems to work for the .NET Storage SDK, but not event hubs. 
        //If you need all of it (ex Storage, event hubs,and app insights) then consider using the above.
        //If you don't want to check an SSL certificate coming back, simply use the return true delegate below.
        //Also it may help to use non-ssl URIs if you have the ability to, until Unity fixes the issue (which may be fixed by the time you read this)
        ServicePointManager.ServerCertificateValidationCallback = CheckValidCertificateCallback; //delegate { return true; };
#endif

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("More than one instance of AppInsightsLogger found. Destroying the additional one created.");
            return;
        }

        //Let's keep this around so we can log during lifetime of app
        DontDestroyOnLoad(gameObject);


        if (string.IsNullOrEmpty(instrumentationKey))
        {
            Debug.LogError("The instrumentation key needs to be set to log events");
            return;
        }

        var config = Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.CreateDefault();
        config.InstrumentationKey = instrumentationKey;
        telemetry = new TelemetryClient(config);

        //Let's hook into a couple common Unity events.
        if (LogSceneLoaded)
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        if (LogSceneUnloaded)
        {
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        }

        if (LogActiveSceneChanged)
        {
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }
        
        //We want to log errors. This gives us the ability to view all logged messages and act on them.
        Application.logMessageReceivedThreaded += Application_logMessageReceivedThreaded;

    }

    private void Application_logMessageReceivedThreaded(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Exception || type == LogType.Error)
        {
            var properties = new Dictionary<string, string>();
            properties.Add("StackTrace", stackTrace);
            properties.Add("Condition", condition);
            properties.Add("LogType", type.ToString());

            //Not using TrackException here since it isn't an exception type we receive.
            telemetry.TrackEvent(type == LogType.Exception ?
                                TelemetryEventNames.Exception : TelemetryEventNames.Error,
                                properties);

        }
    }

    private void SceneManager_activeSceneChanged(Scene priorScene, Scene activeScene)
    {
        var properties = new Dictionary<string, string>();
        properties.Add("PriorScene", priorScene.name);
        properties.Add("ActiveScene", activeScene.name);
        telemetry.TrackEvent(TelemetryEventNames.ActiveSceneChanged, properties);
    }

    private void SceneManager_sceneUnloaded(Scene scene)
    {
        var properties = new Dictionary<string, string>();
        properties.Add("Name", scene.name);

        telemetry.TrackEvent("SceneUnloaded", properties);
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        var properties = new Dictionary<string, string>();
        properties.Add("Name", scene.name);
        properties.Add("LoadSceneMode", loadSceneMode.ToString());

        telemetry.TrackEvent(TelemetryEventNames.SceneLoaded, properties);
    }

    private void OnDestroy()
    {
        //cleanup event handler
        Application.logMessageReceivedThreaded -= Application_logMessageReceivedThreaded;
        
        //ensure in-mem events are flushed.
        telemetry.Flush();
    }
}
