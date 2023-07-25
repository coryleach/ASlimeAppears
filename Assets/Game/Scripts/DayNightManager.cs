using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts
{
    public class DayNightManager : MonoBehaviour
    {
        [SerializeField]
        private Camera skyCamera;

        [SerializeField] private Light lightSource;

        [SerializeField] private Color daytimeSkyColor;
        [SerializeField] private Color nighttimeSkyColor;

        [SerializeField] private Color daytimeLightColor;
        [SerializeField] private Color nighttimeLightColor;

        private float duration = 1f;

        private void Awake()
        {
            InstantDaytime();
        }

        [ContextMenu("Set Day")]
        public void InstantDaytime()
        {
            skyCamera.backgroundColor = daytimeSkyColor;
            lightSource.color = daytimeLightColor;
        }

        [ContextMenu("Set Night")]
        public void InstantNighttime()
        {
            skyCamera.backgroundColor = nighttimeSkyColor;
            lightSource.color = nighttimeLightColor;
        }

        [ContextMenu("Transition Day")]
        public void Day()
        {
            StartCoroutine(ChangeToDay());
        }

        [ContextMenu("Transition Night")]
        public void Night()
        {
            StartCoroutine(ChangeToNight());
        }

        public IEnumerator ChangeToDay()
        {
            skyCamera.DOColor(daytimeSkyColor, duration);
            lightSource.DOColor(daytimeLightColor, duration);
            yield return new WaitForSeconds(duration);
        }

        public IEnumerator ChangeToNight()
        {
            skyCamera.DOColor(nighttimeSkyColor, duration);
            lightSource.DOColor(nighttimeLightColor, duration);
            yield return new WaitForSeconds(duration);
        }
    }
}
