using UnityEngine;

public class Level_4 : MonoBehaviour
{
    public GameObject Alcohol;
    public GameObject Bolsa;
    public GameObject bolsadrenajec;

    public GameObject phase1;
    public GameObject phase2;
    public GameObject phase3;

    public GameObject BotonSiguiente;
    public GameObject manos;

    // Variable para saber en qué fase estamos parado actualmente
    private int faseActual = 1;

    void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        faseActual = 1;

        // Al comenzar solamente aparece phase1
        phase1.SetActive(true);
        phase2.SetActive(false);
        phase3.SetActive(false);

        // Botón oculto al iniciar
        BotonSiguiente.SetActive(false);
    }

    // Llama a esta función cuando termine el objetivo de la fase 1 O de la fase 2
    public void MostrarBotonSiguiente()
    {
        BotonSiguiente.SetActive(true);
    }

    // Se llama cuando el jugador presiona el botón "Siguiente"
    public void Siguiente()
    {
        // Ocultamos el botón al avanzar
        BotonSiguiente.SetActive(false);

        if (faseActual == 1)
        {
            // Ocultar phase1 y activar phase2
            phase1.SetActive(false);
            phase2.SetActive(true);
            phase3.SetActive(false);

            faseActual = 2; // Actualizamos el estado a la fase 2
        }
        else if (faseActual == 2)
        {
            // Ocultar phase2 y activar phase3
            phase1.SetActive(false);
            phase2.SetActive(false);
            phase3.SetActive(true);

            faseActual = 3; // Actualizamos el estado a la fase 3
        }
    }
}