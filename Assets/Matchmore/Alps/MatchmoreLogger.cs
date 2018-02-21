using UnityEngine;

public static class MatchmoreLogger
{
    public static bool Enabled { get; set; }

    public static GameObject Context { get; set; }

    public static void Debug(string format, params object[] args)
    {
        if (!Enabled)
        {
            return;
        }

        var logText = string.Format(format, args);
#if (SILVERLIGHT || WINDOWS_PHONE || MONOTOUCH || __IOS__ || MONODROID || __ANDROID__)
                System.Diagnostics.Debug.WriteLine(logText);
#elif (UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_IOS || UNITY_ANDROID || UNITY_5 || UNITY_WEBGL || UNITY_WSA_8_1 || UNITY_WSA_10_0 || UNITY_WSA)
        if (Context != null)
            UnityEngine.Debug.Log(string.Format("\nMatchmore: {0} \n", logText), Context);
        else
            UnityEngine.Debug.Log(string.Format("\nMatchmore: {0} \n", logText));
#else
                try {
                    Trace.WriteLine (logText);
                    Trace.Flush ();
                } catch {
                }
#endif
    }
}