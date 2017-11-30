#define DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Diagnostics;

public class AppInsightsTest : MonoBehaviour
{
    public void TrackEvent()
    {
        string eventInfo = DateTime.Now.ToString() + " Test Event on platform:" + Application.platform;
#if UNITY_EDITOR
           eventInfo = "EDITOR:" + eventInfo;
#endif
        AppInsightsLogger.Instance.Telemetry.TrackEvent(eventInfo);
    }

    public void TrackPageView()
    {
        AppInsightsLogger.Instance.Telemetry.TrackPageView("Loaded page view");
    }

    public void TrackMetric()
    {
        var sample = new MetricTelemetry();
        sample.Name = "Some Metric Name";
        sample.Sum = UnityEngine.Random.Range(1f,10f);
        AppInsightsLogger.Instance.Telemetry.TrackMetric(sample);
    }

    public void TrackException()
    {
        AppInsightsLogger.Instance.Telemetry.TrackException(new Exception("Test Exception"));
    }

    public void TrackRequest()
    {
        //TODO
        //AppInsightsLogger.Instance.Telemetry.TrackRequest
    }


    public void TrackTrace()
    {
        AppInsightsLogger.Instance.Telemetry.TrackTrace(System.Environment.StackTrace.ToString());
    }

    public void TrackDependency()
    {
        StartCoroutine(CoTrackDependency());

    }
    public void Flush()
    {
        //Manual flush for debugging
        AppInsightsLogger.Instance.Telemetry.Flush();
    }

    IEnumerator CoTrackDependency()
    {
        var success = false;
        var startTime = DateTime.UtcNow;
        var timer = Stopwatch.StartNew();
        
        //Fake a delay here where we'd be calling an external service
        yield return new WaitForSeconds(2);

        success = true;

        timer.Stop();
        AppInsightsLogger.Instance.Telemetry.TrackDependency("SomeExternalWebCall", "GetUserHighScore", startTime, timer.Elapsed, success);

    }
}
