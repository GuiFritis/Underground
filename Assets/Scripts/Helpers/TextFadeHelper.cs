using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextFadeHelper : MonoBehaviour
{
    public float fadeDuration;
    public TextMeshProUGUI textMesh;
    public Color targetColor;

    public void FadeIn()
    {
        textMesh.DOColor(targetColor, fadeDuration);
    }

    public void FadeOut()
    {
        textMesh.DOColor(Color.clear, fadeDuration);
    }
}
