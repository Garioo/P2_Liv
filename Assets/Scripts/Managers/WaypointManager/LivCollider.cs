using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LivCollider : MonoBehaviour
{
    public VenPath VenPath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           Debug.Log("Collided with Liv");
              VenPath.VenWalk = true;
            
        }
    }
}
