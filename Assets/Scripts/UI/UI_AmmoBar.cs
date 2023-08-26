using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_AmmoBar : MonoBehaviour
{
    public Player player;
    public Image sprite;
    public float baseWidth = 15f;
    public Color targetColor = Color.red;
    public float flashDuration = .3f;
    private Color _initialColor;

    void Start()
    {
        player.OnShoot += OnShoot;
        _initialColor = sprite.color;
    }

    private void OnShoot(int totalAmmo, int shots)
    {
        sprite.rectTransform.sizeDelta = new Vector2(
            baseWidth * (totalAmmo - shots + 1),
            sprite.rectTransform.sizeDelta.y
        );
        if(shots == totalAmmo)
        {
            sprite.DOColor(targetColor, flashDuration).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            sprite.DOKill();
            sprite.DOColor(_initialColor, flashDuration);
        }
    }
}
