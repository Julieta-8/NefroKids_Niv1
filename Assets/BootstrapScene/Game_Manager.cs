/*using UnityEngine;

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
*/


