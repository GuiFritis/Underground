using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyer : EnemyBase
{
    public float speed;
    public float stopFlyghtDistance;
    public bool spriteFacingRight = false;

    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if(Vector2.Distance(transform.position, _target.transform.position) > stopFlyghtDistance)
        {
            sprite.flipX = _target.transform.position.x > transform.position.x ^ spriteFacingRight;
            transform.position += (_target.transform.position - transform.position).normalized * speed * Time.deltaTime;
        }
    }
}
