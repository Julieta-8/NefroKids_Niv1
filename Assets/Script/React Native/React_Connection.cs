using UnityEngine;

public class React_Connection : MonoBehaviour
{
    public Nivel_1 nivel1;

    void Start()
    {
        Debug.Log("React_Connection arrancó");

        Debug.Log("nivel1 = " + nivel1);

        if (nivel1 == null)
        {
            Debug.LogError("Nivel_1 no asignado en React_Connection");
            return;
        }

        Debug.Log("Nivel_1 encontrado correctamente");

        nivel1.OnLevelCompleted += SendResultToReact;
    }

    void OnDestroy()
    {
        if (nivel1 != null)
        {
            nivel1.OnLevelCompleted -= SendResultToReact;
        }
    }

    public void SendResultToReact()
    {
        ResultData result = new ResultData
        {
            levelId = 1,
            completed = true,

            phase1Time = nivel1.Phase1Time,
            phase2Time = nivel1.Phase2Time,
            averageTime = nivel1.AverageTime,

            phase1Stars = nivel1.Phase1Stars,
            phase2Stars = nivel1.Phase2Stars
        };

        string json = JsonUtility.ToJson(result);

        Debug.Log("Enviando a React Native:");
        Debug.Log(json);

#if UNITY_ANDROID
        using (AndroidJavaClass jc =
            new AndroidJavaClass(
                "com.azesmwayreactnativeunity.ReactNativeUnityViewManager"))
        {
            jc.CallStatic(
                "sendMessageToMobileApp",
                json
            );
        }
#endif
    }
}