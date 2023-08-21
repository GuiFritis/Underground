using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthBase))]
public class EnemyBase : MonoBehaviour
{
    public HealthBase health;
    public SpriteRenderer sprite;
    public Animator animator;
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

    public void SetPlayer(Player player)
    {
        _target = player;
    }

    protected virtual void OnDeath(HealthBase hp)
    {
        health.OnDeath -= OnDeath;
        Destroy(gameObject);
    }
}
