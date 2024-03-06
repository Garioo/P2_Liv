using System;
using UnityEngine.Audio;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [System.Serializable]
public class Sound
{
    public SoundType soundType;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;

    [HideInInspector] public AudioSource source;
}

    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Play(SoundType soundType)
    {
        Sound s = Array.Find(this.sounds, sound => sound.soundType == soundType);

        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;

        // We define volume and pitch here to be able to change them in real time
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.Play();
    }
}