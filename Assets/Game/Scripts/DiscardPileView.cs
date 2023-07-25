using DG.Tweening;
using UnityEngine;

namespace Game.Scripts
{
    /// <summary>
    /// Cards dragged to this view will be discarded
    /// </summary>
    public class DiscardPileView : DropTarget
    {
        public override bool CanAcceptDraggable(Draggable draggable)
        {
            return draggable is CardView;
        }

        public override void AcceptDraggable(Draggable draggable)
        {
            var cardView = (CardView) draggable;
            CombatManager.Instance.Discard(cardView.CardInstance);
        }
    }
}
