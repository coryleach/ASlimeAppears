using UnityEngine;

namespace Game.Scripts
{
    [System.Serializable]
    public class CardInstance
    {
        //Card id is the name of the CardInfo scriptable object
        public string cardId;

        [SerializeField]
        private CardInfo cardInfo = null;
        public CardInfo CardInfo
        {
            get
            {
                if (cardInfo == null)
                {
                    cardInfo = Databases.Cards.GetCard(cardId);
                }
                return cardInfo;
            }
        }

        public CardInstance() { }

        public CardInstance(CardInfo cardInfo)
        {
            this.cardId = cardInfo.name;
            this.cardInfo = cardInfo;
        }

        public CardInstance(string cardId)
        {
            this.cardId = cardId;
            cardInfo = Databases.Cards.GetCard(cardId);
        }

        public CardInstance Copy()
        {
            var copy = new CardInstance()
            {
                cardId = cardId,
                cardInfo = cardInfo,
            };
            return copy;
        }
    }
}
