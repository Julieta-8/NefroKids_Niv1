using UnityEngine;
using TMPro;

public class Bolsa : MonoBehaviour
{
    private bool bolsaAplicada = false;
    private bool agarrado = false;
    public GameObject palo;
    public Sprite palobolsas;
    public SpriteRenderer paloRenderer;

    public TMP_Text Textalc;

    void OnMouseDown()
    {
        agarrado = true;

    }
    void OnMouseUp()
    {
        agarrado = false;
        float distancia = Vector2.Distance(transform.position, palo.transform.position);

        if (distancia < 1.5f)
        {
            paloRenderer.sprite = palobolsas;
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
            paloRenderer.sprite = palobolsas;


        }
    }
}
