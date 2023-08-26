using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Floors;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public List<TutorialStep> steps = new();
    public Image panel;
    public float fadeDuration;
    public float delayBetweenSteps = .2f;
    public TextMeshProUGUI textMesh;
    private Color _originalColor;
    private int _currentStep;
    private string _playerPrefTutorial = "TutorialDone";

    void Awake()
    {
        if(PlayerPrefs.GetInt(_playerPrefTutorial, 0) == 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            FloorManager.Instance.delayToStart = fadeDuration * steps.Count * 2 + delayBetweenSteps * steps.Count ;
            foreach (var item in steps)
            {
                FloorManager.Instance.delayToStart += item.duration;
            }
        }
    }

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
        else
        {
            PlayerPrefs.SetInt(_playerPrefTutorial, 1);
            panel.DOColor(Color.clear, fadeDuration).OnComplete(() => gameObject.SetActive(false));
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
