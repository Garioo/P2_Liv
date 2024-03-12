using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [Serializable]
    public struct VideoInfo
    {
        public string cutsceneTag;
        public VideoClip videoClip;
    }

    public static VideoManager instance; // Singleton instance

    public VideoPlayer videoPlayer; // Assign the VideoPlayer component in the Unity Editor
    public List<VideoInfo> videoInfos; // List of video information (tag and VideoClip)
    public GameManager gameManager; // Reference to the GameManager script

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
        videoPlayer.loopPointReached += OnVideoFinished;

        GameObject camera = GameObject.Find("Main Camera");
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // Notify the GameManager that the video has finished playing
        gameManager.OnVideoFinished();
    }

    public void PlayVideo(string cutsceneTag)
    {
        if (!hasPlayed)
        {
            Debug.Log("Tjekker efter video i arrayen 'videoInfos'");
            foreach (VideoInfo videoInfo in videoInfos)
            {
                if (videoInfo.cutsceneTag == tag)
                {
                    videoPlayer.clip = videoInfo.videoClip;
                    videoPlayer.Play();
                    hasPlayed = true;
                    break;
                }
            }
        }
    }
    public float GetVideoDuration(string cutsceneTag)
    {
        foreach (VideoInfo videoInfo in videoInfos)
        {
            if (videoInfo.cutsceneTag == cutsceneTag)
            {
                return (float)videoInfo.videoClip.length;
            }
        }
        return 0f; // Return 0 if video clip not found
    }
}
