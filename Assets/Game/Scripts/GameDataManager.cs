using System;
using UnityEngine;

namespace Game.Scripts
{
    public class GameDataManager : MonoBehaviour
    {
        private static GameDataManager _instance = null;
        public static GameDataManager Instance => _instance;

        [SerializeField]
        private GameData currentGameData = new GameData();
        public GameData CurrentData => currentGameData;

        [SerializeField]
        private DeckInstance starterDeck = new DeckInstance();

        private void Awake()
        {
            _instance = this;
            NewGameData();
        }

        public void NewGameData()
        {
            currentGameData = new GameData()
            {
                currentDeck = new DeckInstance(starterDeck),
            };
        }

        public bool HasSavedGameData()
        {
            return PlayerPrefs.GetString("SavedGame", null) != null;
        }

        public void LoadGameData()
        {
            var jsonData = PlayerPrefs.GetString("SavedGame", null);
            if (jsonData == null)
            {
                return;
            }
            currentGameData = JsonUtility.FromJson<GameData>(jsonData);
        }

        public void SaveGameData()
        {
            var jsonData = JsonUtility.ToJson(currentGameData);
            PlayerPrefs.SetString("SavedGame", jsonData);
        }
    }
}
