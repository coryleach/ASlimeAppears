using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public class CombatFieldView : MonoBehaviour
    {
        [SerializeField] private EnemyView enemyViewPrefab;
        [SerializeField] private AllyView allyViewPrefab;

        [SerializeField] private List<Transform> enemySpawnLocations = new List<Transform>();
        [SerializeField] private List<Transform> allySpawnLocations = new List<Transform>();

        private List<EnemyView> enemies = new List<EnemyView>();
        public IReadOnlyList<EnemyView> Enemies => enemies;

        private List<AllyView> allies = new List<AllyView>();
        public IReadOnlyList<AllyView> Allies => allies;

        public int AllyCount => allies.Count(x => x.IsOccupied);
        public int MaxEnemies => enemySpawnLocations.Count;

        public void RemoveEnemy(EnemyInstance enemyInstance)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                var enemyView = enemies[i];
                if (enemyView.EnemyInstance != enemyInstance)
                {
                    continue;
                }

                enemies.RemoveAt(i);

                var sequence = DOTween.Sequence();
                sequence.Append(enemyView.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack));
                sequence.AppendInterval(1f);
                sequence.AppendCallback(() => Destroy(enemyView.gameObject));

                return;
            }
        }

        public void AddEnemy(EnemyInstance enemyInstance)
        {
            if (enemies.Count >= MaxEnemies)
            {
                return;
            }

            var enemyView = Instantiate(enemyViewPrefab, transform);
            enemyView.SetEnemy(enemyInstance);

            var location = enemySpawnLocations.First(x =>
            {
                return !enemies.Any(e => Vector3.Distance(e.transform.position,x.position) < 0.01f);
            });

            enemyView.transform.position = location.position;
            enemies.Add(enemyView);
        }

        public void AddAllyView()
        {
            var allyView = Instantiate(allyViewPrefab, transform);

            var location = allySpawnLocations[allies.Count];
            allyView.transform.position = location.position;
            allies.Add(allyView);
        }

    }
}
