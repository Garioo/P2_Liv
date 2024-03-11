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
    public StorylineManager storylineManager; // Reference to the StorylineManager script

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
    }

    private void OnVideoFinished(VideoPlayer vp)
{
    // Video finished playing
    GameManager.instance.OnVideoFinished();
}

    public void PlayVideo(string cutsceneTag)
    {
        if (!hasPlayed)
        {
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
}