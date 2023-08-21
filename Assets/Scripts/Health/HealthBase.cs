using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    public int startLife = 5;
    public bool destroyOnDeath = false;
    public float delayToKill = 0f;
    public Action<HealthBase, float> OnDamage;
    public Action<HealthBase> OnDeath;
    private int _currentLife;
    private bool _isDead = false;

    // [SerializeField]
    // private FlashColor _flashColor;

    void Awake()
    {
        Init();
        // if(_flashColor == null){
        //     _flashColor = GetComponent<FlashColor>();
        // }
    }

    void Init()
    {
        _isDead = false;
        _currentLife = startLife;
    }

    public void TakeDamage(int damage){

        if(_isDead){
            return;
        }

        _currentLife -= damage;
        OnDamage?.Invoke(this, damage);

        if(_currentLife <= 0){
            Kill();
        }

        // if(_flashColor != null){
        //     _flashColor.Flash();
        // }
    }

    private void Kill(){
        _isDead = true;
        OnDeath?.Invoke(this);

        if(destroyOnDeath){
            Destroy(gameObject, delayToKill);
        }
    }
}
