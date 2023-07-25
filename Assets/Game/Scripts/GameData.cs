namespace Game.Scripts
{
    [System.Serializable]
    public class GameData
    {
        public DeckInstance currentDeck = new DeckInstance();
        public int day = 1;
        public int coins = 0;
    }
}
