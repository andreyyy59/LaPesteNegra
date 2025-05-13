using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    private Rigidbody2D rb;
    public Animator anim;
    private Vector2 movement;

    private AudioSource audioSource;
    public float stepDelay = 0.5f;
    private float stepTimer;

    public bool tieneLlave = false; // ? Esta es la variable que usará la puerta/llave

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        stepTimer = stepDelay;
    }

    void FixedUpdate()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        rb.velocity = movement * speed;

        anim.SetFloat("x", movement.x);
        anim.SetFloat("y", movement.y);

        // Reproduce sonido de pasos con retardo
        if (movement.magnitude > 0)
        {
            stepTimer -= Time.fixedDeltaTime;

            if (stepTimer <= 0f)
            {
                if (audioSource != null && audioSource.clip != null)
                    audioSource.PlayOneShot(audioSource.clip);

                stepTimer = stepDelay;
            }
        }
        else
        {
            stepTimer = stepDelay; // Reinicia al valor original, no a cero
        }
    }
}



