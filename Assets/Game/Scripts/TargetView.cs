using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts
{
    public class TargetView : DropTarget, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected Transform modelPivot;

        public override bool CanAcceptDraggable(Draggable draggable)
        {
            var cardView = draggable as CardView;
            if (cardView == null)
            {
                return false;
            }
            return cardView.CardInstance.CardInfo.IsType(CardType.Exp);
        }

        public override void OnDrop(PointerEventData eventData)
        {
            base.OnDrop(eventData);
            TargetManager.Instance.SetTarget((TargetView)null);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.dragging && CanAccept(eventData.pointerDrag))
            {
                TargetManager.Instance.SetTarget(this);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.dragging)
            {
                TargetManager.Instance.SetTarget((TargetView)null);
            }
        }
    }
}
