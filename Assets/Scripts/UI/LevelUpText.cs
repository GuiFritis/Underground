using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class LevelUpText : MonoBehaviour
{
    public float animationDuration;
    public float textHideDelay;
    // Start is called before the first frame update
    public void ShowLevelUpText()
    {
        transform.DOScale(1, animationDuration).SetEase(Ease.OutBack).OnComplete(
            () => transform.DOScale(0, animationDuration).SetEase(Ease.InBack).SetDelay(textHideDelay)
        );
    }
}
