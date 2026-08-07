using UnityEngine;

public class Cruz : MonoBehaviour
{
    public Bolsa bolsa;

    void OnMouseDown()
    {
        bolsa.Cruz();
    }
}