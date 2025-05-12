using UnityEngine;

public class PlayerInventario : MonoBehaviour
{
    public bool tieneLlave = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Llave"))
        {
            tieneLlave = true;
            Destroy(other.gameObject); // Elimina la llave del mundo
            Debug.Log("¡Llave recogida!");
        }
    }
}
