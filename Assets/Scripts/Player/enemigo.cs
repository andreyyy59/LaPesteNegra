using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscenaAlTocarEnemigo : MonoBehaviour
{
    public string nombreEscena = "Combates";  // Nombre de la escena

    void OnTriggerEnter2D(Collider2D other)
    {
        // Log para verificar si se entra al trigger
        Debug.Log("El jugador ha tocado algo: " + other.gameObject.name);

        if (other.CompareTag("Enemigo"))
        {
            Debug.Log("¡Colisión con el enemigo detectada! Cambiando de escena...");
            SceneManager.LoadScene(nombreEscena);
        }
    }
}
