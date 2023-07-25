using System.Collections;
using System.Linq;
using Gameframe.ScriptableObjects.Events;
using UnityEngine;

namespace Game.Scripts
{
    /// <summary>
    /// Controls the flow of combat
    /// </summary>
    public class CombatManager : MonoBehaviour
    {
        private static CombatManager _instance;
        public static CombatManager Instance => _instance;

        [SerializeField] private BannerView bannerView;
        [SerializeField] private DayNightManager dayNightManager;

        [SerializeField] private CombatFieldView _combatField;
        [SerializeField] private HandView _handView;
        [SerializeField] private GameDataManager _dataManager;

        [SerializeField] private int handSize = 5;
        [SerializeField] private GameEvent refreshEvent;

        private DeckInstance deck = null;
        private bool busy = false;

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            StartCoroutine(ExecutePhaseStartCombat());
        }

        private IEnumerator DrawUpToHandSize()
        {
            yield return new WaitForSeconds(0.2f);

            while (deck.currentHand.Count < handSize)
            {
                if (deck.drawPile.Count == 0)
                {
                    if (deck.discardPile.Count > 0)
                    {
                        deck.ShuffleDiscardIntoDeck();
                    }
                    else
                    {
                        //Break out of loop. We can't draw any more because we have no cards left to draw.
                        break;
                    }
                }
                var card = deck.DrawNext();
                _handView.AddDrawnCard(card);
                yield return new WaitForSeconds(0.2f);
            }
            _handView.Sort();
        }

        private IEnumerator DiscardHand()
        {
            while (deck.currentHand.Count > 0)
            {
                var card = deck.currentHand[^1];
                deck.Discard(card);
                _handView.RemoveDiscardedCard(card);
                yield return new WaitForSeconds(0.2f);
            }
        }

        private IEnumerator ExecutePhaseStartCombat()
        {
            busy = true;
            deck = _dataManager.CurrentData.currentDeck;

            _combatField.AddAllyView();
            _combatField.AddAllyView();
            _combatField.AddAllyView();

            GameDataManager.Instance.CurrentData.day = 1;
            bannerView.Show($"Day {GameDataManager.Instance.CurrentData.day}");

            deck.ShuffleAllIntoDeck();
            yield return DrawUpToHandSize();
            busy = false;
        }

        private AllyView WeakestAlly()
        {
            AllyView weakest = null;
            var min = int.MaxValue;
            foreach (var allyView in _combatField.Allies)
            {
                if (!allyView.IsOccupied)
                {
                    continue;
                }

                var total = allyView.TotalHealthAndShield;
                if (total >= min)
                {
                    continue;
                }

                weakest = allyView;
                min = total;
            }
            return weakest;
        }

        private AllyView RandomAlly()
        {
            var occupied = _combatField.Allies.Where(x => x.IsOccupied).ToArray();
            if (occupied.Length <= 0)
            {
                return null;
            }
            return occupied[UnityEngine.Random.Range(0,occupied.Length)];
        }

        private IEnumerator ExecuteEnemyAttacks()
        {
            foreach (var enemy in _combatField.Enemies)
            {
                var target = RandomAlly();
                if (target != null)
                {
                    enemy.AnimateAttack(target.transform);
                    target.Damage(1);
                }
                yield return new WaitForSeconds(0.3f);
            }
        }

        private IEnumerator ExecuteAllyAttacks()
        {
            foreach (var ally in _combatField.Allies)
            {
                if (!ally.IsOccupied)
                {
                    continue;
                }

                var target = RandomEnemy();
                if (target != null)
                {
                    ally.AnimateAttack(target.transform);
                    target.Damage(1);
                    target.Refresh();
                }
                yield return new WaitForSeconds(0.3f);
            }

            for (int i = _combatField.Enemies.Count - 1; i >= 0; i--)
            {
                var enemy = _combatField.Enemies[i];
                if (enemy.EnemyInstance.health <= 0)
                {
                    OnEnemyKilled(enemy.EnemyInstance);
                }
            }
        }

        private EnemyView RandomEnemy()
        {
            var enemyList = _combatField.Enemies;
            if (enemyList.Count <= 0)
            {
                return null;
            }
            return enemyList[UnityEngine.Random.Range(0,enemyList.Count)];
        }

        private void SpawnEnemy()
        {
            var enemy = new EnemyInstance("BasicSlime");
            _combatField.AddEnemy(enemy);
            bannerView.Show($"A {enemy.EnemyInfo.displayName} Appears!");
        }

        private IEnumerator ExecutePhaseEndOfTurn()
        {
            busy = true;

            yield return DiscardHand();

            yield return ExecuteEnemyAttacks();

            yield return ExecuteAllyAttacks();

            yield return dayNightManager.ChangeToNight();

            for (int i = 0; i < _combatField.Allies.Count; i++)
            {
                if (_combatField.Allies[i].TryLevelUp())
                {
                    yield return new WaitForSeconds(1f);
                }
            }

            if (_combatField.Enemies.Count < _combatField.MaxEnemies && _combatField.AllyCount > 0)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(1f);
            }

            yield return dayNightManager.ChangeToDay();

            GameDataManager.Instance.CurrentData.day += 1;
            bannerView.Show($"Day {GameDataManager.Instance.CurrentData.day}");
            refreshEvent.Raise();

            yield return DrawUpToHandSize();

            busy = false;
        }

        private IEnumerator ExecuteDiscard(CardInstance cardInstance)
        {
            busy = true;
            deck.Discard(cardInstance);
            _handView.RemoveDiscardedCard(cardInstance);
            yield return new WaitForSeconds(0.2f);
            busy = false;

            if (deck.currentHand.Count <= 0)
            {
                EndTurn();
            }

            refreshEvent.Raise();
        }

        private IEnumerator ExecuteSell(CardInstance cardInstance)
        {
            busy = true;

            deck.currentHand.Remove(cardInstance);
            _handView.RemoveSoldCard(cardInstance);

            var coins = cardInstance.CardInfo.coins;

            yield return new WaitForSeconds(0.2f);

            FloatyTextManager.Instance.SpawnText($"+{coins} Coins", Color.yellow, _handView.sellPanelTransform.position);
            GameDataManager.Instance.CurrentData.coins += coins;
            refreshEvent.Raise();

            busy = false;

            if (deck.currentHand.Count <= 0)
            {
                EndTurn();
            }
        }

        public void OnEnemyKilled(EnemyInstance enemy)
        {
            var view = _combatField.Enemies.First(x => x.EnemyInstance == enemy);
            GameDataManager.Instance.CurrentData.coins += 1;
            FloatyTextManager.Instance.SpawnText("+1 Coin", Color.yellow, view.transform.position);
            _combatField.RemoveEnemy(enemy);
            refreshEvent.Raise();
        }

        public void OnAllyKilled(AllyInstance ally)
        {
        }

        public void OnAllyUnsummon(AllyInstance ally)
        {
            var card = new CardInstance("Card_Knight");
            deck.discardPile.Add(card);
            _handView.AnimateAddedCard(card);
            refreshEvent.Raise();
        }

        public void EndTurn()
        {
            if (busy)
            {
                return;
            }
            StartCoroutine(ExecutePhaseEndOfTurn());
        }

        public void Sell(CardInstance cardInstance)
        {
            if (busy)
            {
                return;
            }
            StartCoroutine(ExecuteSell(cardInstance));
        }

        public void Discard(CardInstance cardInstance)
        {
            if (busy)
            {
                return;
            }
            StartCoroutine(ExecuteDiscard(cardInstance));
        }

    }
}
