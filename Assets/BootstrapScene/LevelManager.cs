using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

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

        if (react != null)
        {
            react.Log("Bootstrap listo");
            react.Send(new ReadyMessage());
        }
    }

    private void ProcessMessage(string json)
    {
        Debug.Log($"[LevelManager] Mensaje recibido: {json}");

        StartLevelMessage message =
            JsonUtility.FromJson<StartLevelMessage>(json);

        if (message.type == "START_LEVEL")
        {
            StartLevel(message.level);
        }
    }

    private void StartLevel(int level)
    {
        string sceneName = level switch
        {
            1 => "Nivel 1",
            2 => "Nivel 2",
            3 => "Nivel 3",
            5 => "Nivel 5",
            _ => null
        };

        if (sceneName == null)
        {
            Debug.LogError($"[LevelManager] Nivel inválido: {level}");
            return;
        }

        if (SceneManager.GetActiveScene().name == sceneName)
        {
            Debug.Log($"[LevelManager] Ya estamos en {sceneName}");
            return;
        }

        Debug.Log($"[LevelManager] Cargando {sceneName}");

        SceneManager.LoadScene(sceneName);
    }

#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartLevel(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartLevel(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartLevel(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StartLevel(5);
        }
    }

#endif
}

[System.Serializable]
public class StartLevelMessage
{
    public string type;
    public int level;
}

[System.Serializable]
public class ReadyMessage
{
    public string type = "READY";
}