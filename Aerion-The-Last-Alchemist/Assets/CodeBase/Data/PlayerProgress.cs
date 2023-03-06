using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public GameData gameData;


        public PlayerProgress(string initialLevel)
        {
            gameData = new GameData(initialLevel);
        }
    }
}