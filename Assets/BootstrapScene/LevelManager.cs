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

    private void ProcessMessage(string json)
    {
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
            _ => null
        };

        if (sceneName == null)
        {
            Debug.LogError($"Nivel inválido: {level}");
            return;
        }

        Debug.Log($"[LevelManager] Cargando {sceneName}");

        SceneManager.LoadScene(sceneName);
    }
}

[System.Serializable]
public class StartLevelMessage
{
    public string type;
    public int level;
}