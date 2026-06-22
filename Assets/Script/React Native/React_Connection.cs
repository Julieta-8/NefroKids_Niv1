using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class React_Connection : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SendToReactNative(string json);

    public void SendResultToReact()
    {
        string json = JsonUtility.ToJson(new ResultData
        {
            levelId = 1,
            completed = true
        });

#if UNITY_WEBGL && !UNITY_EDITOR
        SendToReactNative(json);
#else
        Debug.Log(json);
#endif
    }
}