using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthBase))]
public class EnemyBase : MonoBehaviour
{
    public HealthBase health;
    public SpriteRenderer sprite;
    public Animator animator;
    public float healthBarOffsetY = .3f;
    [SerializeField]
    protected Player _target;

    void OnValidate()
    {
        health = GetComponent<HealthBase>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    
    void Awake()
    {
        health.OnDeath += OnDeath;
    }

    public void SetTarget(Player player)
    {
        _target = player;
    }

    protected virtual void OnDeath(HealthBase hp)
    {
        health.OnDeath -= OnDeath;
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * healthBarOffsetY, .1f);
    }
}
