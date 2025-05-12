using UnityEngine;

public class Puerta : MonoBehaviour
{
    public GameObject mensajeSinLlave; // Texto o UI de advertencia

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventario inventario = other.GetComponent<PlayerInventario>();

            if (inventario != null && inventario.tieneLlave)
            {
                Debug.Log("Puerta abierta");
                Destroy(gameObject); // Destruye la puerta
            }
            else
            {
                Debug.Log("Necesitas una llave");
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
