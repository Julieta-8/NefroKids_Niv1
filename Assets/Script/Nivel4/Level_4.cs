using UnityEngine;
public class Level_4 : MonoBehaviour
{
    public GameObject Alcohol;
    public GameObject Bolsa;
    public GameObject phase2;
    public GameObject BotonSiguiente;
    public GameObject manos;

    void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        Alcohol.SetActive(true);
        phase2.SetActive(false);
        BotonSiguiente.SetActive(false);
        manos.SetActive(true);
    }

    public void PasarABolsa()
    {
        Alcohol.SetActive(false);
        phase2.SetActive(true);
        BotonSiguiente.SetActive(false);
        manos.SetActive(false);
    }

    public void MostrarBotonSiguiente()
    {
        BotonSiguiente.SetActive(true);
    }
}