using UnityEngine;

public class Level_4 : MonoBehaviour
{
    public GameObject Alcohol;
    public GameObject Bolsa;
    public GameObject BotonSiguiente;

    public void StartLevel()
    {
        Alcohol.SetActive(true);
        Bolsa.SetActive(false);

        // El botón empieza oculto
        BotonSiguiente.SetActive(false);
    }

    // Esta función pasa de Alcohol a Bolsa
    public void PasarABolsa()
    {
        Alcohol.SetActive(false);
        Bolsa.SetActive(true);

        // Ocultar el botón
        BotonSiguiente.SetActive(false);
    }

    // Mostrar el botón cuando termina Alcohol
    public void MostrarBotonSiguiente()
    {
        BotonSiguiente.SetActive(true);
    }

    void Update()
    {

    }
}