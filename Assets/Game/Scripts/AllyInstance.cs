using System;

namespace Game.Scripts
{
    [Serializable]
    public class AllyInstance
    {
        public string characterId;

        public AllyInstance(string characterId)
        {
            this.characterId = characterId;
            this.health = this.Info.maxHealth;
        }

        public AllyInstance()
        {
        }

        public AllyInfo Info => Databases.Allies.Get(characterId);

        public int level = 1;
        public int health;
    }
}
