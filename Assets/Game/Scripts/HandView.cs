using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    /// <summary>
    /// Displays views representing the cards in the player's current hand
    /// </summary>
    public class HandView : MonoBehaviour
    {
        public Camera uiCamera;

        public CardView cardViewPrefab;

        public BezierLineView lineView;

        [FormerlySerializedAs("draggingY")] public float recedeOnDragAmountY = 0f;
        public float maxDegreeDelta = 5f;
        public float angleSpacing = 5f;
        public float hoverSpacingX = 50;
        public float minSpacingX = 100f;
        public float maxSpacingX = 170f;
        public float spacingY = 5f;
        public float hoverY = 100f;
        public float smoothTime = 1f;

        public Transform centerTransform;
        public Transform drawPileTransform;
        public Transform discardPileTransform;
        public Transform sellPanelTransform;

        private List<CardView> cardList = new List<CardView>();

        private void OnEnable()
        {
            GetCardViews();
        }

        private void Update()
        {
            var hoveredIndex = GetHoveredIndex();
            var draggingIndex = GetDraggingIndex();

            //Only shift if we're hovering but not yet dragging
            var hoverShiftAmount = (hoveredIndex != -1 && draggingIndex == -1) ? hoverSpacingX : 0;

            var spacingX = GetSpacingX();
            var centerIndex = (cardList.Count - 1) * 0.5f;
            var startX = (cardList.Count - 1) * 0.5f * -spacingX;

            var startY = 0f;
            var hoverCard = hoveredIndex == -1 ? null : cardList[hoveredIndex];
            var draggingCard = draggingIndex == -1 ? null : cardList[draggingIndex];
            ;

            if (draggingIndex != -1)
            {
                // var draggedDistance = cardList[hoveredIndex].DragAmount;
                // var lerpAmount = Mathf.InverseLerp(0, Mathf.Abs(recedeOnDragAmountY * 0.5f), Mathf.Max(Mathf.Abs(draggedDistance.x * 0.25f), draggedDistance.y));
                // startY = Mathf.Lerp(0, recedeOnDragAmountY, lerpAmount);
                // hoverAvoidance = Mathf.Lerp(hoverAvoidance, 0, lerpAmount);
                startY = recedeOnDragAmountY;
            }

            for (var i = 0; i < cardList.Count; i++)
            {
                var card = cardList[i];
                if (card.IsDragging)
                {
                    card.Position = Vector3.SmoothDamp(card.Position, new Vector3(0, hoverY, 0), ref card.velocity,
                        smoothTime);
                    continue;
                }

                var distanceFromCenter = centerIndex - i;
                var isHovered = card.IsHovered && draggingIndex == -1;

                var pt = card.Position;
                pt.x = startX + (spacingX * i) + (hoveredIndex > i ? -1 : 1) * (isHovered ? 0 : hoverShiftAmount);
                pt.y = startY + (isHovered ? hoverY : Mathf.Abs(distanceFromCenter) * -spacingY);

                var rot = isHovered ? Quaternion.identity : Quaternion.Euler(0, 0, angleSpacing * distanceFromCenter);
                card.transform.localRotation = Quaternion.RotateTowards(card.transform.localRotation, rot,
                    maxDegreeDelta * Time.deltaTime);

                card.Position = Vector3.SmoothDamp(card.Position, pt, ref card.velocity, smoothTime);
            }


            if (draggingCard != null)
            {
                lineView.gameObject.SetActive(true);
                var screenPt = Input.mousePosition;
                var worldPt = uiCamera.ScreenToWorldPoint(screenPt);
                var localPt = lineView.transform.InverseTransformPoint(worldPt);
                //Debug.Log($"Screen: {screenPt} World:{worldPt} Local:{localPt}");
                var pt = localPt;
                pt.z = 0;
                lineView.endPoint = pt;
            }
            else
            {
                lineView.gameObject.SetActive(false);
                SetHoverCard(hoverCard);
            }
        }

        private CardView currentHoverCard = null;

        private void SetHoverCard(CardView hoverCard)
        {
            if (currentHoverCard == hoverCard)
            {
                return;
            }

            currentHoverCard = hoverCard;
            int siblingIndex = 0;
            for (var i = 0; i < cardList.Count; i++)
            {
                var card = cardList[i];
                if (card.IsHovered || card.IsDragging)
                {
                    continue;
                }

                card.transform.SetSiblingIndex(siblingIndex);
                siblingIndex++;
            }

            if (currentHoverCard != null && !currentHoverCard.IsDragging)
            {
                currentHoverCard.transform.SetSiblingIndex(siblingIndex);
            }
        }

        private void RefreshCardLayers()
        {
            int siblingIndex = 0;
            for (var i = 0; i < cardList.Count; i++)
            {
                var card = cardList[i];
                card.transform.SetSiblingIndex(siblingIndex);
                siblingIndex++;
            }
        }

        private void GetCardViews()
        {
            cardList.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var card = child.GetComponent<CardView>();
                if (card != null)
                {
                    cardList.Add(card);
                }
            }
        }

        private float GetSpacingX()
        {
            return Mathf.Lerp(minSpacingX, maxSpacingX, 1 - Mathf.InverseLerp(1, 10, cardList.Count));
        }

        private CardView CreateCardView(CardInstance cardInstance)
        {
            var cardView = Instantiate(cardViewPrefab, transform);
            cardView.SetCardInstance(cardInstance);
            return cardView;
        }

        public void AddDrawnCard(CardInstance cardInstance)
        {
            var cardView = CreateCardView(cardInstance);
            cardList.Add(cardView);
            cardView.transform.position = drawPileTransform.position;
        }

        public void RemoveDiscardedCard(CardInstance cardInstance)
        {
            var cardView = cardList.FirstOrDefault(x => x.CardInstance == cardInstance);

            if (cardView == null)
            {
                return;
            }

            cardList.Remove(cardView);

            AnimateAndDestroyDiscardedCard(cardView);
        }

        public void RemoveSoldCard(CardInstance cardInstance)
        {
            var cardView = cardList.FirstOrDefault(x => x.CardInstance == cardInstance);

            if (cardView == null)
            {
                return;
            }

            cardList.Remove(cardView);

            AnimateAndDestroySoldCard(cardView);
        }

        private void AnimateAndDestroyDiscardedCard(CardView cardView)
        {
            AnimateAndDestroyCard(cardView, discardPileTransform);
        }

        private void AnimateAndDestroySoldCard(CardView cardView)
        {
            AnimateAndDestroyCard(cardView, sellPanelTransform);
        }

        public void AnimateAddedCard(CardInstance cardInstance)
        {
            var cardView = CreateCardView(cardInstance);
            cardView.CanDrag = false;

            var transform1 = cardView.transform;
            transform1.position = centerTransform.position;
            transform1.localScale = Vector3.zero;

            var sequence = DOTween.Sequence();
            sequence.Append(cardView.transform.DOScale(Vector3.one, 0.25f));
            sequence.AppendInterval(1f);
            sequence.AppendCallback(() => AnimateAndDestroyCard(cardView, discardPileTransform));
        }

        private void AnimateAndDestroyCard(CardView cardView, Transform targetTransform)
        {
            var cardTransform = (RectTransform) cardView.transform;
            cardTransform.SetParent(targetTransform, true);
            cardView.CanDrag = false;
            var sequence = DOTween.Sequence();
            sequence.Append(cardView.transform.DOMove(targetTransform.position, 0.225f));
            sequence.Append(cardView.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() => { Destroy(cardView.gameObject); }));
        }

        public void Sort()
        {
            cardList.Sort((a, b) => string.Compare(a.CardId, b.CardId, StringComparison.Ordinal));
            RefreshCardLayers();
        }

        private int GetHoveredIndex()
        {
            for (var i = 0; i < cardList.Count; i++)
            {
                if (cardList[i].IsHovered)
                {
                    return i;
                }
            }

            return -1;
        }

        private int GetDraggingIndex()
        {
            for (var i = 0; i < cardList.Count; i++)
            {
                if (cardList[i].IsDragging)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
