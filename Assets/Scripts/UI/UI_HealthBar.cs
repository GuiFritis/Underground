using UnityEngine;
using DG.Tweening;
using System.Collections;

public class UI_HealthBar : MonoBehaviour
{
    public HealthBase health;
    public RectTransform uiHealth;
    public float baseWidth = 100f;
    public float animationDuration = .2f;

    private Coroutine resizeCoroutine;

    void Awake()
    {        
        if(health != null)
        {
            HealthSet();
        }
    }

    public void HealthSet()
    {
        health.OnDamage += Damaged;
    }

    protected virtual void Damaged(HealthBase hp, int damage)
    {
        if(uiHealth != null && health != null)
        {
            if(resizeCoroutine != null)
            {
                StopCoroutine(resizeCoroutine);
            }
            resizeCoroutine = StartCoroutine(AnimateResize());
        }
    }

    private IEnumerator AnimateResize()
    {
        float timer = 0f;
        float currentSize = uiHealth.sizeDelta.x;
        while(timer < animationDuration)
        {
            uiHealth.sizeDelta = new Vector2(
                Mathf.Lerp(currentSize, baseWidth * health.GetCurrentHealth(), timer/animationDuration), 
                uiHealth.sizeDelta.y
            );
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        uiHealth.sizeDelta = new Vector2(
            baseWidth * health.GetCurrentHealth(), 
            uiHealth.sizeDelta.y
        );
    }
}
