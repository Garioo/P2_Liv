using UnityEngine;
using Cinemachine;
using UnityEngine.Video;

public class ShakeCamera : MonoBehaviour
{
    public GameManager.GameState targetState;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    public CinemachineVirtualCamera virtualCamera;
    public VideoPlayer initialVideoPlayer; 
    public VideoPlayer shakeVideoPlayer; 
    bool shakeDone;
    private int shakeCount = 0;
    private const int shakeThreshold = 30; // Set the shake threshold to 20 shakes

    private Vector3 originalPos;
    private float shakeDuration = 0f;
    private bool initialVideoPlayed = false;

    void Start()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("Virtual Camera not assigned!");
            enabled = false;
            return;
        }
        originalPos = virtualCamera.transform.localPosition;

        // Play the initial video
        if (initialVideoPlayer != null)
        {
            shakeVideoPlayer.gameObject.SetActive(false);
            initialVideoPlayer.gameObject.SetActive(true);
            GetComponent<VideoPlayer>().isLooping = true;
            initialVideoPlayer.Play();
        }
    }

    void Update()
    {
        if (!initialVideoPlayed && initialVideoPlayer != null && initialVideoPlayer.isPlaying)
        {
            // Check if the initial video is still playing
            return;
        }
        else
        {
            initialVideoPlayed = true;
        }

        if (Input.acceleration.sqrMagnitude >= 3f && Input.acceleration.sqrMagnitude < 20f)
        {
            shakeCount++;
            Shake();
            VibrationManager.Vibrate(10); 
        }
        else if (Input.acceleration.sqrMagnitude >= 5f && Input.acceleration.sqrMagnitude < 20f)
        {
            shakeCount++;
            Shake();
            VibrationManager.Vibrate(40); 
            
            // Switch to the shake video when shaking detected
            if(shakeVideoPlayer != null)
            {
                shakeVideoPlayer.gameObject.SetActive(true);
                shakeVideoPlayer.Play();
                Debug.Log("Shake Video Playing");
                Object.Destroy(initialVideoPlayer.gameObject);
            }
        }
        else if (Input.acceleration.sqrMagnitude >= 20f && !shakeDone || shakeCount >= shakeThreshold)
        {
                Shake();
                VibrationManager.Vibrate(80); 
                Debug.Log("Shake detected");

                GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
                if (gameManager != null)
                {
                    // Transition to the target game state
                    gameManager.EnterState(targetState);
                    AudioManager.instance.PlayAudio("event:/Cutscenes/Liv Vågner");
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
        shakeDuration = 0.5f; 
    }
}
