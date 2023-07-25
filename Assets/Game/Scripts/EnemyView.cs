using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace Game.Scripts
{
    /// <summary>
    /// Displays an enemy on the combat field
    /// </summary>
    public class EnemyView : TargetView
    {
        [SerializeField] private Slider healthSlider;

        private EnemyInstance enemyInstance = null;
        public EnemyInstance EnemyInstance => enemyInstance;

        public void SetEnemy(EnemyInstance enemy)
        {
            enemyInstance = enemy;
            Instantiate(enemyInstance.EnemyInfo.displayPrefab, modelPivot);
            AnimateModelSpawn();
            Refresh();
        }

        public void Refresh()
        {
            healthSlider.value = enemyInstance.health / (float)enemyInstance.EnemyInfo.maxHealth;
        }

        [SerializeField] private Vector3 punchScale = new Vector3(0.1f, 0.1f, 0.1f);

        public void AnimateAttack(Transform target)
        {
            modelPivot.DOPunchPosition(target.position - modelPivot.transform.position, 0.25f);
        }

        private void AnimateModelSpawn()
        {
            modelPivot.localScale = Vector3.zero;
            modelPivot.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        public void Damage(int amount)
        {
            enemyInstance.health -= amount;
            FloatyTextManager.Instance.SpawnText($"{amount}", Color.red, modelPivot.position);
            modelPivot.DOPunchScale(punchScale, 0.2f);
        }

        public override bool CanAcceptDraggable(Draggable draggable)
        {
            var cardView = draggable as CardView;
            if (cardView == null)
            {
                return false;
            }
            return cardView.CardInstance.CardInfo.IsType(CardType.Attack);
        }

        public override void AcceptDraggable(Draggable draggable)
        {
            var cardView = draggable as CardView;
            if (cardView == null)
            {
                return;
            }

            var card = cardView.CardInstance;

            CombatManager.Instance.Discard(card);

            if (enemyInstance.health > 0)
            {
                Damage(card.CardInfo.damage);
                Refresh();
            }

            if (enemyInstance.health <= 0)
            {
                CombatManager.Instance.OnEnemyKilled(enemyInstance);
            }
        }

    }
}
