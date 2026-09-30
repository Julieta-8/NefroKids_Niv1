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

    public TMP_Text Textalc;

    void Start()
    {
        // Al comenzar, la bolsa blanca NO se puede agarrar
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void OnMouseDown()
    {
        agarrado = true;
    }

    void OnMouseUp()
    {
        agarrado = false;

        float distancia = Vector2.Distance(
            transform.position,
            bolsadudosa2.transform.position
        );

        if (distancia < 1.5f)
        {
            paloRenderer.sprite = andycompletobolsas;
            bolsaAplicada = true;

            Textalc.text = "Bolsa aplicada";

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
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

        if (bolsaAplicada && Input.GetMouseButton(0))
        {
            paloRenderer.sprite = andycompletobolsas;
        }
    }
}