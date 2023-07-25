using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts
{
    public class DropTarget : MonoBehaviour, IDropHandler
    {
        [SerializeField] private GameObject validDropIndicator;

        public async virtual void OnEnable()
        {
            await Task.Yield();
            DragManager.Instance.OnDragBegin.AddListener(OnDragBegin);
            DragManager.Instance.OnDragEnd.AddListener(OnDragEnd);
        }

        public virtual void OnDisable()
        {
            DragManager.Instance.OnDragBegin.RemoveListener(OnDragBegin);
            DragManager.Instance.OnDragEnd.RemoveListener(OnDragEnd);
        }

        private void OnDragBegin(Draggable draggable)
        {
            if (validDropIndicator != null)
            {
                validDropIndicator.SetActive(CanAcceptDraggable(draggable));
            }
        }

        private void OnDragEnd(Draggable draggable)
        {
            if (validDropIndicator != null)
            {
                validDropIndicator.SetActive(false);
            }
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
            {
                return;
            }

            var draggable = eventData.pointerDrag.GetComponent<Draggable>();
            if (draggable == null)
            {
                return;
            }

            if (CanAcceptDraggable(draggable))
            {
                AcceptDraggable(draggable);
            }
        }

        public virtual bool CanAcceptDraggable(Draggable draggable)
        {
            return false;
        }

        public bool CanAccept(GameObject gobj)
        {
            var draggable = gobj.GetComponent<Draggable>();
            return draggable != null && this.CanAcceptDraggable(draggable);
        }

        public virtual void AcceptDraggable(Draggable draggable)
        {

        }
    }
}
