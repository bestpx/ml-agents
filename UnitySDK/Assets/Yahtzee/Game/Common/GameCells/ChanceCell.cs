namespace Yahtzee.Game.Common.GameCells
{
    public class ChanceCell : GameCell
    {
        public override int EvaluateScore(Hand hand, Gameboard gameboard)
        {
            int value = 0;
            value += hand.GetSum();
            value += YahtzeeBonus(gameboard, hand);
            return value;
        }

        public override int MeanExpectation(Gameboard gameboard)
        {
            return 15;
        }

        public override int MaximumPossible(Gameboard gameboard)
        {
            int maxPossible = 30;
            if (gameboard.ShouldHaveYahtzeeBonus())
            {
                maxPossible += gameboard.YahtzeeBonus;
            }
            return maxPossible;
        }
    }
}