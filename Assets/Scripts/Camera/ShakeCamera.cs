/*
--------------------------  
    ShakeCamera.cs
Description: A script to shake the camera when the device is shaking with a force greater than 10.
--------------------------

Literature:
    * Unity Scripting API - CinemachineVirtualCamera: 
        [CinemachineVirtualCamera Documentation](https://docs.unity3d.com/ScriptReference/Cinemachine.CinemachineVirtualCamera.html)
    * Unity Scripting API - Input.acceleration:
        [Input.acceleration Documentation](https://docs.unity3d.com/ScriptReference/Input-acceleration.html)
    * Unity Scripting API - sqr.magnitude:
        [Squared magnitude Documentation](https://docs.unity3d.com/ScriptReference/Vector3-sqrMagnitude.html
    * Youtube Tutorial - How to Shake Camera in Unity:
        [link](hhttps://www.youtube.com/watch?v=0DLSIVOsMv8&ab_channel=AlexanderZotov)
*/
using UnityEngine;
using Cinemachine;
using UnityEngine.Video;

public class ShakeCamera : MonoBehaviour
{
    // The target game state to transition to when the shake is detected
    public GameManager.GameState targetState;
    // The duration of the shake
    private float shakeDuration;
    // The amount of shake
    public float shakeAmount;
    // The decrease factor of the shake
    public float decreaseFactor;
    // The virtual camera to shake
    public CinemachineVirtualCamera virtualCamera;
    // The initial video player
    public VideoPlayer initialVideoPlayer; 
    // The shake video player
    public VideoPlayer shakeVideoPlayer;
    bool shakeDone;
    // The shake count
    private int shakeCount = 0;
    // The shake threshold
    private const int shakeThreshold = 70;
    // The original position of the camera
    private Vector3 originalPos;
    // The initial video played bool
    private bool initialVideoPlayed = false;

    void Start()
    {
        // Check if the virtual camera is assigned
        if (virtualCamera == null)
        {
            Debug.LogError("Virtual Camera not assigned!");
            enabled = false;
            return;
        }
        // Get the original position of the camera
        originalPos = virtualCamera.transform.localPosition;

        // Play the initial video
        if (initialVideoPlayer != null)
        {
            // Disable the shake video player
            shakeVideoPlayer.gameObject.SetActive(false);
            // Enable the initial video player
            initialVideoPlayer.gameObject.SetActive(true);
            // Set the initial video player to loop
            GetComponent<VideoPlayer>().isLooping = true;
            // Play the initial video
            initialVideoPlayer.Play();
        }
    }

    void Update()
    {
        // Check if the shake is done
        if (!initialVideoPlayed && initialVideoPlayer != null && initialVideoPlayer.isPlaying)
        {
            return;
        }
        else
        {
            // Set the initial video played to true
            initialVideoPlayed = true;
        }

        // If the device is shaking with a force between 3 and 10 and the shake count is less than the shake threshold
        if (Input.acceleration.sqrMagnitude >= 3f && Input.acceleration.sqrMagnitude < 10f && shakeCount < shakeThreshold)
        {
            // Increment the shake count
            shakeCount++;
            // Shake the camera
            Shake(0.6f);
            // Vibrate the device
            VibrationManager.Vibrate(10); 
        }
        // If the device is shaking with a force greater than 10 and the shake count is less than the shake threshold
        else if (Input.acceleration.sqrMagnitude >= 10 && shakeCount < shakeThreshold)
        {
            // Increment the shake count
            shakeCount++;
            // Shake the camera
            Shake(0.9f);
            // Vibrate the device
            VibrationManager.Vibrate(40); 
            
            // Switch to the shake video when shaking detected
            if(shakeVideoPlayer != null)
            {
                // Enable the shake video player
                shakeVideoPlayer.gameObject.SetActive(true);
                // Enable the shake video player
                shakeVideoPlayer.Play();
                Debug.Log("Shake Video Playing");
                // Destroy the initial video player
                Object.Destroy(initialVideoPlayer.gameObject);
            }
        }
        
        // If the shake count is equal to the shake threshold
        if (shakeCount == shakeThreshold)
        {
                // Increment the shake count
                shakeCount++;
                // Vibrate the device
                VibrationManager.Vibrate(80); 
                Debug.Log("Shake detected");
                // Get the game manager
                GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
                if (gameManager != null)
                {
                    //Play the audio
                    AudioManager.instance.PlayAudio("event:/Cutscenes/Liv Vågner");
                    // Transition to the target state
                    gameManager.EnterState(targetState);
                    Debug.Log("Transitioned to " + targetState);
                }
                else
                {
                    Debug.LogError("GameManager object is not found in the scene.");
                }
                // Set the shake done to true
                shakeDone = true;
        }
        // If the shake duration is greater than o
        if (shakeDuration > 0)
        {
            // Shake the camera
            virtualCamera.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            // Decrease the shake duration
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            // Reset the shake duration
            shakeDuration = 0f;
            // Reset the camera position
            virtualCamera.transform.localPosition = originalPos;
        }
    }

    void Shake(float shakeForce)
    {
        // Set the shake duration
        shakeDuration = 0.5f; 
        // Set the shake amount equal to the shake force
        shakeAmount = shakeForce;
    }
}
