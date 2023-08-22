using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorItem : MonoBehaviour
{
    public Collider2D collider2d;

    void OnValidate()
    {
        collider2d = GetComponent<Collider2D>();
    }

    public void EnableItem()
    {
        if(collider2d != null)
        {
            collider2d.enabled = true;
        }
    }

    public void DisableItem()
    {
        if(collider2d != null)
        {
            collider2d.enabled = false;
        }
    }
}
