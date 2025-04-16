using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscena : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TileTrigger")) // Etiqueta que le pusiste al Tilemap
        {
            SceneManager.LoadScene("Escenepixel 1"); // Cambia al nombre exacto de tu escena
        }
    }
}
