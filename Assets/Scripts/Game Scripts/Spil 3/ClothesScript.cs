using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesScript : MonoBehaviour
{
    private Vector3 offset;
    private void OnMouseDrag()
    {
        // Move the pills based on mouse position
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

}
