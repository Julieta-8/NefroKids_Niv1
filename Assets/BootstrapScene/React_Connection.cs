using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ReactConnection : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SendToReactNative(string json);
#endif

    // Evento para que otros scripts escuchen los mensajes
    public static Action<string> OnMessageReceived;

    /// <summary>
    /// Envía cualquier objeto serializable a React Native.
    /// </summary>
    public void Send(object message)
    {
        string json = JsonUtility.ToJson(message);

#if UNITY_WEBGL && !UNITY_EDITOR
        SendToReactNative(json);
#else
        Debug.Log($"[ReactConnection] Enviado: {json}");
#endif
    }

    /// <summary>
    /// React Native llama a este método mediante SendMessage().
    /// </summary>
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