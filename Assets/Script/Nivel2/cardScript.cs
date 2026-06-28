using UnityEngine;
using UnityEngine.UI;

public class cardScript : MonoBehaviour
{
    [Header("Componentes")]

    [SerializeField] private Image cardImage;

    [SerializeField] private Sprite backSprite;

    [Header("Datos")]

    public MaterialDialisis material;

    private GameManager gameManager;

    private bool descubierta = false;

<<<<<<< HEAD
    private bool encontrada = false;
=======
	public void setupGraphics() {
		_cardBack = _manager.GetComponent<Level_2> ().getCardBack ();
		_cardFace = _manager.GetComponent<Level_2> ().getCardFace (_cardValue);
>>>>>>> 03c4578464c9a690586a94453f54d26c0b72b609

    //--------------------------------------------------

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
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