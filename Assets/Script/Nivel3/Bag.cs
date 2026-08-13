using UnityEngine;

public class Bag : MonoBehaviour
{
    public BagData bagData;

    public SpriteRenderer SpriteRenderer;

    public Transform zoomPoint;

    public Vector3 originalPosition;
    public Vector3 originalScale;

    private void Awake()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;

        if (SpriteRenderer == null)
            SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}

[System.Serializable]
public class BagData
{
    public string fecha;
    public int volumen;
    public float glucosa;

    public bool tieneFugas;
    public bool estaVencida;

    public bool IsCorrect()
    {
        return !estaVencida &&
               !tieneFugas &&
               volumen == 2000 &&
               glucosa == 1.5f;
    }
}