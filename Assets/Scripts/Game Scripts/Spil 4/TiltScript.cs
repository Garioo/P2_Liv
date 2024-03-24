using UnityEngine;
// https://www.youtube.com/watch?v=wpSm2O2LIRM&t=9s&ab_channel=AlexanderZotov
public class TiltScript : MonoBehaviour
{
   Rigidbody2D rb;
   float dirx;
   public float moveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        dirx = Input.acceleration.x * moveSpeed;
       transform.position = new Vector2(Mathf.Clamp(transform.position.x, -6.5f, 6.5f), transform.position.y);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirx, 0f);
    }
}
