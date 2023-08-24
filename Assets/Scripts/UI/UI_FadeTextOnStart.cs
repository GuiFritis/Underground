using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextFadeHelper))]
public class UI_FadeTextOnStart : MonoBehaviour
{
    public TextFadeHelper textFade;
    public float fadeDelay;

    void OnValidate()
    {
        textFade = GetComponent<TextFadeHelper>();
    }

    void Start()
    {
        Invoke(nameof(FadeText), fadeDelay);
    }

    private void FadeText()
    {
        textFade.FadeOut();
    }
}
