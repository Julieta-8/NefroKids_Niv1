using UnityEngine;

public class Tick : MonoBehaviour
{
    public Bolsa bolsa;

    void OnMouseDown()
    {
        bolsa.Tick();
    }
}
