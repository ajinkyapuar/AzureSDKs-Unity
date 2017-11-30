#define DEBUG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.ApplicationInsights;
using System;
using UnityEngine.SceneManagement;

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
                Debug.LogError("Telemetry hasn't been instantiated yet");
                throw new NullReferenceException("Telemetry hasn't been instantiated yet");
            }
            Debug.Log("returning telemetry");
            return telemetry;
        }
    }


    // Use this for initialization
    void Awake()
    {
        Debug.Log("Adding trace listener");
        System.Diagnostics.Debug.Listeners.Clear();

        System.Diagnostics.Debug.Listeners.Add(new UnityConsoleDebugTraceListener());
        System.Diagnostics.Debug.WriteLine("Test");
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
            //telemetry.TrackEvent(type == LogType.Exception ?
            //                    TelemetryEventNames.Exception : TelemetryEventNames.Error,
            //                    properties);
        }
    }

    private void SceneManager_activeSceneChanged(Scene priorScene, Scene activeScene)
    {
        var properties = new Dictionary<string, string>();
        properties.Add("PriorScene", priorScene.name);
        properties.Add("ActiveScene", activeScene.name);
        //telemetry.TrackEvent(TelemetryEventNames.ActiveSceneChanged, properties);
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

        //telemetry.TrackEvent(TelemetryEventNames.SceneLoaded, properties);
    }

    private void OnDestroy()
    {
        //cleanup event handler
        Application.logMessageReceivedThreaded -= Application_logMessageReceivedThreaded;
        
        //ensure in-mem events are flushed.
        telemetry.Flush();
    }
}
