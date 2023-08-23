using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorItem : MonoBehaviour
{
    public Collider2D[] colliders;

    void OnValidate()
    {
        if(colliders != null)
        {
            colliders = GetComponents<Collider2D>();
        }
    }

    public void EnableItem()
    {
        if(colliders != null)
        {
            foreach (var item in colliders)
            {
                item.enabled = true;
            }
        }
    }

    public void DisableItem()
    {
        if(colliders != null)
        {
            foreach (var item in colliders)
            {
                item.enabled = false;
            }
        }
    }
}
