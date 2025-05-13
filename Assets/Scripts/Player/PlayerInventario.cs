using UnityEngine;
using System.Collections; // ← ¡Esta línea es clave!

public class Llave : MonoBehaviour
{
    public GameObject mensajeLlave;
    public float duracionMensaje = 2f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventario inventario = other.GetComponent<PlayerInventario>();
            if (inventario != null)
            {
                inventario.tieneLlave = true;

                if (mensajeLlave != null)
                    StartCoroutine(MostrarMensajeTemporal());

                Destroy(gameObject);
            }
        }
    }

    IEnumerator MostrarMensajeTemporal()
    {
        mensajeLlave.SetActive(true);
        yield return new WaitForSeconds(duracionMensaje);
        mensajeLlave.SetActive(false);
    }
}
