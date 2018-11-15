using Yahtzee.Game.Common;

namespace Yahtzee
{
    public class GameService
    {
        public Game.Common.Game CreateNewGame()
        {
            return Game.Common.Game.GameFactory.MakeClassicGame();
        }
    }
}