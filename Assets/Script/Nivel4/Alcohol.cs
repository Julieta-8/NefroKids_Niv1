
using UnityEngine;

public class Alcohol : MonoBehaviour
{
    private bool alcoholAplicado = false;
    private bool agarrado = false;
    public GameObject manos;
    public Sprite manosMojadas;
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