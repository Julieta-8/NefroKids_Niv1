using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Level_1 level1Manager;
   // [SerializeField] private Level_2 level2Manager; 

    private void OnEnable()
    {
        ReactConnection.OnMessageReceived += ProcessMessage;
    }

    private void OnDisable()
    {
        ReactConnection.OnMessageReceived -= ProcessMessage;
    }

    private void Start() { 
        ReactConnection react = FindFirstObjectByType<ReactConnection>(); 
        react.Log("Iniciando nivel"); 
        react.Send(new ReadyMessage()); 
    }

    private void ProcessMessage(string json)
    {
        ReactConnection react = FindFirstObjectByType<ReactConnection>();

        react.Log("Entró a ProcessMessage");

        StartLevelMessage message =
            JsonUtility.FromJson<StartLevelMessage>(json);

        react.Log($"Tipo: {message.type}");
        react.Log($"Nivel: {message.level}");

        if (message.type == "START_LEVEL")
        {
            react.Log("Voy a StartLevel");

            StartLevel(message.level);
        }
    }

    private void StartLevel(int level)
    {
        Level_1 level1Manager = FindFirstObjectByType<Level_1>();

        if (level1Manager == null)
        {
            Debug.LogError("No se encontró Level_1");
            return;
        }

        switch (level)
        {
            case 1:
                level1Manager.StartLevel();
                break;
        }
    }
}