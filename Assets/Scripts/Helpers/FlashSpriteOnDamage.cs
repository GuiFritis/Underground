using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlashSpriteOnDamage : MonoBehaviour
{
    public HealthBase health;
    public SpriteRenderer sprite;
    public float flashDuration = .3f;
    public Color targetColor = Color.red;
    private Color _initialColor;

    void OnValidate()
    {
        health = GetComponent<HealthBase>();
        if(sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
        }
    }

    void Start()
    {
        _initialColor = sprite.color;
        health.OnDamage += Flash;
    }

    public void Flash(HealthBase hp, int damage)
    {
        if(sprite != null && (hp.GetCurrentHealth() > 0 || !hp.destroyOnDeath))
        {
            sprite.DOKill();
            sprite.color = targetColor;
            sprite.DOColor(_initialColor, flashDuration);
        }
    }
}
