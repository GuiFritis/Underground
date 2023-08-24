using System;
using UnityEngine;

namespace Sounds
{
    public class SFX_Pool : PoolBase<AudioSource, SFX_Pool>
    {
        public void Play(AudioClip clip)
        {
            if(clip != null)
            {
                var item = GetPoolItem();
                item.clip = clip;
                item.Play();
            }
        }

        protected override bool CheckItem(AudioSource item)
        {
            return !item.isPlaying;
        }
    }
}