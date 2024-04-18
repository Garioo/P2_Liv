/*
------------------------------
    AudioManager.cs
Description: A script to manage audio in the game using FMOD events
------------------------------

Litterature:
    * FMOD Documentation:
        [FMOD Documentation](https://www.fmod.com/docs/2.00/unity/integration-tutorial.html)
    * EventInstance Documentation:
        [EventInstance Documentation](https://www.fmod.com/resources/documentation-api?version=2.0&page=studio-api-eventinstance.html)
    * ChatGPT:
        [ChatGPT](https://openai.com/)

*/

using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // Reference to the AudioManager instance
    public static AudioManager instance;

    // Dictionary to store FMOD event instances by name
    private Dictionary<string, FMOD.Studio.EventInstance> eventInstances = new Dictionary<string, FMOD.Studio.EventInstance>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to play an FMOD event by name
    public void PlayAudio(string eventName)
    {
        // Check if the event name is empty
        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogWarning("Audio event name is empty.");
            return;
        }

        // Check if the event instance already exists
        if (!eventInstances.ContainsKey(eventName))
        {
            // Create a new event instance
            FMOD.Studio.EventInstance eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventName);
            eventInstances[eventName] = eventInstance;
        }

        // Start the event instance
        eventInstances[eventName].start();
    }

    // Method to stop an FMOD event by name
    public void StopAudio(string eventName)
    {
        // Check if the event name is empty
        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogWarning("Audio event name is empty.");
            return;
        }

        // Check if the event instance exists
        if (eventInstances.ContainsKey(eventName))
        {
            // Stop the event instance
            eventInstances[eventName].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            // Release the event instance
            eventInstances[eventName].release();
            // Remove the event instance from the dictionary
            eventInstances.Remove(eventName);
        }
        else
        {
            Debug.LogWarning("No event instance found for audio event: " + eventName);
        }
    }
}
