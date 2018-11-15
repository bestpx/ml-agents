namespace Yahtzee.Game.Common.GameCells
{
    public class YahtzeeCell : GameCell
    {
        public override int EvaluateScore(Hand hand, Gameboard gameboard)
        {
            return hand.IsYahtzee() ? gameboard.YahtzeeBonus : 0;
        }

        public override int MeanExpectation(Gameboard gameboard)
        {
            return 25;
        }

        public override int MaximumPossible(Gameboard gameboard)
        {
            return 50;
        }
    }
}