
namespace CardGame.Model
{
    public class CardGameModel
    {
        public static int GameModelIdCounter;
        public int MobSpawnOrder { get; set; }

        public float GameTime { get; private set; }
        public float MobAttackDuration { get; private set; }

        public void SetModelFromLevelData(CardGameLevelDataSo levelData)
        {
            if (levelData == null) return;

            MobSpawnOrder = 0;

        }

    }
}