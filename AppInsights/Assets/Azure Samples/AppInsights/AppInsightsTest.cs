using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Diagnostics;
using Microsoft.ApplicationInsights;

public class AppInsightsTest : MonoBehaviour
{
    private TelemetryClient telemetry;
    private void Start()
    {
        telemetry = AppInsightsLogger.Instance.Telemetry;
    }

    public void TrackEvent()
    {
        string eventInfo = $"Test Event on platform:{Application.platform} InEditor:{Application.isEditor}";
        telemetry.TrackEvent(eventInfo);
    }

    public void TrackPageView()
    {
        telemetry.TrackPageView("Loaded page view");
    }

    public void TrackMetric()
    {
        var sample = new MetricTelemetry();
        sample.Name = "Some Metric Name";
        sample.Sum = UnityEngine.Random.Range(1f, 10f);
        telemetry.TrackMetric(sample);
    }

    public void TrackException()
    {
        telemetry.TrackException(new Exception("Test Exception"));
    }

    public void TrackRequest()
    {
        //Typically this is used to track an HttpRequest for web platforms. See using Operation Context
        //Since an HttpRequest has work associated with it in a single request, this is how you can track work
        //in a single request.

        // Establish an operation context and associated telemetry item:
        using (var operation = telemetry.StartOperation<RequestTelemetry>("operationName"))
        {
            // Telemetry sent in here will use the same operation ID.
            telemetry.TrackTrace("Test trace");

            // Set properties of containing telemetry item--for example:
            operation.Telemetry.ResponseCode = "200";

            // Optional: explicitly send telemetry item:
            telemetry.StopOperation(operation);

        } // When operation is disposed, telemetry item is sent.

    }


    public void TrackTrace()
    {
        telemetry.TrackTrace(System.Environment.StackTrace.ToString());
    }

    public void TrackDependency()
    {
        StartCoroutine(CoTrackDependency());
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
        telemetry.TrackDependency("SomeExternalWebCall", "GetUserHighScore", startTime, timer.Elapsed, success);

    }
    
    public void Flush()
    {
        //Manual flush for debugging
        telemetry.Flush();
    }

}
