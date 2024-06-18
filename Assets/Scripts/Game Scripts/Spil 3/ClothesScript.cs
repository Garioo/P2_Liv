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
    private Vector3 offset;
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

}
