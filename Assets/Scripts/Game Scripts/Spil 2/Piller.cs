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
    // Rigidbody2D component
    private Rigidbody2D rb;
    // Offset between mouse position and pills position
    private Vector3 offset;

    private void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        // Initially, disable gravity for the pills
        rb.gravityScale = 0f; 
    }

    // When the mouse button is pressed
    private void OnMouseDown()
    {
        // Calculate offset between mouse position and pills position
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // When the mouse is dragged
    private void OnMouseDrag()
    {
        // Move the pills based on mouse position
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        // Update the position of the pills
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

    // When the mouse button is released
    private void OnMouseUp()
    {
        // Enable gravity when the mouse button is released
        rb.gravityScale = 1f;
    }
}