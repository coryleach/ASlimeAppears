using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField]
        private bool useDragParent = true;

        [SerializeField]
        private bool moveWhenDragged = true;

        [SerializeField]
        private bool dragable = true;
        public bool CanDrag
        {
            get => dragable;
            set => dragable = value;
        }

        private bool dragging = false;
        public bool IsDragging => dragging;

        private Vector2 dragAmount;
        public Vector2 DragAmount => dragAmount;

        private Vector2 dragPosition;
        private Vector2 startPosition;
        private int siblingIndex = 0;
        private Transform previousParent;

        public DraggableEvent OnDragBegin { get; } = new DraggableEvent();
        public DraggableEvent OnDragEnd { get; } = new DraggableEvent();

        public virtual void OnEnable()
        {
            DragManager.Instance.Register(this);
        }

        public virtual void OnDisable()
        {
            DragManager.Instance.Deregister(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!dragable)
            {
                return;
            }

            dragging = true;

            if (useDragParent)
            {
                previousParent = transform.parent;
                siblingIndex = transform.GetSiblingIndex();
                transform.SetParent(DragParent.Instance.transform, true);
            }

            startPosition = ((RectTransform)transform).anchoredPosition;
            dragPosition = startPosition;
            dragAmount = Vector2.zero;
            OnDragBegin.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!dragging)
            {
                return;
            }

            dragging = false;

            if (useDragParent)
            {
                transform.SetParent(previousParent, true);
                transform.SetSiblingIndex(siblingIndex);
            }

            OnDragEnd.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!dragging || !eventData.dragging)
            {
                return;
            }
            dragAmount = (dragAmount + eventData.delta);
            dragPosition = startPosition + dragAmount;
            if (moveWhenDragged)
            {
                ((RectTransform)transform).anchoredPosition = dragPosition;
            }
        }
    }
}
