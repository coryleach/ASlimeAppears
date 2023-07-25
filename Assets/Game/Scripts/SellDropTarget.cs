using UnityEngine;

namespace Game.Scripts
{
    public class SellDropTarget : DropTarget
    {
        public override bool CanAcceptDraggable(Draggable draggable)
        {
            var card = draggable as CardView;
            if (card == null)
            {
                return false;
            }
            return card.CardInstance.CardInfo.IsType(CardType.Treasure);
        }

        public override void AcceptDraggable(Draggable draggable)
        {
            var cardView = (CardView) draggable;
            CombatManager.Instance.Sell(cardView.CardInstance);
        }
    }
}
