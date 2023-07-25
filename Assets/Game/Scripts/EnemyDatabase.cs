using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu]
    public class EnemyDatabase : ScriptableObject
    {
        public List<EnemyInfo> enemies = new List<EnemyInfo>();

        public EnemyInfo GetEnemy(string enemyId)
        {
            return enemies.FirstOrDefault(x => x.name == enemyId);
        }
    }
}