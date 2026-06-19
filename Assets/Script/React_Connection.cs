using UnityEngine;
using UnityMessageManagerNS;
public class React_Connection : MonoBehaviour
{
    public Nivel_1 nivel1;
    void Start()
    {
        nivel1.OnLevelCompleted += SendResultToReact;
    }

    void SendResultToReact()
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

        UnityMessageManager.Instance.SendMessageToRN(json);
    }
}