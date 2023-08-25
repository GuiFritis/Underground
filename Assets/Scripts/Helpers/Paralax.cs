using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    public SpriteRenderer sprite;
    public float speed = .2f;

    void OnValidate()
    {
        if(sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        sprite.size += Vector2.right * speed * Time.deltaTime;
    }
}
