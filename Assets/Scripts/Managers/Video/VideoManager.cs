/*
This script is used to manage the video in the game. It provides methods to play and stop VideoClips by name.

Litterature:
Link for VideoPlayer: https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Video.VideoPlayer.html
Link For VideoClip: https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Video.VideoClip.html

*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

// VideoManager class to manage video playback
public class VideoManager : MonoBehaviour
{
    // Struct to store video information
    [Serializable]
    public struct VideoInfo
    {
        // Tag to identify the video
        public string cutsceneTag;
        // VideoClip to play
        public VideoClip videoClip;
    }

    // Singleton instance of the VideoManager
    public static VideoManager instance;

    // Reference to the VideoPlayer component in the scene
    public VideoPlayer videoPlayer;
    // List of video information to play
    public List<VideoInfo> videoInfos; 
    // Reference to the GameManager script
    public GameManager gameManager;

    // Bool to check if the video has played
    private bool hasPlayed;

    void Awake()
    {
        // Ensure only one instance of VideoManager exists
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

    void Start()
    {
        // Subscribe to the loopPointReached event
        videoPlayer.loopPointReached += OnVideoFinished;
    }

   // Method to handle the video finished event
    private void OnVideoFinished(VideoPlayer vp)
{
    // Null check for gameManager
    if (gameManager == null)
    {
        Debug.LogError("GameManager is not assigned in the VideoManager.");
        return;
    }

    // Notify the GameManager that the video has finished playing
    Debug.Log("Video finished");
    // Transition to the next game state
    gameManager.VideoNextState();
    // Reset the hasPlayed flag
    hasPlayed = false;
}

    // Method to play a video by cutscene tag   
    public void PlayVideo(string cutsceneTag)
    {
        // Find the main camera in the scene
        Camera mainCamera = Camera.main;
        
        // Check if the main camera is found
        if (mainCamera != null) 
        {   
            // Set the video player render mode to CameraNearPlane
            videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            // Set the target camera for the video player
            videoPlayer.targetCamera = mainCamera;
            
        }
        else
        {
            Debug.LogWarning("Main camera not found. VideoPlayer render mode not set.");
        }
        // Check if the video has played
        if (!hasPlayed)
        {
            Debug.Log("Tjekker efter video i arrayen 'videoInfos'");
            // Loop through the videoInfos list
            foreach (VideoInfo videoInfo in videoInfos)
            {
                // Check if the cutscene tag matches
                if (videoInfo.cutsceneTag == cutsceneTag)
                {
                    // Set the video clip and play the video
                    videoPlayer.clip = videoInfo.videoClip;
                    // Play the video
                    videoPlayer.Play();
                    Debug.Log("Afspiller video");
                    // Set the hasPlayed flag to true
                    hasPlayed = true;
                    break;
                }
            }
        }
    }
    // Method to stop the video playback
    public float GetVideoDuration(string cutsceneTag)
    {
        // Loop through the videoInfos list
        foreach (VideoInfo videoInfo in videoInfos)
        {
            // Check if the cutscene tag matches
            if (videoInfo.cutsceneTag == cutsceneTag)
            {
                // Return the duration of the video clip
                return (float)videoInfo.videoClip.length;
            }
        }
        // Return 0 if video clip not found
        return 0f;
    }
}
