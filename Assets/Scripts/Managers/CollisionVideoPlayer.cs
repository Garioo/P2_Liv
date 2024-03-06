using UnityEngine;
using UnityEngine.Video;

public class CollisionVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign the VideoPlayer component in the Unity Editor
    public string videoPath; // Specify the path of the video file
    public string targetTag; // Tag of the trigger object
    public StorylineManager storylineManager; // Reference to the StorylineManager script

    private bool hasPlayed;

    void Start()
    {
        videoPlayer.url = videoPath;
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // Video finished playing
        storylineManager.LoadNextScene();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            videoPlayer.Play();
            hasPlayed = true;
        }
    }
}