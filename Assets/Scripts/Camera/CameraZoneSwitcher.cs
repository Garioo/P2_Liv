/*
--------------------------  
    CameraZoneSwitcher.cs
Description: A script to switch between different Cinemachine virtual cameras based on trigger events.
--------------------------

Literature:
    * Unity Scripting API - CinemachineVirtualCamera: 
        [CinemachineVirtualCamera Documentation](https://docs.unity3d.com/ScriptReference/Cinemachine.CinemachineVirtualCamera.html)
    * Youtube tutorial by This is GameDev - Cinemachine: 
        [Link](https://www.youtube.com/watch?v=X_vK66w3GJk&ab_channel=ThisisGameDev)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoneSwitcher : MonoBehaviour
{
    // The tag of the trigger object
    public string triggerTag;
    // The primary camera
    public CinemachineVirtualCamera primaryCamera;
    // The list of virtual cameras    
    public CinemachineVirtualCamera[] virtualCameras;

    // When the player enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the trigger zone
        if (other.CompareTag(triggerTag))
        {
            // Get the target camera
            CinemachineVirtualCamera targetCamera = other.GetComponentInChildren<CinemachineVirtualCamera>();
            Debug.Log("Entered trigger. Switching to camera: " + targetCamera.name);
            // Switch to the target camera
            SwitchToCamera(targetCamera);

        }
    }

    // When the player exits the trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player has exited the trigger zone
        if (other.CompareTag(triggerTag))
        {
            Debug.Log("Exited trigger. Switching to primary camera.");
            // Switch to the primary camera
            SwitchToCamera(primaryCamera);
        }
    }

    // Switch to the target camera
    private void SwitchToCamera(CinemachineVirtualCamera targetCamera)
{
    if (targetCamera != null)
    {
        // Enable the target camera and disable all other cameras
        foreach (CinemachineVirtualCamera camera in virtualCameras)
        {
            // Enable the target camera
            camera.enabled = camera == targetCamera;
        }
    }
    else
    {
        Debug.LogError("targetCamera is null!");
    }
}
}
