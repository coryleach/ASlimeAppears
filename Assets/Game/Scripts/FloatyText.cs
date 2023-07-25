using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    public class FloatyText : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro text;

        [SerializeField]
        private AnimationCurve positionCurve;

        [SerializeField]
        private AnimationCurve scaleCurve;

        [SerializeField]
        private AnimationCurve alphaCurve;

        [SerializeField] private float duration = 1f;
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vector3 endPosition;
        [SerializeField] private Vector3 startScale = Vector3.one;
        [SerializeField] private Vector3 endScale = Vector3.one;
        [SerializeField] private float startAlpha = 0;
        [SerializeField] private float endAlpha = 1;

        public UnityEvent<FloatyText> OnComplete = new UnityEvent<FloatyText>();

        private float t = 0;

        public string Text
        {
            get => text.text;
            set => text.text = value;
        }

        public float Alpha
        {
            get => TextColor.a;
            set => TextColor = new Color(text.color.r, text.color.g, text.color.b, value);
        }

        public Color TextColor
        {
            get => text.color;
            set => text.color = value;
        }

        public Vector3 Pivot
        {
            get;
            set;
        }

        private void OnEnable()
        {
            t = 0;
            UpdateAnimation(t);
        }

        private void Update()
        {
            t += Time.deltaTime;
            UpdateAnimation(t/duration);
            if (t >= duration)
            {
                enabled = false;
                OnComplete.Invoke(this);
            }
        }

        private void UpdateAnimation(float time)
        {
            var posT = positionCurve.Evaluate(time);
            var scaleT = scaleCurve.Evaluate(time);
            var alphaT = alphaCurve.Evaluate(time);
            transform.position = Pivot + Vector3.Lerp(startPosition, endPosition, posT);
            transform.localScale = Vector3.Lerp(startScale, endScale, scaleT);
            Alpha = Mathf.Lerp(startAlpha, endAlpha, alphaT);
        }

        private void OnValidate()
        {
            if (text == null)
            {
                text = GetComponent<TextMeshPro>();
            }
        }
    }
}
