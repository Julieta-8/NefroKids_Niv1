//using UnityEngine;
//using UnityEngine.UI;

//public class TimeScript : MonoBehaviour
//{
//    [Header("UI")]

//    [SerializeField] private Text tiempoText;

//    [SerializeField] private Text progresoText;

//    [Header("Game Manager")]

//    [SerializeField] private GameManager gameManager;

//    //----------------------------------------------------

//    private float tiempo;

//    private bool contando = true;

//    //----------------------------------------------------

//    void Update()
//    {
//        if (!contando)
//            return;

//        tiempo += Time.deltaTime;

//        MostrarTiempo();
//        MostrarProgreso();
//    }

//    //----------------------------------------------------

//    void MostrarTiempo()
//    {
//        int minutos = Mathf.FloorToInt(tiempo / 60);

//        int segundos = Mathf.FloorToInt(tiempo % 60);

//        tiempoText.text =
//            minutos.ToString("00") +
//            ":" +
//            segundos.ToString("00");
//    }

//    //----------------------------------------------------

//    void MostrarProgreso()
//    {
//        progresoText.text =
//            "Materiales aprendidos: " +
//            gameManager.ObtenerParesEncontrados() +
//            " / " +
//            gameManager.ObtenerTotalPares();
//    }

//    //----------------------------------------------------

//    public void DetenerTiempo()
//    {
//        contando = false;
//    }

//    //----------------------------------------------------

//    public void ReanudarTiempo()
//    {
//        contando = true;
//    }

//    //----------------------------------------------------

//    public void ReiniciarTiempo()
//    {
//        tiempo = 0;

//        contando = true;

//        MostrarTiempo();
//    }

//    //----------------------------------------------------

//    public float ObtenerTiempo()
//    {
//        return tiempo;
//    }
//}