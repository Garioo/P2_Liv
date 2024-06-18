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

using UnityEngine; // Import the UnityEngine namespace
using Cinemachine; // Import the Cinemachine namespace
using UnityEngine.Video; // Import the UnityEngine.Video namespace

public class ShakeCamera : MonoBehaviour // Declare a public class named ShakeCamera that inherits from MonoBehaviour
{
    public GameManager.GameState targetState; // Declare a public variable named targetState of type GameManager.GameState
    private float shakeDuration; // Declare a private variable named shakeDuration of type float
    public float shakeAmount; // Declare a public variable named shakeAmount of type float
    public float decreaseFactor; // Declare a public variable named decreaseFactor of type float
    public CinemachineVirtualCamera virtualCamera; // Declare a public variable named virtualCamera of type CinemachineVirtualCamera
    public VideoPlayer initialVideoPlayer; // Declare a public variable named initialVideoPlayer of type VideoPlayer
    public VideoPlayer shakeVideoPlayer; // Declare a public variable named shakeVideoPlayer of type VideoPlayer
    bool shakeDone; // Declare a private variable named shakeDone of type bool
    private int shakeCount = 0; // Declare a private variable named shakeCount and initialize it to 0
    private const int shakeThreshold = 70; // Declare a private constant variable named shakeThreshold and initialize it to 70
    private Vector3 originalPos; // Declare a private variable named originalPos of type Vector3
    private bool initialVideoPlayed = false; // Declare a private variable named initialVideoPlayed and initialize it to false

    void Start() // Declare a method named Start with no parameters and no return type
    {
        if (virtualCamera == null) // Check if virtualCamera is null
        {
            Debug.LogError("Virtual Camera not assigned!"); // Log an error message
            enabled = false; // Disable the script
            return; // Exit the method
        }
        originalPos = virtualCamera.transform.localPosition; // Assign the local position of virtualCamera to originalPos

        // Play the initial video
        if (initialVideoPlayer != null) // Check if initialVideoPlayer is not null
        {
            shakeVideoPlayer.gameObject.SetActive(false); // Deactivate the shakeVideoPlayer game object
            initialVideoPlayer.gameObject.SetActive(true); // Activate the initialVideoPlayer game object
            GetComponent<VideoPlayer>().isLooping = true; // Set the isLooping property of the VideoPlayer component to true
            initialVideoPlayer.Play(); // Play the initial video
        }
    }

    void Update() // Declare a method named Update with no parameters and no return type
    {
        if (!initialVideoPlayed && initialVideoPlayer != null && initialVideoPlayer.isPlaying) // Check if initialVideoPlayed is false and initialVideoPlayer is not null and is playing
        {
            // Check if the initial video is still playing
            return; // Exit the method
        }
        else
        {
            initialVideoPlayed = true; // Set initialVideoPlayed to true
        }

        if (Input.acceleration.sqrMagnitude >= 3f && Input.acceleration.sqrMagnitude < 10f && shakeCount < shakeThreshold) // Check if the squared magnitude of the acceleration input is greater than or equal to 3, less than 10, and shakeCount is less than shakeThreshold
        {
            shakeCount++; // Increment shakeCount by 1
            Shake(0.6f); // Call the Shake method with a shakeForce of 0.6f
            VibrationManager.Vibrate(10); // Call the Vibrate method from the VibrationManager class with a duration of 10
        }
        else if (Input.acceleration.sqrMagnitude >= 10 && shakeCount < shakeThreshold) // Check if the squared magnitude of the acceleration input is greater than or equal to 10 and shakeCount is less than shakeThreshold
        {
            shakeCount++; // Increment shakeCount by 1
            Shake(0.9f); // Call the Shake method with a shakeForce of 0.9f
            VibrationManager.Vibrate(40); // Call the Vibrate method from the VibrationManager class with a duration of 40

            // Switch to the shake video when shaking detected
            if (shakeVideoPlayer != null) // Check if shakeVideoPlayer is not null
            {
                shakeVideoPlayer.gameObject.SetActive(true); // Activate the shakeVideoPlayer game object
                shakeVideoPlayer.Play(); // Play the shake video
                Debug.Log("Shake Video Playing"); // Log a message
                Object.Destroy(initialVideoPlayer.gameObject); // Destroy the initialVideoPlayer game object
            }
        }

        if (shakeCount == shakeThreshold) // Check if shakeCount is equal to shakeThreshold
        {
            shakeCount++; // Increment shakeCount by 1
            VibrationManager.Vibrate(80); // Call the Vibrate method from the VibrationManager class with a duration of 80
            Debug.Log("Shake detected"); // Log a message

            GameManager gameManager = GameObject.FindObjectOfType<GameManager>(); // Find an object of type GameManager in the scene and assign it to gameManager
            if (gameManager != null) // Check if gameManager is not null
            {
                // Transition to the target game state
                AudioManager.instance.PlayAudio("event:/Liv Vågner"); // Play an audio clip using the AudioManager instance
                gameManager.EnterState(targetState); // Call the EnterState method of the gameManager with the targetState as the parameter
                Debug.Log("Transitioned to " + targetState); // Log a message
            }
            else
            {
                Debug.LogError("GameManager object is not found in the scene."); // Log an error message
            }
            shakeDone = true; // Set shakeDone to true
        }

        if (shakeDuration > 0) // Check if shakeDuration is greater than 0
        {
            virtualCamera.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount; // Set the local position of virtualCamera to originalPos plus a random vector inside a unit sphere multiplied by shakeAmount

            shakeDuration -= Time.deltaTime * decreaseFactor; // Decrease shakeDuration over time
        }
        else
        {
            shakeDuration = 0f; // Set shakeDuration to 0
            virtualCamera.transform.localPosition = originalPos; // Reset the local position of virtualCamera to originalPos
        }
    }

    void Shake(float shakeForce) // Declare a method named Shake with a parameter named shakeForce of type float and no return type
    {
        shakeDuration = 0.5f; // Set shakeDuration to 0.5f
        shakeAmount = shakeForce; // Set shakeAmount to shakeForce
    }
}
