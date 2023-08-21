using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageOnTrigger : MonoBehaviour
{
    public LayerMask targetLayer;
    public int damage = 1;
    public float damageRate = 1;
    public bool destroyOnContact = false;
    private float _damageCooldown = 0f;
    private HealthBase _target;

    void Start()
    {
        _damageCooldown = 0f;
    }

    void FixedUpdate()
    {
        if(_damageCooldown > 0)
        {
            _damageCooldown -= Time.fixedDeltaTime;
        }
        else 
        {
            if(_target != null)
            {
                _target.TakeDamage(damage);
                _damageCooldown = damageRate;
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if((targetLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            _target = other.gameObject.GetComponent<HealthBase>();
            if(destroyOnContact)
            {
                _target.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.Equals(_target.gameObject))
        {
            _target = null;
        }
    }
}
