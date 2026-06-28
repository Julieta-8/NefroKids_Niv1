using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level_1 level1Manager;
    [SerializeField] private Level_2 level2Manager; 

    private void OnEnable()
    {
        ReactConnection.OnMessageReceived += ProcessMessage;
    }

    private void OnDisable()
    {
        ReactConnection.OnMessageReceived -= ProcessMessage;
    }

    private void Start()
    {
        ReactConnection react = FindFirstObjectByType<ReactConnection>();
        react.Log("Iniciando nivel");
        react.Send(new ReadyMessage());
    }

    private void ProcessMessage(string json)
    {
        StartLevelMessage message = JsonUtility.FromJson<StartLevelMessage>(json);
        ReactConnection react = FindFirstObjectByType<ReactConnection>();
        react.Log("LevelManager Start");
        react.Send(new ReadyMessage());

        if (message.type == "START_LEVEL")
        {
            StartLevel(message.level);
        }
    }

    private void StartLevel(int level)
    {
        switch (level)
        {
            case 1:
                level1Manager.StartLevel();
                break;

            case 2:
                level2Manager.StartLevel();
                break;

            default:
                Debug.LogError($"Nivel {level} no implementado.");
                break;
        }
    }
}