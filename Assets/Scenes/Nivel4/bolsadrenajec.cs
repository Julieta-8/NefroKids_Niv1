using UnityEngine;
using TMPro;

public class bolsadrenajec : MonoBehaviour
{
    private bool bolsaAplicada = false;
    private bool agarrado = false;

    public GameObject palo;
    public GameObject bolsadudosa2;
    public Sprite andycompletobolsas;
    public SpriteRenderer paloRenderer;
    public Level_4 level4; // Agregada la referencia al LevelManager
     public TMP_Text Textalc;

    void Start()
    {
        // Al comenzar la fase 2, esta bolsa NO se puede agarrar hasta que la primera bolsa se ponga
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void OnMouseDown()
    {
        if (!bolsaAplicada)
        {
            agarrado = true;
        }
    }

    void OnMouseUp()
    {
        agarrado = false;

        if (bolsadudosa2 == null || bolsaAplicada) return;

        float distancia = Vector2.Distance(transform.position, bolsadudosa2.transform.position);

        if (distancia < 1.5f)
        {
            bolsaAplicada = true;

            if (paloRenderer != null && andycompletobolsas != null)
            {
                  bolsadudosa2.SetActive(false);
                paloRenderer.sprite = andycompletobolsas;
            }

            if (Textalc != null)
            {
                Textalc.text = "¡Fase 2 completada!";
            }

            // Ocultar la segunda bolsa
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            // LLAMAR AL BOTÓN SIGUIENTE
            if (level4 != null)
            {
                level4.MostrarBotonSiguiente();
            }
            else
            {
                Debug.LogError("¡Falta asignar 'level4' (LevelManager) en el Inspector de bolsadrenajec!");
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