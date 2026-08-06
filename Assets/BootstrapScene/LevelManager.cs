
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Level_1 level1Manager;
    private Level_2 level2Manager;
    private Level_4 level4Manager;
    [SerializeField] private GameObject nivel1;
    [SerializeField] private GameObject nivel2;
    [SerializeField] private GameObject nivel4;
    [SerializeField] private int debugLevel = 2;

    private void OnEnable()
    {
        ReactConnection.OnMessageReceived += ProcessMessage;
    }

    private void OnDisable()
    {
        ReactConnection.OnMessageReceived -= ProcessMessage;
    }
    private void Awake()
    {
        level1Manager = nivel1.GetComponentInChildren<Level_1>(true);
        level2Manager = nivel2.GetComponentInChildren<Level_2>(true);
        level4Manager = nivel4.GetComponentInChildren<Level_4>(true);
        Debug.Log(level4Manager);
    }

    private void Start()
    {
        nivel1.SetActive(false);
        nivel2.SetActive(false);
        nivel4.SetActive(false);
#if UNITY_EDITOR
        Debug.Log($"[DEBUG] Iniciando automáticamente el nivel {1}");
        StartLevel(4);
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
        nivel1.SetActive(false);
        nivel2.SetActive(false);
        nivel4.SetActive(false);

        switch (level)
        {
            case 1:
                nivel1.SetActive(true);
                level1Manager.StartLevel();
                break;

            case 2:

                nivel2.SetActive(true);
                level2Manager.StartLevel();

                break;
            case 4:
                nivel4.SetActive(true);
                level4Manager.StartLevel();
                break;
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
