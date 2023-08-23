using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : EnemyBase
{
    public float speed = 3f;
    public float outOfSightSpeedMultiplier = .3f;
    public float speedTransitionDuration = .5f;
    public float sightHeight = .25f;
    public float stopChaseDistance = .2f;
    protected float _currentSpeed;
    public bool spriteFacingRight = true;

    void Start()
    {
        _currentSpeed = speed;
    }

    void Update(){
        Chase();
    }

    void FixedUpdate()
    {
        CheckEnemyInSight();
    }

    protected void Chase()
    {
        if(Mathf.Abs(_target.transform.position.x - transform.position.x) > stopChaseDistance)
        {
            sprite.flipX = _target.transform.position.x > transform.position.x ^ spriteFacingRight;
            transform.Translate(Vector2.right * _currentSpeed * Time.deltaTime * (sprite.flipX ? 1 : -1));
        }
        animator.speed = _currentSpeed / speed;
    }

    protected void CheckEnemyInSight()
    {
        if(Mathf.Abs(_target.transform.position.y - transform.position.y) > sightHeight)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, speed * outOfSightSpeedMultiplier, Time.fixedDeltaTime * speedTransitionDuration);
        }
        else 
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, speed, Time.fixedDeltaTime * speedTransitionDuration);
        }
    }
}
