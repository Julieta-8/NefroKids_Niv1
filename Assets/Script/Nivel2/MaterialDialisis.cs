using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevoMaterial", menuName = "NfroKids/Material Diálisis")]
public class MaterialDialisis : ScriptableObject
{
    [Header("Información General")]

    public int id;

    public string nombre;

    [TextArea(2, 5)]
    public string funcion;

    [TextArea(2, 5)]
    public string importancia;

    [TextArea(2, 5)]
    public string riesgo;

    [Header("Imagen")]

    public Sprite imagen;
}