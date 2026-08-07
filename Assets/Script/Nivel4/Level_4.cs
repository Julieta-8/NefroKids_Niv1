
using UnityEngine;

public class Level_4 : MonoBehaviour
{
    public GameObject Alcohol;
    public GameObject Bolsa;

    // Start is called before the first frame update
    public void StartLevel()
    {
        Alcohol.SetActive(false);
        Bolsa.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
