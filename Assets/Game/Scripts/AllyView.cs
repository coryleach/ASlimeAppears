using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class AllyView : TargetView
    {
        [SerializeField] private Slider xpSlider;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Image shield;
        [SerializeField] private TMP_Text shieldLabel;
        [SerializeField] private TMP_Text levelLabel;

        [SerializeField] private Vector3 punchScale = new Vector3(0.1f, 0.1f, 0.1f);

        public bool IsOccupied { get; private set; }  = false;

        public int Health => currentAlly?.health ?? 0;
        public int Shield => currentAlly?.health ?? 0;

        public int TotalHealthAndShield => Health + Shield;

        private AllyInstance currentAlly;
        public AllyInstance AllyInstance => currentAlly;

        private GameObject currentModel;

        private int xp = 0;
        private int maxXp = 3;

        private int damageShield = 0;

        private void Awake()
        {
            Refresh();
        }

        public override bool CanAcceptDraggable(Draggable draggable)
        {
            var cardView = draggable as CardView;
            if (cardView == null)
            {
                return false;
            }
            return cardView.CardInstance.CardInfo.IsAnyType(CardType.Exp, CardType.Summon, CardType.Unsummon, CardType.Defense);
        }

        public void AnimateAttack(Transform target)
        {
            modelPivot.DOPunchPosition(target.position - modelPivot.transform.position, 0.25f);
        }

        public override void AcceptDraggable(Draggable draggable)
        {
            var cardView = draggable as CardView;
            if (cardView == null)
            {
                return;
            }

            var card = cardView.CardInstance;

            if (!IsOccupied && cardView.CardInstance.CardInfo.IsType(CardType.Summon))
            {
                IsOccupied = true;
                var characterId = cardView.CardInstance.CardInfo.characterId;
                currentAlly = new AllyInstance(characterId);
                currentModel = Instantiate(currentAlly.Info.modelPrefabs[0], modelPivot);
            }
            else if (cardView.CardInstance.CardInfo.IsType(CardType.Exp))
            {
                xp = Mathf.Clamp(xp += cardView.CardInstance.CardInfo.xpAmount, 0, maxXp);
                FloatyTextManager.Instance.SpawnText("+1", Color.green, modelPivot.position);
            }
            else if (cardView.CardInstance.CardInfo.IsType(CardType.Unsummon))
            {
                if (IsOccupied)
                {
                    if (currentAlly.level < currentAlly.Info.MaxLevel)
                    {
                        FloatyTextManager.Instance.SpawnText("Not Max Level!", Color.red, modelPivot.position);
                        return;
                    }
                    else
                    {
                        CombatManager.Instance.OnAllyUnsummon(currentAlly);
                        Clear();
                    }
                }
                else
                {
                    FloatyTextManager.Instance.SpawnText("Nothing Here!", Color.red, modelPivot.position);
                    return;
                }
            }
            else if (cardView.CardInstance.CardInfo.IsType(CardType.Defense))
            {
                if (!IsOccupied)
                {
                    FloatyTextManager.Instance.SpawnText($"Summon Ally!", Color.red, modelPivot.position);
                    return;
                }

                var amount = cardView.CardInstance.CardInfo.shieldAmount;
                damageShield += amount;
                FloatyTextManager.Instance.SpawnText($"+{amount} Shield", Color.white, modelPivot.position);
            }
            else
            {

            }

            Refresh();

            CombatManager.Instance.Discard(card);
        }

        public void Damage(int amount)
        {
            if (damageShield >= amount)
            {
                damageShield -= amount;
                FloatyTextManager.Instance.SpawnText($"Blocked {amount}", Color.white, modelPivot.position);
            }
            else
            {
                amount -= damageShield;
                currentAlly.health -= amount;
                FloatyTextManager.Instance.SpawnText($"{amount}", Color.red, modelPivot.position);
            }

            modelPivot.DOPunchScale(punchScale, 0.2f);

            if (currentAlly.health <= 0)
            {
                CombatManager.Instance.OnAllyKilled(currentAlly);
                Clear();
            }

            Refresh();
        }

        public void Refresh()
        {
            healthSlider.gameObject.SetActive(IsOccupied);
            if (currentAlly != null)
            {
                healthSlider.value = currentAlly.health / (float) currentAlly.Info.maxHealth;
            }
            xpSlider.value = xp / (float)maxXp;
            shield.gameObject.SetActive(damageShield > 0);
            shieldLabel.text = damageShield.ToString();
            levelLabel.text = currentAlly != null ? $"Level {currentAlly.level}" : string.Empty;
        }

        public void Clear()
        {
            Destroy(currentModel);
            currentAlly = null;
            IsOccupied = false;
            damageShield = 0;
        }

        public bool TryLevelUp()
        {
            if (xp <= 0)
            {
                return false;
            }

            xp -= 1;

            if (IsOccupied && currentAlly.level >= currentAlly.Info.MaxLevel)
            {
                FloatyTextManager.Instance.SpawnText("Max Level!", Color.yellow, modelPivot.position);
            }
            else if (IsOccupied)
            {
                Destroy(currentModel);
                currentAlly.level += 1;
                currentModel = Instantiate(currentAlly.Info.modelPrefabs[currentAlly.level-1], modelPivot);
                FloatyTextManager.Instance.SpawnText("Level Up!", Color.white, modelPivot.position);
            }
            else
            {
                FloatyTextManager.Instance.SpawnText("-1", Color.red, modelPivot.position);
            }

            Refresh();
            return true;
        }
    }
}
