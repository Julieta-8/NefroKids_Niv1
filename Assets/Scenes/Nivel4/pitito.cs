using UnityEngine;
using System.Collections;
using TMPro;

public class Pitito : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    public float rotacionInicial = -72f;
    public float pasoRotacion = -72f;
    
    [Header("Referencias y Tiempos")]
    public GameObject palocompleto; 
    public GameObject imagenZoom; 
    public GameObject Pitito1; 
    public float tiempoEspera = 2f; // Tiempo de espera tras acomodar el pitito a 0°

    [Header("Interfaz de Fin de Juego")]
    public GameObject panelFinal;
    public TMP_Text textoFinal;
    private bool bloqueado = false;

    [Header("Textos de la Interfaz (UI)")]
    public TMP_Text textoSuperior;  // El texto central de la pantalla
    public TMP_Text textoCostado;   // El texto de la izquierda ("Toca a Andy...")
    private float rotacionActual;
    private bool cuentaIniciada = false;

    void Start()
    {
        rotacionActual = rotacionInicial;
        AplicarRotacion();

        if (palocompleto != null)
        {
            palocompleto.SetActive(false);
        }

        if (textoSuperior != null)
        {
            textoSuperior.text = "Rota para conseguir que Andy esté en la posición correcta";
        }

        if (textoCostado != null)
        {
            textoCostado.gameObject.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (cuentaIniciada || bloqueado) return;

        rotacionActual += pasoRotacion;
        rotacionActual = Mathf.Repeat(rotacionActual, 360f);

        if (Mathf.Approximately(rotacionActual, 0f) || Mathf.Approximately(rotacionActual, 360f))
        {
            rotacionActual = 0f;
            AplicarRotacion();

            StartCoroutine(EsperarYMostrarPalo());
        }
        else
        {
            AplicarRotacion();
        }
    }

    void AplicarRotacion()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, rotacionActual);
    }

    IEnumerator EsperarYMostrarPalo()
    {
        cuentaIniciada = true;

        // 1. Espera inicial de la mecánica de rotación
        yield return new WaitForSeconds(tiempoEspera);

        if (palocompleto != null)
        {
            imagenZoom.SetActive(false);
            Pitito1.SetActive(false);
            palocompleto.SetActive(true);

            // 2. Muestra el mensaje de éxito
            if (textoSuperior != null)
            {
                textoSuperior.text = "¡Excelente! Andy está en la posición correcta.";
            }

            // 3. PAUSA SOLICITADA: Espera 2 segundos antes de terminar completamente
            yield return new WaitForSeconds(2f);

            // 4. Muestra la pantalla final y concluye el juego
            FinalizarJuego();
        }
        else
        {
            Debug.LogError("¡Falta asignar 'palocompleto' en el Inspector de Pitito!");
        }
    }

    public void FinalizarJuego()
    {
        bloqueado = true;

        if (panelFinal != null)
        {
            panelFinal.SetActive(true);
        }

        if (textoFinal != null)
        {
            textoFinal.text =
                "¡Felicitaciones!\n\n" +
                "Has identificado correctamente todos los materiales necesarios para la diálisis peritoneal.\n\n" +
                "Ahora conoces su función y la importancia de utilizarlos correctamente.";
        }

        ReactConnection react = FindFirstObjectByType<ReactConnection>();

        if (react != null)
        {
            react.Send(new LevelCompletedMessage());
        }
    }
}