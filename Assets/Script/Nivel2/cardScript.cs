using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine.SceneManagement;
using TMPro;
//using static System.Net.Mime.MediaTypeNames.Image;
using System.Collections;


public class cardScript : MonoBehaviour
{
    [Header("Componentes")]

    [SerializeField] private Image cardImage; //ANTES ERA IMG

    [SerializeField] private Sprite backSprite;

    [Header("Datos")]

    public MaterialDialisis material;

    private Level_2 gameManager;

    private bool descubierta = false;

    private bool encontrada = false;

    //--------------------------------------------------

    private void Awake()
    {
        gameManager = FindObjectOfType<Level_2>();
    }

    //--------------------------------------------------

    private void Start()
    {
        OcultarCarta();
    }

    //--------------------------------------------------

    public void Configurar(MaterialDialisis nuevoMaterial)
    {
        material = nuevoMaterial;

        cardImage.sprite = backSprite;

        descubierta = false;

        encontrada = false;
    }

    //--------------------------------------------------

    public void ClickCarta()
    {
        if (encontrada)
            return;

        if (descubierta)
            return;

        if (gameManager.PuedeSeleccionar() == false)
            return;

        MostrarCarta();

        gameManager.SeleccionarCarta(this);
    }

    //--------------------------------------------------

    public void MostrarCarta()
    {
        descubierta = true;

        cardImage.sprite = material.imagen;
    }

    //--------------------------------------------------

    public void OcultarCarta()
    {
        if (encontrada)
            return;

        descubierta = false;

        cardImage.sprite = backSprite;
    }

    //--------------------------------------------------

    public void MarcarComoEncontrada()
    {
        encontrada = true;
    }

    //--------------------------------------------------

    public bool EstaDescubierta()
    {
        return descubierta;
    }

    //--------------------------------------------------

    public bool EstaEncontrada()
    {
        return encontrada;
    }

    //--------------------------------------------------

    public int ObtenerID()
    {
        return material.id;
    }

    //--------------------------------------------------

    public MaterialDialisis ObtenerMaterial()
    {
        return material;
    }
}