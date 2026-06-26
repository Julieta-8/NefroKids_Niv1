using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialDialisis
{
    public int id;

    public string nombre;

    [TextArea(2, 5)]
    public string funcion;

    [TextArea(2, 5)]
    public string importancia;

    public Sprite imagen;
}