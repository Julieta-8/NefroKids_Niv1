using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        ReactConnection.OnMessageReceived += ProcessMessage;
    }

    private void OnDisable()
    {
        ReactConnection.OnMessageReceived -= ProcessMessage;
    }

    void ProcessMessage(string json)
    {
        StartLevelMessage message =
            JsonUtility.FromJson<StartLevelMessage>(json);

        switch (message.type)
        {
            case "START_LEVEL":
                StartLevel(message.level);
                break;
        }
    }
    void Start()
    {
        ReactConnection react = FindFirstObjectByType<ReactConnection>();

        react.Send(new ReadyMessage());
    }

    void StartLevel(int level)
    {
        Debug.Log($"Iniciando nivel {level}");
    }
}

[System.Serializable]
public class ReadyMessage
{
    public string type = "READY";
}

[System.Serializable]
public class StartLevelMessage
{
    public string type;
    public int level;
}

[System.Serializable]
public class LevelCompletedMessage
{
    public string type = "LEVEL_COMPLETED";
    public int stars;
    public float time;
}