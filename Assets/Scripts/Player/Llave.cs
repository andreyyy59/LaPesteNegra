using UnityEngine;

public class PlayerInventario : MonoBehaviour
{
    public bool tieneLlave = false;
    public GameObject iconoLlaveUI; // Asignar en el Inspector

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Llave"))
        {
            tieneLlave = true;
            Destroy(other.gameObject); // Elimina la llave del mundo
            if (iconoLlaveUI != null)
                iconoLlaveUI.SetActive(true);
            Debug.Log("¡Llave recogida!");
        }
    }
}
