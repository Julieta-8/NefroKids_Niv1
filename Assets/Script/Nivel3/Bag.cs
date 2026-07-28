using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public BagData bagData;

    public SpriteRenderer SpriteRenderer { get; private set; }

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetData(BagData data)
    {
        bagData = data;
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
               Mathf.Approximately(glucosa, 1.5f);
    }
}