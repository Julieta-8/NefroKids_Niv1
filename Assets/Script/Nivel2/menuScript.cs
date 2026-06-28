using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    //----------------------------------------------------
    // Inicia el juego
    //----------------------------------------------------

    public void Jugar()
    {
        SceneManager.LoadScene("gameScene");
    }

    //----------------------------------------------------
    // Vuelve al menú principal
    //----------------------------------------------------

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("menuScene");
    }

    //----------------------------------------------------
    // Reinicia el nivel actual
    //----------------------------------------------------

    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //----------------------------------------------------
    // Cierra la aplicación
    //----------------------------------------------------

    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}