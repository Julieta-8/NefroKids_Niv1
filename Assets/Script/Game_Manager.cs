/*using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    public void FinishLevel()
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

[System.Serializable]
public class ResultData
{
    public int levelId;
    public bool completed;
    public int durationSeconds;
}
*/