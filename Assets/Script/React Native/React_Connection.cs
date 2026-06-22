using System.Runtime.InteropServices;
using UnityEngine;

public class React_Connection : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SendToReactNative(string json);

    public void SendResult(ResultData result)
    {
        string json = JsonUtility.ToJson(result);

#if UNITY_WEBGL && !UNITY_EDITOR
        SendToReactNative(json);
#else
        Debug.Log(json);
#endif
    }
}