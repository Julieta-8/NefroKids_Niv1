using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    private MaterialDialisis materialActual;
    private int pasoActual;



    [Header("Panel")]
    [SerializeField] private GameObject panelPopup;

    [Header("Imagen")]
    [SerializeField] private Image imagenMaterial;

    [Header("Textos")]
    [SerializeField] private TMP_Text nombreMaterial;
    [SerializeField] private TMP_Text funcionMaterial;
    [SerializeField] private TMP_Text importanciaMaterial;
    [SerializeField] private TMP_Text riesgoMaterial;

    [Header("Botón")]
    [SerializeField] private Button botonContinuar;

    //Referencia al GameManager
    private Level_2 gameManager;

    //--------------------------------------------------------

    private void Start()
    {
        panelPopup.SetActive(false);
        nombreMaterial.text = "";
        funcionMaterial.text = "";
        importanciaMaterial.text = "";
        riesgoMaterial.text = "";

        imagenMaterial.enabled = false;

        botonContinuar.gameObject.SetActive(false);

        botonContinuar.onClick.AddListener(SiguientePaso);

    }

    //--------------------------------------------------------

    public void MostrarMaterial(MaterialDialisis material, Level_2 gm)
    {
        imagenMaterial.enabled = true;

        gameManager = gm;
        materialActual = material;
        pasoActual = 0;

        gameManager.BloquearJuego(true);

        imagenMaterial.sprite = material.imagen;
        nombreMaterial.text = material.nombre;

        panelPopup.SetActive(true);
        botonContinuar.gameObject.SetActive(true);

        MostrarPaso();
    }
    private void MostrarPaso()
    {
        funcionMaterial.text = "";
        importanciaMaterial.text = "";
        riesgoMaterial.text = "";

        switch (pasoActual)
        {
            case 0:

                funcionMaterial.text =
                    "<b>¿Para qué sirve?</b>\n\n" +
                    materialActual.funcion;

                botonContinuar.GetComponentInChildren<TMP_Text>().text = "Siguiente";
                break;

            case 1:

                importanciaMaterial.text =
                    "<b>¿Por qué es importante?</b>\n\n" +
                    materialActual.importancia;

                botonContinuar.GetComponentInChildren<TMP_Text>().text = "Siguiente";
                break;

            case 2:

                riesgoMaterial.text =
                    "<b>¿Qué puede pasar si no se usa correctamente?</b>\n\n" +
                    materialActual.riesgo;

                botonContinuar.GetComponentInChildren<TMP_Text>().text = "Entendido";
                break;
        }
    }
    private void SiguientePaso()
    {
        pasoActual++;

        if (pasoActual > 2)
        {
            CerrarPopup();
        }
        else
        {
            MostrarPaso();
        }
    }

    //--------------------------------------------------------

    public void CerrarPopup()
    {
        panelPopup.SetActive(false);
        nombreMaterial.text = "";
        funcionMaterial.text = "";
        importanciaMaterial.text = "";
        riesgoMaterial.text = "";
        botonContinuar.gameObject.SetActive(false);
        imagenMaterial.enabled = false;

        gameManager.ContinuarJuego();
    }

    //--------------------------------------------------------

    public bool PopupAbierto()
    {
        return panelPopup.activeSelf;
    }

    //--------------------------------------------------------

    public void AbrirPopup()
    {
        panelPopup.SetActive(true);
    }

    //--------------------------------------------------------
    public void OcultarPopup()
    {
        imagenMaterial.enabled = false;

        nombreMaterial.text = "";
        funcionMaterial.text = "";
        importanciaMaterial.text = "";
        riesgoMaterial.text = "";

        botonContinuar.gameObject.SetActive(false);
        panelPopup.SetActive(false);
    }
}
