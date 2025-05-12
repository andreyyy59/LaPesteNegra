using UnityEngine;

public class EnemigoPerseguirJugador : MonoBehaviour
{
    private Transform jugador;
    public float velocidad = 2f;
    public float distanciaVision = 5f;

    private AudioSource audioSource;
    public float stepDelay = 0.7f; // Tiempo entre sonidos
    private float stepTimer;

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

        audioSource = GetComponent<AudioSource>();
        stepTimer = stepDelay;
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia < distanciaVision)
        {
            Vector2 direccion = (jugador.position - transform.position).normalized;
            transform.position += (Vector3)(direccion * velocidad * Time.deltaTime);

            // Sonido por pasos o gruñidos cada cierto tiempo
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                audioSource.PlayOneShot(audioSource.clip);
                stepTimer = stepDelay;
            }
        }
        else
        {
            stepTimer = 0f; // Reiniciar el temporizador si no ve al jugador
        }
    }
}


