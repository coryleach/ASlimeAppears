using System;

namespace Game.Scripts
{
    /// <summary>
    /// Data representing an instance of an enemy.
    /// </summary>
    [Serializable]
    public class EnemyInstance
    {
        public string enemyId;
        public EnemyInfo EnemyInfo => Databases.Enemies?.GetEnemy(enemyId);
        public int health;

        public EnemyInstance()
        {
        }

        public EnemyInstance(string id)
        {
            enemyId = id;
            health = EnemyInfo.maxHealth;
        }

        public EnemyInstance(EnemyInfo info)
        {
            enemyId = info.name;
            health = info.maxHealth;
        }
    }
}
