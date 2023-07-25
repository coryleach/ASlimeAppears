using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public bool loop = true;
    public float speed = 1;
    public Ease easeType = Ease.Linear;

    private Tween rotationTween = null;

    private void OnEnable()
    {
        StartRotation();
    }

    private void OnDisable()
    {
        rotationTween.Kill();
        rotationTween = null;
    }

    private void StartRotation()
    {
        //Do nothing if speed is invalid
        if (speed == 0)
        {
            return;
        }
        rotationTween = transform.DOBlendableRotateBy(new Vector3(0,180,0), 1 / speed).SetLoops(loop ? -1 : 0).SetEase(easeType);
    }

    // private void OnValidate()
    // {
    //     if (Application.isPlaying)
    //     {
    //         rotationTween?.Kill();
    //         StartRotation();
    //     }
    // }
}
