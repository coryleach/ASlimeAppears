using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu]
    public class AllyInfo : ScriptableObject
    {
        public string CharacterId => name;

        public string displayName;
        public List<GameObject> modelPrefabs = new List<GameObject>();
        public int MaxLevel => modelPrefabs.Count;
        public int maxHealth = 3;
    }
}
