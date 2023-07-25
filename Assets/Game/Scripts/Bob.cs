using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts
{
    public class Bob : MonoBehaviour
    {
        public float amount = 1f;
        public float speed = 1f;
        public Ease easeType = Ease.InOutSine;

        private Tween currentTween = null;
        private float? startY;

        private void Awake()
        {
            startY = transform.localPosition.y;
        }

        private void OnEnable()
        {
            Play();
        }

        private void Play()
        {
            Stop();

            if (speed == 0)
            {
                return;
            }

            var endY = transform.localPosition.y;
            endY += amount;
            currentTween = transform.DOLocalMoveY(endY, 1 / speed).SetLoops(-1, LoopType.Yoyo).SetEase(easeType);
        }

        private void Stop()
        {
            currentTween?.Kill();
            currentTween = null;
            ResetPosition();
        }

        private void ResetPosition()
        {
            if (!startY.HasValue)
            {
                return;
            }
            var pt = transform.localPosition;
            pt.y = startY.Value;
            transform.localPosition = pt;
        }

        // private void OnValidate()
        // {
        //     if (Application.isPlaying)
        //     {
        //         Play();
        //     }
        // }
    }
}
