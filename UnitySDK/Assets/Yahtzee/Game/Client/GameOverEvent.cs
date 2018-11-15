namespace Yahtzee.Game.Client
{
    public class GameOverEvent
    {
        public Common.Game Game { get; }

        public GameOverEvent(Common.Game game)
        {
            Game = game;
        }
    }
}