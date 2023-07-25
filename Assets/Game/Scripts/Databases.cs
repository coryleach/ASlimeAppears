using UnityEngine;

namespace Game.Scripts
{
    /// <summary>
    /// Provides references to the databases containing game static data.
    /// </summary>
    [ExecuteAlways]
    public class Databases : MonoBehaviour
    {
        [SerializeField] private CardDatabase cardDatabase;
        [SerializeField] private EnemyDatabase enemyDatabase;
        [SerializeField] private AllyDatabase allyDatabase;

        private static Databases _instance = null;

        public static CardDatabase Cards => _instance?.cardDatabase;

        public static EnemyDatabase Enemies => _instance?.enemyDatabase;

        public static AllyDatabase Allies => _instance?.allyDatabase;

        private void Awake()
        {
            _instance = this;
        }
    }
}
