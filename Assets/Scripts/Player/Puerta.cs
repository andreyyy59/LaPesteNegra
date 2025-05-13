using UnityEngine;
using UnityEngine.SceneManagement;

public class Puerta : MonoBehaviour
{
    public GameObject mensajeSinLlave;
    public string nombreEscenaDestino; // Asignar en el Inspector

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventario inventario = other.GetComponent<PlayerInventario>();

            if (inventario != null && inventario.tieneLlave)
            {
                Debug.Log("Puerta abierta. Cargando escena...");
                SceneManager.LoadScene("SalaMedicos"); // Cambiar de escena
            }
            else
            {
                if (mensajeSinLlave != null)
                    mensajeSinLlave.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && mensajeSinLlave != null)
        {
            mensajeSinLlave.SetActive(false);
        }
    }
}
