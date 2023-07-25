using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    [Flags]
    public enum CardType
    {
        Attack   = 1 << 0,
        Exp = 1 << 1,
        Defense  = 1 << 2,
        Summon = 1 << 3,
        Unsummon = 1 << 4,
        Treasure = 1 << 5,
    }

    [CreateAssetMenu]
    public class CardInfo : ScriptableObject
    {
        public string CardId => name;

        [Header("Shared Properties")]
        public string displayName;
        public CardType cardType;
        public Sprite sprite;
        public string text;

        [Header("Attack Properties")]
        public int damage;

        [Header("Defensee Properties")]
        public int shieldAmount;

        [Header("Character Properties")]
        public string characterId;

        [Header("Xp Properties")]
        public int xpAmount;

        [Header("Treasure Properties")]
        public int coins = 0;

        public CardInstance CreateData()
        {
            return new CardInstance(this);
        }

        public bool IsType(CardType type)
        {
            return (this.cardType & type) != 0;
        }

        public bool IsAnyType(params CardType[] types)
        {
            return types.Any(type => (this.cardType & type) != 0);
        }
    }
}
