using UnityEngine;

public class Level_4 : MonoBehaviour
{
    public GameObject Alcohol;
    public GameObject Bolsa;

    public GameObject phase1;
    public GameObject phase2;

    public GameObject BotonSiguiente;
    public GameObject manos;

    void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        // Al comenzar solamente aparece phase1
        phase1.SetActive(true);
        phase2.SetActive(false);


        // Botón oculto hasta terminar phase1
        BotonSiguiente.SetActive(false);
    }

    // Esta función se llama cuando termina la parte del alcohol
    public void MostrarBotonSiguiente()
    {
        BotonSiguiente.SetActive(true);
    }

    // Se llama cuando el jugador toca "Siguiente"
    public void Siguiente()
    {
        // Ocultar TODO phase1
        phase1.SetActive(false);

        // Ocultar botón
        BotonSiguiente.SetActive(false);

        // Mostrar phase2
        phase2.SetActive(true);
    }
}