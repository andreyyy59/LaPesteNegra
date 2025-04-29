using UnityEngine;

public class EnemigoPerseguirJugador : MonoBehaviour
{
    private Transform jugador;
    public float velocidad = 2f;
    public float distanciaVision = 5f;

    void Start()
    {
        GameObject objJugador = GameObject.Find("player");
        if (objJugador != null)
        {
            jugador = objJugador.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró un GameObject llamado 'player'");
        }
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia < distanciaVision)
        {
            Vector2 direccion = (jugador.position - transform.position).normalized;
            transform.position += (Vector3)(direccion * velocidad * Time.deltaTime);
        }
    }
}

