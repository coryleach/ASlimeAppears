using DG.Tweening;
using UnityEngine;

namespace Game.Scripts
{
    public class TargetCursor : MonoBehaviour
    {

        [SerializeField] private float hideDuration = 0.2f;
        [SerializeField] private float showDuration = 0.2f;
        [SerializeField] private Ease showEaseScale = Ease.OutBack;

        public void Hide(bool instant = false)
        {
            if (instant)
            {
                gameObject.SetActive(false);
                return;
            }

            transform.DOScale(new Vector3(0,0,0), hideDuration).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
        }

        public void Show(Transform targetTransform)
        {
            transform.DOComplete();
            gameObject.SetActive(true);
            transform.localScale = new Vector3(0,0, 0);
            transform.position = targetTransform.position;
            transform.DOScale(Vector3.one, showDuration).SetEase(showEaseScale);
        }

    }
}
