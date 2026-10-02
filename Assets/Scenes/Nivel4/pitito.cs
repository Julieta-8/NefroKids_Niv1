using UnityEngine;
using System.Collections;

public class Pitito : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    public float rotacionInicial = -72f;
    public float pasoRotacion = -72f;
    
    [Header("Referencias y Tiempos")]
    public GameObject palocompleto; // Arrastrá el GameObject del palo completo desde la jerarquía
    public GameObject imagenZoom; 
    public GameObject Pitito1; 
    public float tiempoEspera = 3f;

    private float rotacionActual;
    private bool cuentaIniciada = false;

    void Start()
    {
        // Establecer la rotación inicial en Z
        rotacionActual = rotacionInicial;
        AplicarRotacion();

        if (palocompleto != null)
        {
            palocompleto.SetActive(false);
          
        }
    }

    void OnMouseDown()
    {
        // Si ya está contando los 3 segundos en 0°, evitamos que sigan haciendo clic
        if (cuentaIniciada) return;

        // Sumamos -72°
        rotacionActual += pasoRotacion;

        // Normalizamos el ángulo para que se mantenga en el rango (-360 a 360) o de 0 a 360
        rotacionActual = Mathf.Repeat(rotacionActual, 360f);

        // Si el ángulo es prácticamente 0° (o 360°)
        if (Mathf.Approximately(rotacionActual, 0f) || Mathf.Approximately(rotacionActual, 360f))
        {
            rotacionActual = 0f;
            AplicarRotacion();

            // Iniciamos la cuenta regresiva de 3 segundos
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

        // Espera los 3 segundos manteniendo la rotación en 0°
        yield return new WaitForSeconds(tiempoEspera);

        // Muestra la imagen del palo completo
        if (palocompleto != null)
        {
            imagenZoom.SetActive(false);
            Pitito1.SetActive(false);
            palocompleto.SetActive(true);
        }
        else
        {
            Debug.LogError("¡Falta asignar 'palocompleto' en el Inspector de Pitito!");
        }
    }
}