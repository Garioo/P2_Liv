/*
--------------------------  
    Piller.cs
Description: This script is used to move the pills in the game. 
It allows the player to drag and drop the pills to the desired location.
--------------------------

Literature:
    * Unity Tutorial - Drag and Drop: 
        [Drag and Drop Tutorial](https://www.youtube.com/watch?v=yalbvB84kCg&ab_channel=BMo)
    * Unity Documentation - Rigidbody2D:
        [Rigidbody2D](https://docs.unity3d.com/ScriptReference/Rigidbody2D.html)
    * ChatGPT:
        [ChatGPT](https://openai.com/)
*/

using UnityEngine;

public class Piller : MonoBehaviour
{
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Vector3 offset; // Offset between mouse position and pill's position

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the pill
        rb.gravityScale = 0f; // Initially, disable gravity for the pills
    }

    private void OnMouseDown()
    {
        // Calculate offset between mouse position and pill's position
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        // Move the pill based on mouse position
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

    private void OnMouseUp()
    {
        rb.gravityScale = 1f; // Enable gravity when the mouse button is released
    }
}