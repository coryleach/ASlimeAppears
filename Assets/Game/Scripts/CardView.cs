using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts
{
    /// <summary>
    /// Displays an instance of a card.
    /// </summary>
    public class CardView : Draggable, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        [SerializeField] private TMP_Text cardNameLabel;
        [SerializeField] private TMP_Text cardDescriptionLabel;
        [SerializeField] private Image background;
        [SerializeField] private Image art;

        private bool isHovered = false;
        public bool IsHovered => isHovered;

        [NonSerialized] public Vector3 velocity = Vector3.zero;

        [NonSerialized] public Vector3 rotationalVelocity = Vector3.zero;

        private List<RaycastResult> raycastList = new List<RaycastResult>();

        private CardInstance cardInstance = null;
        public CardInstance CardInstance => cardInstance;

        public string CardId => cardInstance.CardInfo.name;

        public void SetCardInstance(CardInstance instance)
        {
            cardInstance = instance;
            var cardInfo = cardInstance.CardInfo;
            cardNameLabel.text = cardInfo.displayName;
            cardDescriptionLabel.text = cardInfo.text;
            background.color = Databases.Cards.GetColor(cardInfo.cardType);
            art.sprite = cardInfo.sprite;
        }

        public Vector2 Position
        {
            get => ((RectTransform) transform).anchoredPosition;
            set => ((RectTransform) transform).anchoredPosition = value;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovered = false;
        }

        public void OnDrop(PointerEventData eventData)
        {
            raycastList.Clear();

            var pointerEvent = new PointerEventData(EventSystem.current)
            {
                position = eventData.position
            };

            EventSystem.current.RaycastAll(pointerEvent, raycastList);

            foreach (var result in raycastList)
            {
                var dropTarget = result.gameObject.GetComponent<DropTarget>();
                if (dropTarget != null && dropTarget.CanAcceptDraggable(this))
                {
                    dropTarget.AcceptDraggable(this);
                    return;
                }
            }
        }
    }
}
