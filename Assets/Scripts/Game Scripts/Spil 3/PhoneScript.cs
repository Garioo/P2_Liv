/*
---------------------------
    phoneScript.cs
Description: A script to transition to a target game state when the player clicks on the phone
---------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneScript : MonoBehaviour
{
    // Changed from GameState to GameManager.GameState
    public GameManager.GameState targetState; 
    // Reference to the GameManager
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

    // When the player clicks on the toothbrush
    private void OnMouseDown()
    {
        Debug.Log("Toothbrush clicked");

        // Check if the GameManager object is properly initialized
        if (gameManager != null)
        {
            // Stop the phone ringing sound
            AudioManager.instance.StopAudio("event:/Cutscenes/Telefon Ringer");
            // Transition to the target game state
            gameManager.EnterState(targetState);
            // Play the phone conversation sound
            AudioManager.instance.PlayAudio("event:/Cutscenes/Telefon Samtale");
            Debug.Log("Transitioned to " + targetState);
        }
        else
        {
            Debug.LogError("GameManager object is not initialized.");
        }
    }
}
