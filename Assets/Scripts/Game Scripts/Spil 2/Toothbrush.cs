/*
--------------------------  
    Toothbrush.cs
Description: Script for the toothbrush object in the second game scene
--------------------------
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toothbrush : MonoBehaviour
{
    public GameManager.GameState targetState;
    public GameManager gameManager;

    // Start is called before the first frame update
    public void Start()
    {
        // Find the GameManager object in the scene and get its GameManager component
        gameManager = FindObjectOfType<GameManager>();

        // Check if the GameManager object is found
        if (gameManager == null)
        {
            Debug.LogError("GameManager object not found in the scene.");
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Toothbrush clicked");

        // Check if the GameManager object is properly initialized
        if (gameManager != null)
        {
            // Transition to the target game state
            AudioManager.instance.StopAudio("event:/Liv Vågner");
            gameManager.EnterState(targetState);
            AudioManager.instance.PlayAudio("event:/Telefon Ringer");
            Debug.Log("Transitioned to " + targetState);
        }
        else
        {
            Debug.LogError("GameManager object is not initialized.");
        }
    }
}
