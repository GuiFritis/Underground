using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Padrao.Core.Utils;
using Padrao.Core.Singleton;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : Singleton<MusicPlayer>
{
    public List<AudioClip> musicClips;
    public AudioSource audioSource;

    private bool _playing = true;

    protected override void Awake()
    {
        base.Awake();
        PlayMusic();
    }

    void Update()
    {
        if(_playing && !audioSource.isPlaying)
        {
            PlayMusic();
        }
    }

    public void PlayMusic(bool changeMusic = true)
    {
        if(changeMusic)
        {
            if(audioSource.clip != null)
            {
                audioSource.clip = musicClips.GetRandom(audioSource.clip);
            }
            else 
            {
                audioSource.clip = musicClips.GetRandom();
            }
        }
        _playing = true;
        audioSource.Play();
    }

    public void StopMusic()
    {
        _playing = false;
        audioSource.Stop();
    }
}
