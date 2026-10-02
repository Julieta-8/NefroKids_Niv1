using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ReactConnection : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SendToReactNative(string json);
#endif

    public static Action<string> OnMessageReceived;

    private static ReactConnection instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Send(object message)
    {
        string json = JsonUtility.ToJson(message);

#if UNITY_WEBGL && !UNITY_EDITOR
        SendToReactNative(json);
#else
        Debug.Log($"[ReactConnection] Enviado: {json}");
#endif
    }

    public void Receive(string json)
    {
        Debug.Log($"[ReactConnection] Recibido: {json}");
        Log($"Recibido: {json}");

        OnMessageReceived?.Invoke(json);
    }

    public void Log(string message)
    {
        Send(new DebugMessage { message = message });
    }

    [System.Serializable]
    public class DebugMessage
    {
        public string type = "DEBUG";
        public string message;
    }


}
[System.Serializable]
public class LevelCompletedMessage
{
    public string type = "LEVEL_COMPLETED";
}