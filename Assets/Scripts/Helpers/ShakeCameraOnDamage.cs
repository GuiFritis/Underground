using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShakeCameraOnDamage : MonoBehaviour
{
    public HealthBase playerHealth;
    public Transform cameraTransform;
    public float shakeDuration = .3f;
    public float shakeForce = .01f;
    
    void Start()
    {
        playerHealth.OnDamage += ShakeCamera;
    }

    private void ShakeCamera(HealthBase hp, int damage)
    {
        cameraTransform.DOKill();
        cameraTransform.position = new Vector3(0f, cameraTransform.position.y, cameraTransform.position.z);
        cameraTransform.DOShakePosition(shakeDuration, shakeForce);
    }
}
