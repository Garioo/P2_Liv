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
    // The tag of the trigger object that will activate camera switching
    public string triggerTag;

    // The primary camera that will be active when not in a trigger zone
    public CinemachineVirtualCamera primaryCamera;

    // An array of virtual cameras to switch between
    public CinemachineVirtualCamera[] virtualCameras;

    private void Start()
    {
        // Initialization code can be added here if needed
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the trigger object has the specified tag
        if (other.CompareTag(triggerTag))
        {
            // Get the CinemachineVirtualCamera component from the trigger object
            CinemachineVirtualCamera targetCamera = other.GetComponentInChildren<CinemachineVirtualCamera>();

            // Log the name of the camera being switched to
            Debug.Log("Entered trigger. Switching to camera: " + targetCamera.name);

            // Switch to the target camera
            SwitchToCamera(targetCamera);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the trigger object has the specified tag
        if (other.CompareTag(triggerTag))
        {
            // Log that the trigger has been exited
            Debug.Log("Exited trigger. Switching to primary camera.");

            // Switch back to the primary camera
            SwitchToCamera(primaryCamera);
        }
    }

    private void SwitchToCamera(CinemachineVirtualCamera targetCamera)
    {
        // Check if the target camera is not null
        if (targetCamera != null)
        {
            // Iterate through all virtual cameras
            foreach (CinemachineVirtualCamera camera in virtualCameras)
            {
                // Enable the target camera and disable all others
                camera.enabled = camera == targetCamera;
            }
        }
        else
        {
            // Log an error if the target camera is null
            Debug.LogError("targetCamera is null!");
        }
    }
}
