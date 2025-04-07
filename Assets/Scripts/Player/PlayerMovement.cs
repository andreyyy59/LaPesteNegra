using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    private Rigidbody2D rb;
    public Animator anim;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Asigna el Rigidbody2D automáticamente
    }

    void FixedUpdate()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        rb.velocity = movement * speed;

        anim.SetFloat("x", movement.x);
        anim.SetFloat("y", movement.y);
    }
}


