/*
--------------------------  
    ClothesScript.cs
Description: Script for moving clothes in the game
--------------------------

Literature:
    * Unity Tutorial - Drag and Drop: 
        [Drag and Drop Tutorial](https://www.youtube.com/watch?v=yalbvB84kCg&ab_channel=BMo)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesScript : MonoBehaviour
{
    // Offset between mouse position and pills position
    private Vector3 offset; 
    private void OnMouseDrag()
    {
        // Calculate offset between mouse position and clothes position
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        // Update the position of the clothes
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

}
