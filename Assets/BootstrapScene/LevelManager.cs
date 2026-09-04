/*using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Level_1 level1Manager;
    private Level_2 level2Manager;
    private Nivel3 level3Manager;

    [SerializeField] private GameObject nivel1;
    [SerializeField] private GameObject nivel2;
    [SerializeField] private GameObject nivel3;

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
        level3Manager = nivel3.GetComponentInChildren<Nivel3>(true);

    }

    private void Start()
    {
        nivel1.SetActive(false);
        nivel2.SetActive(false);
        nivel3.SetActive(false);
#if UNITY_EDITOR
        Debug.Log($"[DEBUG] Iniciando automáticamente el nivel {1}");
        StartLevel(1);
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
        nivel3.SetActive(false);

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
            case 3:

                nivel3.SetActive(true);
                level3Manager.StartLevel();

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
*/