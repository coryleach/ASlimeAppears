using System;
using System.Collections.Generic;
using System.Linq;
using Gameframe.ScriptableObjects.Variables;
using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu]
    public class CardDatabase : ScriptableObject
    {
        [Header("Colors")]
        public List<CardTypeColor> cardTypeColors = new List<CardTypeColor>();

        [Header("Cards")]
        public List<CardInfo> cards = new List<CardInfo>();

        /// <summary>
        /// Get the CardInfo for a specific cardId
        /// </summary>
        /// <param name="cardId">the id of the card (name property of the CardInfo scriptable object)</param>
        /// <returns>CardInfo or Null if id is not found</returns>
        public CardInfo GetCard(string cardId)
        {
            return cards.FirstOrDefault(x => x.name == cardId);
        }

        public Color GetColor(CardType cardType)
        {
            foreach (var entry in cardTypeColors)
            {
                if (cardType == entry.cardType)
                {
                    return entry.color.Value;
                }
            }
            return Color.white;
        }

        [Serializable]
        public class CardTypeColor
        {
            public CardType cardType;
            public ColorReference color;
        }

    }
}
