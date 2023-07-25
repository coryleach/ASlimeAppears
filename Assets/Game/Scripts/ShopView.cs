using UnityEngine;

namespace Game.Scripts
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup mainGroup;
        [SerializeField] private CanvasGroup shopGroup;
        private bool showing = false;

        private void Awake()
        {
            Hide();
        }

        public void Show()
        {
            shopGroup.alpha = 1;
            mainGroup.alpha = 0;
            gameObject.SetActive(true);
            showing = true;
        }

        public void Hide()
        {
            shopGroup.alpha = 0;
            mainGroup.alpha = 1;
            gameObject.SetActive(false);
            showing = false;
        }

        public void Toggle()
        {
            if (showing)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

    }
}
