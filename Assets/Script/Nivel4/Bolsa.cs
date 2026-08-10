using UnityEngine;

public class Bolsa : MonoBehaviour
{
    private bool bolsaAplicada = false;
    public GameObject palo;
    public Sprite palobolsas;
    public paloRenderer manosRenderer;


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
            palo.sprite = palobolsas;
            
        }
    }
}
