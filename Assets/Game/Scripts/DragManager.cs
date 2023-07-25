using Unity.VisualScripting;
using UnityEngine;

namespace Game.Scripts
{
    public class DragManager : MonoBehaviour
    {
        private static DragManager instance = null;
        public static DragManager Instance => instance;

        public bool IsDragging
        {
            get;
            private set;
        } = false;

        private Draggable currentDraggable = null;
        public Draggable Current => currentDraggable;

        public DraggableEvent OnDragBegin { get; } = new DraggableEvent();
        public DraggableEvent OnDragEnd { get; } = new DraggableEvent();

        private void Awake()
        {
            instance = this;
        }

        /// <summary>
        /// Register a draggable object with the manager
        /// </summary>
        /// <param name="draggable">a draggable object</param>
        public void Register(Draggable draggable)
        {
            draggable.OnDragBegin.AddListener(DragBegin);
            draggable.OnDragEnd.AddListener(DragEnd);
        }

        /// <summary>
        /// Deregister a Draggable object
        /// </summary>
        /// <param name="draggable"></param>
        public void Deregister(Draggable draggable)
        {
            if (draggable == currentDraggable)
            {
                currentDraggable = null;
                IsDragging = false;
            }
            draggable.OnDragBegin.RemoveListener(DragBegin);
            draggable.OnDragEnd.RemoveListener(DragEnd);
        }


        private void DragBegin(Draggable target)
        {
            currentDraggable = target;
            IsDragging = true;
            OnDragBegin.Invoke(currentDraggable);
        }

        private void DragEnd(Draggable target)
        {
            if (target == currentDraggable)
            {
                currentDraggable = null;
                IsDragging = false;
            }
            OnDragEnd.Invoke(target);
        }
    }
}
