using UnityEngine;

namespace Game.Scripts
{
    public class TargetManager : MonoBehaviour
    {
        private static TargetManager instance;
        public static TargetManager Instance => instance;

        [SerializeField]
        private TargetCursor cursor;

        private void Awake()
        {
            instance = this;
            cursor.Hide(true);
        }

        public void SetTarget(EnemyView enemyView)
        {
            if (enemyView == null)
            {
                cursor.Hide();
            }
            else
            {
                cursor.Show(enemyView.transform);
            }
        }

        public void SetTarget(TargetView view)
        {
            if (view == null)
            {
                cursor.Hide();
            }
            else
            {
                cursor.Show(view.transform);
            }
        }
    }
}
