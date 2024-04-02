using UnityEngine;
using Cinemachine;

public class ShakeCamera : MonoBehaviour
{
    public GameManager.GameState targetState;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    public CinemachineVirtualCamera virtualCamera;
    bool shakeDone;

    private Vector3 originalPos;
    private float shakeDuration = 0f;

    void Start()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("Virtual Camera not assigned!");
            enabled = false;
            return;
        }
        originalPos = virtualCamera.transform.localPosition;
    }

    void Update()
    {
        if (Input.acceleration.sqrMagnitude >= 3f && Input.acceleration.sqrMagnitude < 30f) // Adjust this value as needed for your desired sensitivity
        {
            Shake();
            VibrationManager.Vibrate(50); // Vibrate for 50 milliseconds
        }
        else if (Input.acceleration.sqrMagnitude >= 30f && !shakeDone) // Adjust this value as needed for your desired sensitivity
        {
            Shake();
            VibrationManager.Vibrate(80); // Vibrate for 80 milliseconds
            Debug.Log("Shake detected");

            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                // Transition to the target game state
                gameManager.EnterState(targetState);
                Debug.Log("Transitioned to " + targetState);
            }
            else
            {
                Debug.LogError("GameManager object is not found in the scene.");
            }
            shakeDone = true;
        }
        if (shakeDuration > 0)
        {
            virtualCamera.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }

        else
        {
            shakeDuration = 0f;
            virtualCamera.transform.localPosition = originalPos;
        }
    }

    void Shake()
    {
        shakeDuration = 0.5f; // Adjust the duration of the shake
    }
}
