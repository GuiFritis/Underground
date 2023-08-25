using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVFX_Pool : PoolBase<ParticleSystem, HitVFX_Pool>
{
    public void Play(Vector3 position)
    {
        var item = GetPoolItem();
        item.transform.position = position;
        item.transform.rotation = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
        item.Play();
    }

    protected override bool CheckItem(ParticleSystem item)
    {
        return !item.isPlaying;
    }
}
