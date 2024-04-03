using UnityEngine;

public class Piller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 offset;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Initially, disable gravity for the pills
    }

    private void OnMouseDown()
    {
        // Calculate offset between mouse position and pills position
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        // Move the pills based on mouse position
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

    private void OnMouseUp()
    {
        rb.gravityScale = 1f; // Enable gravity when the mouse button is released
    }
}