using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Scripts
{
    public class BannerView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup group;
        [SerializeField] private TMP_Text label;
        [SerializeField] private Transform pivot;

        [SerializeField] private float fadeDuration = 0.2f;
        [SerializeField] private float stayDuration = 1f;

        [SerializeField] private Transform start;
        [SerializeField] private Transform end;

        private void Awake()
        {
            @group.alpha = 0;
        }

        public void Show(string text)
        {
            pivot.DOKill(false);
            @group.DOKill(false);

            label.text = text;

            pivot.DOMove(end.position, fadeDuration);

            var sequence = DOTween.Sequence();
            sequence.Append(@group.DOFade(1f, fadeDuration));
            sequence.AppendInterval(stayDuration);
            sequence.AppendCallback(() =>
            {
                @group.DOFade(0f, fadeDuration);
                pivot.DOMove(start.position, fadeDuration);
            });
        }

    }
}
