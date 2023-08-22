using UnityEngine;
using DG.Tweening;

public class UI_HealthBar : MonoBehaviour
{
    public HealthBase health;
    public RectTransform uiHealth;
    public float baseWidth = 100f;

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
        if(uiHealth != null)
        {
            uiHealth.DOKill();
            uiHealth.sizeDelta -= Vector2.right * baseWidth * damage;
        }
    }
}
