
using UnityEngine;

public class Alcohol : MonoBehaviour
{
    private bool alcoholAplicado = false;
    private bool agarrado = false;
    private float progresoFrotado = 0f;
    private float tiempoSecado = 0f;
    private bool secando = false;
    public GameObject manos;
    public Sprite manosMojadas;
    public Sprite manosFrotadas;
    public SpriteRenderer manosRenderer;
    void OnMouseDown()
    {
        agarrado = true;
    }

    void OnMouseUp()
    {
        agarrado = false;
        float distancia = Vector2.Distance(transform.position, manos.transform.position);

        if (distancia < 1.5f)
        {
            manosRenderer.sprite = manosMojadas;
            Debug.Log("¡Alcohol aplicado!");
            alcoholAplicado = true;
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
        if (alcoholAplicado && Input.GetMouseButton(0))
        {
            progresoFrotado += Time.deltaTime;

            if (progresoFrotado >= 1.5f)
            {
                manosRenderer.sprite = manosFrotadas;
                secando = true;
            }
        }
        if (secando)
        {
            tiempoSecado += Time.deltaTime;

            if (tiempoSecado >= 5f)
            {
               // objetivo.text = "¡Nivel completado!";
            }
        }
    }
}