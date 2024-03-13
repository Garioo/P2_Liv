using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoneSwitcher : MonoBehaviour
{
    public string triggerTag;

    public CinemachineVirtualCamera primaryCamera;

    public CinemachineVirtualCamera[] virtualCameras;

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(triggerTag))
        {
            CinemachineVirtualCamera targetCamera = other.GetComponentInChildren<CinemachineVirtualCamera>();
            Debug.Log("Entered trigger. Switching to camera: " + targetCamera.name);
            SwitchToCamera(targetCamera);

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(triggerTag))
        {
            Debug.Log("Exited trigger. Switching to primary camera.");
            SwitchToCamera(primaryCamera);
        }
    }

    private void SwitchToCamera(CinemachineVirtualCamera targetCamera)
    {
        foreach (CinemachineVirtualCamera camera in virtualCameras)
        {
            camera.enabled = camera == targetCamera;
        }
    }
}
