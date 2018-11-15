namespace Yahtzee.Game.Common.GameCells
{
    public class SixesCell : GameCell
    {
        public override int EvaluateScore(Hand hand, Gameboard gameboard)
        {
            return EvaluateNumberCategoryCell(gameboard, hand, 6);
        }

        public override int MeanExpectation(Gameboard gameboard)
        {
            return 18;
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