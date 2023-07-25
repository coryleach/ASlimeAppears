using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    [System.Serializable]
    public class DeckInstance
    {
        public List<CardInstance> drawPile = new List<CardInstance>();
        public List<CardInstance> currentHand = new List<CardInstance>();
        public List<CardInstance> discardPile = new List<CardInstance>();

        public DeckInstance()
        {
        }

        public DeckInstance(DeckInstance other)
        {
            foreach (var card in other.drawPile)
            {
                drawPile.Add(card.Copy());
            }

            foreach (var card in other.currentHand)
            {
                currentHand.Add(card.Copy());
            }

            foreach (var card in other.discardPile)
            {
                discardPile.Add(card);
            }
        }

        public CardInstance DrawNext()
        {
            if (drawPile.Count < 1)
            {
                throw new Exception("Cannot draw when deck is empty");
            }

            var card = drawPile[^1];
            currentHand.Add(card);
            drawPile.RemoveAt(drawPile.Count-1);

            return card;
        }

        public void Discard(CardInstance card)
        {
            var cardIndex = currentHand.IndexOf(card);
            if (cardIndex == -1)
            {
                throw new Exception("Card is not in the current hand.");
            }
            currentHand.RemoveAt(cardIndex);
            discardPile.Add(card);
        }

        public void ShuffleAllIntoDeck()
        {
            drawPile.AddRange(currentHand);
            drawPile.AddRange(discardPile);

            currentHand.Clear();
            discardPile.Clear();

            ShuffleDeck();
        }

        public void ShuffleDiscardIntoDeck()
        {
            drawPile.AddRange(discardPile);
            discardPile.Clear();
            ShuffleDeck();
        }

        public void ShuffleDeck()
        {
            for (var i = 0; i < drawPile.Count; i++)
            {
                var swapIndex = UnityEngine.Random.Range(i, drawPile.Count);
                if (swapIndex == i)
                {
                    continue;
                }
                //Swap Values
                (drawPile[swapIndex], drawPile[i]) = (drawPile[i], drawPile[swapIndex]);
            }
        }

        public DeckInstance Copy()
        {
            return new DeckInstance(this);
        }
    }
}
