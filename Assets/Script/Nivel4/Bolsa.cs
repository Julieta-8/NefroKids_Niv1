using UnityEngine;

public class Bolsa : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Sprite bolsa1;
    public Sprite bolsa2;
    public Sprite bolsa3;
    public Sprite bolsa4;

    private int bolsaActual = 1;

    void Start()
    {
        spriteRenderer.sprite = bolsa1;
    }

    void Update()
    {

    }

    public void Tick()
    {
        if (bolsaActual == 4)
        {
            Debug.Log("GANASTE");
        }
        else
        {
            Reiniciar();
        }
    }

    public void Cruz()
    {
        Debug.Log("Entró a Cruz");
        Debug.Log("Bolsa actual: " + bolsaActual);

        if (bolsaActual == 4)
        {
            Debug.Log("Era la bolsa buena");
            Reiniciar();
        }
        else
        {
            Debug.Log("Pasa a la siguiente bolsa");
            SiguienteBolsa();
        }
    }

    void SiguienteBolsa()
    {
        bolsaActual++;

        Debug.Log("Ahora la bolsa es: " + bolsaActual);

        if (bolsaActual == 2)
        {
            Debug.Log("Cambiando a bolsa2");
            spriteRenderer.sprite = bolsa2;
            Debug.Log("Sprite actual: " + spriteRenderer.sprite.name);
        }
        else if (bolsaActual == 3)
        {
            Debug.Log("Cambiando a bolsa3");
            spriteRenderer.sprite = bolsa3;
            Debug.Log("Sprite actual: " + spriteRenderer.sprite.name);
        }
        else if (bolsaActual == 4)
        {
            Debug.Log("Cambiando a bolsa4");
            spriteRenderer.sprite = bolsa4;
            Debug.Log("Sprite actual: " + spriteRenderer.sprite.name);
        }
    }

    void Reiniciar()
    {
        Debug.Log("INTENTELO DE NUEVO");

        bolsaActual = 1;
        spriteRenderer.sprite = bolsa1;
    }
}