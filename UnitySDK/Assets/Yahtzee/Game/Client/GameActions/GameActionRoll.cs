using CommonUtil;

namespace Yahtzee.Game.Client.GameActions
{
    public class GameActionRoll : GameAction
    {
        public override bool IsValid(Common.Game game)
        {
            return game.CanRoll();
        }

        public override void Perform(Common.Game game)
        {
            game.Roll();
            Logger.Log(LogLevel.Debug, "GameActionRoll, current hand: " + game.Hand.ToString());
        }

        public override void Revert(Common.Game game)
        {
            throw new System.NotImplementedException();
        }
    }
}