using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Celdas"); // Cambia por el nombre real de tu escena de juego
    }
    public void Options()
    {
        SceneManager.LoadScene("Opciones"); // Cambia por el nombre real de tu escena de juego
    }
    public void Volver()
    {
        SceneManager.LoadScene("Menu"); // Cambia por el nombre real de tu escena de juego
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("El juego se cerró."); // Solo visible en editor
    }
}
