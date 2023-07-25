using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu]
    public class AllyDatabase : ScriptableObject
    {
        public List<AllyInfo> allies = new List<AllyInfo>();

        public AllyInfo Get(string characterId)
        {
            Debug.Log($"Get {characterId}");
            return allies.FirstOrDefault(x => x.CharacterId == characterId);
        }
    }
}
