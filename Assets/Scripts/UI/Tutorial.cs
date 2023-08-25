using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    public List<TutorialStep> steps = new();
    public float fadeDuration;
    public float delayBetweenSteps = .2f;
    public TextMeshProUGUI textMesh;
    private Color _originalColor;
    private int _currentStep;

    void Start()
    {
        _originalColor = textMesh.color;
        _currentStep = 0;
        StartCoroutine(TutorialStep());
    }
    
    private IEnumerator TutorialStep()
    {
        textMesh.text = steps[_currentStep].text;
        textMesh.DOColor(_originalColor, fadeDuration);
        yield return new WaitForSeconds(fadeDuration + steps[_currentStep].duration);
        textMesh.DOColor(Color.clear, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        _currentStep++;
        if(_currentStep < steps.Count)
        {
            yield return new WaitForSeconds(delayBetweenSteps);
            StartCoroutine(TutorialStep());
        }
    }
}

[System.Serializable]
public class TutorialStep
{
    [TextArea]
    public string text;
    public float duration;
}
