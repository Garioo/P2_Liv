/* 
--------------------------  
    TiltScript.cs
Description: Script for tilting an object based on device acceleration
--------------------------

Literature:
    * Unity Youtube Tutorial by Alexander Zotov- Accelerometer and Gyroscope:
        [Link](https://www.youtube.com/watch?v=wpSm2O2LIRM&t=9s&ab_channel=AlexanderZotov)
*/
using UnityEngine;

public class TiltScript : MonoBehaviour
{
    // Rigidbody2D component
   Rigidbody2D rb;
   // Direction of the object
   float dirx;
   // Movement speed of the object
   public float moveSpeed;


    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        // Set the movement speed of the object to a random value between 3 and 18
        moveSpeed = Random.Range(3f, 18f);
    }

    void Update()
    {
        // Update the direction of the object based on the acceleration of the device
        dirx = Input.acceleration.x * moveSpeed;
        // Clamp the position of the object between -6.5 and 6.5
       transform.position = new Vector2(Mathf.Clamp(transform.position.x, -6.5f, 6.5f), transform.position.y);
    }

    void FixedUpdate()
    {
        // Move the object based on the direction and movement speed
        rb.velocity = new Vector2(dirx, 0f);
    }
}
