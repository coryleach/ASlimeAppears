using UnityEngine;

namespace Game.Scripts
{
    /// <summary>
    /// Static data that defines an enemy that we can create instances of
    /// </summary>
    [CreateAssetMenu]
    public class EnemyInfo : ScriptableObject
    {
        public string displayName;
        public int maxHealth;
        public GameObject displayPrefab;
    }
}
