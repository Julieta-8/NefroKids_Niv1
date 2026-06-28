using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

<<<<<<< HEAD:Assets/Script/Nivel2/gameManager.cs
public class GameManager : MonoBehaviour
{
    [Header("Materiales")]
    [SerializeField] private MaterialDialisis[] materiales;
=======
public class Level_2 : MonoBehaviour {
>>>>>>> 03c4578464c9a690586a94453f54d26c0b72b609:Assets/Script/Nivel2/Level_2.cs

    [Header("Cartas")]
    [SerializeField] private cardScript[] cartas;

<<<<<<< HEAD:Assets/Script/Nivel2/gameManager.cs
    [Header("UI")]
    [SerializeField] private PopupManager popupManager;

    [SerializeField] private Text progresoText;
=======
	private bool _started = false;
    private bool _init  = false;
	private int _matches = 4;

	// Update is called once per frame
	void Update () {
		if (!_started)
            return;
		if (!_init)
			initializeCards ();
>>>>>>> 03c4578464c9a690586a94453f54d26c0b72b609:Assets/Script/Nivel2/Level_2.cs

    [SerializeField] private GameObject panelFinal;

<<<<<<< HEAD:Assets/Script/Nivel2/gameManager.cs
    [SerializeField] private Text textoFinal;
=======
	}
	public void StartLevel()
    {
        _started = true;
    }
>>>>>>> 03c4578464c9a690586a94453f54d26c0b72b609:Assets/Script/Nivel2/Level_2.cs

    //-------------------------------------------------------
    // Variables privadas
    //-------------------------------------------------------

    private cardScript primeraCarta;

    private cardScript segundaCarta;

    private bool bloqueado = false;

    private int paresEncontrados = 0;

    private int totalPares;

    //-------------------------------------------------------

    private void Start()
    {
        totalPares = materiales.Length;

        panelFinal.SetActive(false);

        CrearTablero();

        ActualizarProgreso();
    }

    //-------------------------------------------------------

    public bool PuedeSeleccionar()
    {
        return !bloqueado;
    }

    //-------------------------------------------------------

    public void SeleccionarCarta(cardScript carta)
    {
        if (bloqueado)
            return;

        //---------------------------------------------------
        // Primera carta
        //---------------------------------------------------

        if (primeraCarta == null)
        {
            primeraCarta = carta;
            return;
        }

        //---------------------------------------------------
        // Segunda carta
        //---------------------------------------------------

        segundaCarta = carta;

        StartCoroutine(CompararCartas());
    }

    //-------------------------------------------------------

    void CrearTablero()
    {
        List<MaterialDialisis> lista = new List<MaterialDialisis>();

        //---------------------------------------------------
        // Duplicamos cada material para formar los pares
        //---------------------------------------------------

        foreach (MaterialDialisis material in materiales)
        {
            lista.Add(material);
            lista.Add(material);
        }

        //---------------------------------------------------
        // Mezclamos
        //---------------------------------------------------

        for (int i = 0; i < lista.Count; i++)
        {
            MaterialDialisis aux = lista[i];

            int random = Random.Range(i, lista.Count);

            lista[i] = lista[random];

            lista[random] = aux;
        }

        //---------------------------------------------------
        // Asignamos cada material a una carta
        //---------------------------------------------------

        for (int i = 0; i < cartas.Length; i++)
        {
            cartas[i].Configurar(lista[i]);
        }
    }

    //-------------------------------------------------------

    IEnumerator CompararCartas()
    {
        bloqueado = true;

        yield return new WaitForSeconds(0.8f);

        //---------------------------------------------------
        // ¿Son iguales?
        //---------------------------------------------------

        if (primeraCarta.ObtenerID() == segundaCarta.ObtenerID())
        {
            primeraCarta.MarcarComoEncontrada();

            segundaCarta.MarcarComoEncontrada();

            paresEncontrados++;

            ActualizarProgreso();

            //------------------------------------------------
            // Mostrar información educativa
            //------------------------------------------------

            popupManager.MostrarMaterial(
                primeraCarta.ObtenerMaterial(),
                this
            );

            //------------------------------------------------
            // Esperamos a que el Popup avise que terminó
            //------------------------------------------------

            yield break;
        }

        //---------------------------------------------------
        // No coinciden
        //---------------------------------------------------

        primeraCarta.OcultarCarta();

        segundaCarta.OcultarCarta();

        primeraCarta = null;

        segundaCarta = null;

        bloqueado = false;
    }

    //-------------------------------------------------------

    public void ContinuarJuego()
    {
        primeraCarta = null;

        segundaCarta = null;

        bloqueado = false;

        //---------------------------------------------------
        // ¿Terminó el juego?
        //---------------------------------------------------

        if (paresEncontrados == totalPares)
        {
            FinalizarJuego();
        }
    }

    //-------------------------------------------------------

    void ActualizarProgreso()
    {
        if (progresoText != null)
        {
            progresoText.text =
                "Materiales aprendidos: "
                + paresEncontrados
                + " / "
                + totalPares;
        }
    }
    //-------------------------------------------------------
    // Finaliza la partida
    //-------------------------------------------------------

    void FinalizarJuego()
    {
        bloqueado = true;

        panelFinal.SetActive(true);

        if (textoFinal != null)
        {
            textoFinal.text =
                "¡Felicitaciones!\n\n" +
                "Has identificado correctamente todos los materiales necesarios para la diálisis peritoneal.\n\n" +
                "Ahora conoces su función y la importancia de utilizarlos correctamente.";
        }
    }

    //-------------------------------------------------------
    // Reiniciar partida
    //-------------------------------------------------------

    public void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //-------------------------------------------------------
    // Volver al menú principal
    //-------------------------------------------------------

    public void VolverMenu()
    {
        SceneManager.LoadScene("menuScene");
    }

    //-------------------------------------------------------
    // Salir del juego (solo funciona en la aplicación)
    //-------------------------------------------------------

    public void SalirJuego()
    {
        Application.Quit();
    }

    //-------------------------------------------------------
    // Getters
    //-------------------------------------------------------

    public int ObtenerParesEncontrados()
    {
        return paresEncontrados;
    }

    public int ObtenerTotalPares()
    {
        return totalPares;
    }

    //-------------------------------------------------------
    // Setters
    //-------------------------------------------------------

    public void BloquearJuego(bool estado)
    {
        bloqueado = estado;
    }

    //-------------------------------------------------------
    // Permite volver a jugar sin cambiar de escena
    //-------------------------------------------------------

    public void NuevaPartida()
    {
        primeraCarta = null;
        segundaCarta = null;

        paresEncontrados = 0;

        bloqueado = false;

        panelFinal.SetActive(false);

        CrearTablero();

        ActualizarProgreso();

        foreach (cardScript carta in cartas)
        {
            carta.OcultarCarta();
        }
    }

    //-------------------------------------------------------
    // Devuelve la cantidad de cartas
    //-------------------------------------------------------

    public int CantidadCartas()
    {
        return cartas.Length;
    }

    //-------------------------------------------------------
    // Devuelve la cantidad de materiales
    //-------------------------------------------------------

    public int CantidadMateriales()
    {
        return materiales.Length;
    }
}