using TMPro;
using UnityEngine;

namespace Game.Scripts
{
    public class TopPanelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text dayLabel;
        [SerializeField] private TMP_Text coinsLabel;

        private void Start()
        {
            Refresh();
        }

        public void Refresh()
        {
            dayLabel.text = $"Day {GameDataManager.Instance.CurrentData.day}";
            coinsLabel.text = GameDataManager.Instance.CurrentData.coins.ToString("N0");
        }
    }
}
