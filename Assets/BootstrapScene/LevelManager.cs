using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Level_1 level1Manager;
    private Level_2 level2Manager;
    [SerializeField] private int debugLevel = 2;

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
#if UNITY_EDITOR
        Debug.Log($"[DEBUG] Iniciando automáticamente el nivel {debugLevel}");
        StartLevel(debugLevel);
#else
        ReactConnection react = FindFirstObjectByType<ReactConnection>();
        react.Log("Iniciando nivel");
        react.Send(new ReadyMessage());
#endif
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
        /*
        Level_1 level1Manager = FindFirstObjectByType<Level_1>();

        if (level1Manager == null)
        {
            Debug.LogError("No se encontró Level_1");
            return;
        }
        */


        Level_2 level2Manager = FindFirstObjectByType<Level_2>();

        if (level2Manager == null)
        {
            Debug.LogError("No se encontró Level_2");
            return;
        }

        switch (level)
        {
            case 1:
                level2Manager.StartLevel();
                break;
            /*case 2:
                level2Manager.StartLevel();
                break;
                */

        }

    }
}

    [System.Serializable]
    public class LevelCompletedMessage
    {
        public string type = "LEVEL_COMPLETED";

        public ResultData result;
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
