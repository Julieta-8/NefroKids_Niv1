using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;
using UnityMessageManagerNS;

[System.Serializable]
public class GameData
{
    public int levelId;
}

[System.Serializable]
public class ResultData
{
    public int levelId;
    public bool completed;
    public int durationSeconds;
}
public class React_Connection : MonoBehaviour
{
    public Nivel_1 nivel1;

    public void OnMessage(string json)
    {
        Debug.Log("Mensaje de React: " + json);

        GameData data = JsonUtility.FromJson<GameData>(json);

        if (data.levelId == 1)
        {
            nivel1.gameObject.SetActive(true);
        }
    }

    public void SendResultToReact()
    {
        var result = new ResultData
        {
            levelId = 1,
            completed = true,
            durationSeconds = 120
        };

        string json = JsonUtility.ToJson(result);

        UnityMessageManager.Instance.SendMessageToRN(json);
    }
}
