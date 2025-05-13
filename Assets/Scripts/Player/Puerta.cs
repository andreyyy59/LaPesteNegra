using UnityEngine;
using UnityEngine.SceneManagement;

public class Puerta : MonoBehaviour
{
    public GameObject mensajeSinLlave; // ? Aquí arrastras el objeto de texto
    public string nombreEscenaDestino;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventario inventario = other.GetComponent<PlayerInventario>();

            if (inventario != null && inventario.tieneLlave)
            {
                SceneManager.LoadScene(nombreEscenaDestino);
            }
            else
            {
                if (mensajeSinLlave != null)
                    mensajeSinLlave.SetActive(true); // Muestra el mensaje
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && mensajeSinLlave != null)
        {
            mensajeSinLlave.SetActive(false); // Oculta el mensaje
        }
    }
}
