
using TMPro;
using UnityEngine;

public class Alcohol : MonoBehaviour
{
    private bool alcoholAplicado = false;
    private bool agarrado = false;
    private float progresoFrotado = 0f;
    private float tiempoSecado = 0f;
    private bool secando = false;

    public Level_4 level4;
    public GameObject manos;
    public Sprite manosMojadas;
    public Sprite manosFrotadas;
    public SpriteRenderer manosRenderer;
    private bool terminoParte = false;
    public TMP_Text Textalc;

    void OnMouseDown()
    {
        agarrado = true;
    }

    void OnMouseUp()
    {
        agarrado = false;

        float distancia = Vector2.Distance(
            transform.position,
            manos.transform.position
        );

        if (distancia < 1.5f)
        {
            // Cambiar sprite de las manos
            manosRenderer.sprite = manosMojadas;


            alcoholAplicado = true;

            // Cambiar el texto
            Textalc.text = "frota las manos para secarlas";

            // Desactivar el alcohol
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    void Update()
    {
        // Mover el alcohol con el mouse
        if (agarrado)
        {
            Vector3 posicion = Camera.main.ScreenToWorldPoint(
                Input.mousePosition
            );

            posicion.z = 0;

            transform.position = posicion;
        }

        // Frotar las manos
        if (alcoholAplicado && Input.GetMouseButton(0))
        {
            progresoFrotado += Time.deltaTime;

            if (progresoFrotado >= 1f)
            {
                manosRenderer.sprite = manosFrotadas;

                secando = true;
            }
        }

        // Tiempo de secado
        if (secando)
        {
            tiempoSecado += Time.deltaTime;

            if (tiempoSecado >= 1f && !terminoParte)
            {
                terminoParte = true;
                level4.MostrarBotonSiguiente();
            }
        }

    }
}