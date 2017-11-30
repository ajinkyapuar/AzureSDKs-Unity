using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This approach uses a single place to name the telemetry events to log
/// which helps prevent misspelling them.
/// </summary>
public static class TelemetryEventNames
{
    public const string ActiveSceneChanged = "ActiveSceneChanged";
    public const string SceneLoaded = "SceneLoaded";
    public const string SceneUnloaded = "SceneUnloaded";
}

