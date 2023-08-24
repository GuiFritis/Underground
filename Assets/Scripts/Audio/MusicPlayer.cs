using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Padrao.Core.Utils;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public List<AudioClip> musicClips;
    public AudioSource audioSource;

    void Awake()
    {
        audioSource.clip = musicClips.GetRandom();
        audioSource.Play();
    }

    void Update()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.clip = musicClips.GetRandom(audioSource.clip);
            audioSource.Play();
        }
    }
}
