using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HealthBarEnemy : UI_HealthBar
{
    private float _offsetY = 0f;

    void Update()
    {
        if(health != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(health.transform.position + Vector3.up * _offsetY);
        }
    }

    public void SwitchHealthBase(HealthBase hp, float offsetY)
    {
        if(health != null)
        {
            health.OnLifeChange -= Damaged;
        }
        health = hp;
        HealthSet();
        _offsetY = offsetY;
        Update();
        uiHealth.sizeDelta = new Vector2(
            baseWidth * health.GetCurrentHealth(), 
            uiHealth.sizeDelta.y
        );
    }
}
