using UnityEngine;

public class PaloCompleto : MonoBehaviour
{
    [Header("Referencias de la Imagen Zoom")]
    public GameObject PaloCompleto1;// Arrastra aquí el GameObject que contiene la imagen de zoom
    public GameObject imagenZoom; 
    public GameObject Pitito; 

    private void Start()
    {
        // Asegurarnos de que el zoom comience oculto
        if (imagenZoom != null)
        {
            imagenZoom.SetActive(false);
            Pitito.SetActive(false);
        }
    }

    // Se ejecuta automáticamente al hacer clic sobre el objeto
    private void OnMouseDown()
    {
        MostrarZoom();
    }

    public void MostrarZoom()
    {
        if (imagenZoom != null)
        {
            PaloCompleto1.SetActive(false);
            imagenZoom.SetActive(true);
            Pitito.SetActive(true);
        }
        else
        {
            Debug.LogError("¡No asignaste la imagen de 'zooom' en el Inspector de PaloCompleto!");
        }
    }
}