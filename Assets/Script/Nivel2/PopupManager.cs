//using UnityEngine;
//using UnityEngine.UI;

//public class PopupManager : MonoBehaviour
//{
//    [Header("Panel")]
//    [SerializeField] private GameObject panelPopup;

//    [Header("Imagen")]
//    [SerializeField] private Image imagenMaterial;

//    [Header("Textos")]
//    [SerializeField] private Text nombreMaterial;
//    [SerializeField] private Text funcionMaterial;
//    [SerializeField] private Text importanciaMaterial;
//    [SerializeField] private Text riesgoMaterial;

//    [Header("Botón")]
//    [SerializeField] private Button botonContinuar;

//    //Referencia al GameManager
//    private GameManager gameManager;

//    //--------------------------------------------------------

//    private void Start()
//    {
//        panelPopup.SetActive(false);

//        botonContinuar.onClick.AddListener(CerrarPopup);
//    }

//    //--------------------------------------------------------

//    public void MostrarMaterial(MaterialDialisis material, GameManager gm)
//    {
//        gameManager = gm;

//        //Pausa el juego
//        gameManager.BloquearJuego(true);

//        //Completa la información

//        imagenMaterial.sprite = material.imagen;

//        nombreMaterial.text = material.nombre;

//        funcionMaterial.text =
//            "<b>Función</b>\n\n" +
//            material.funcion;

//        importanciaMaterial.text =
//            "<b>Importancia</b>\n\n" +
//            material.importancia;

//        riesgoMaterial.text =
//            "<b>¿Qué puede ocurrir si no se utiliza correctamente?</b>\n\n" +
//            material.riesgo;

//        panelPopup.SetActive(true);
//    }

//    //--------------------------------------------------------

//    public void CerrarPopup()
//    {
//        panelPopup.SetActive(false);

//        gameManager.ContinuarJuego();
//    }

//    //--------------------------------------------------------

//    public bool PopupAbierto()
//    {
//        return panelPopup.activeSelf;
//    }

//    //--------------------------------------------------------

//    public void AbrirPopup()
//    {
//        panelPopup.SetActive(true);
//    }

//    //--------------------------------------------------------

//    public void OcultarPopup()
//    {
//        panelPopup.SetActive(false);
//    }
//}
