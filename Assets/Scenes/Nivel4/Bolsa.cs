using UnityEngine;
using TMPro;

public class Bolsa : MonoBehaviour
{
    private bool agarrado = false;
    private bool puesta = false;

    public GameObject bolsadudosa;
    public Sprite palobolsas;
    public GameObject bolsadrenajec; // Asigna el GameObject de la segunda bolsa en el Inspector
    public SpriteRenderer paloRenderer;
    public TMP_Text Textalc;

    void OnMouseDown()
    {
        if (!puesta)
        {
            agarrado = true;
        }
    }

    void OnMouseUp()
    {
        agarrado = false;

        if (bolsadudosa == null || puesta) return;

        float distancia = Vector2.Distance(transform.position, bolsadudosa.transform.position);

        if (distancia < 1.5f)
        {
            puesta = true;

            // Cambiar imagen y texto
            if (paloRenderer != null && palobolsas != null)
            {
                paloRenderer.sprite = palobolsas;
            }

            if (Textalc != null)
            {
                 bolsadudosa.SetActive(false);
                Textalc.text = "¡Arrastra la segunda bolsa!";
            }

            // Ocultar esta primera bolsa
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            // HABILITAR LA SEGUNDA BOLSA para que el jugador pueda interactuar con ella
            if (bolsadrenajec != null)
            {
                BoxCollider2D colliderSegundaBolsa = bolsadrenajec.GetComponent<BoxCollider2D>();
                if (colliderSegundaBolsa != null)
                {
                    colliderSegundaBolsa.enabled = true;
                }
            }
            else
            {
                Debug.LogError("¡Falta asignar 'bolsadrenajec' en el Inspector de la primera bolsa!");
            }
        }
    }
    
    void Update()
    {
        if (agarrado)
        {
            Vector3 posicion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            posicion.z = 0;
            transform.position = posicion;
           
        }
    }
}