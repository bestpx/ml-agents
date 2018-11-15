namespace Yahtzee.Game.Common.GameCells
{
    public class FoursCell : GameCell
    {
        public override int EvaluateScore(Hand hand, Gameboard gameboard)
        {
            return EvaluateNumberCategoryCell(gameboard, hand, 4);
        }

        public override int MeanExpectation(Gameboard gameboard)
        {
            return 12;
        }

        public override int MaximumPossible(Gameboard gameboard)
        {
            int maxPossible = 20;
            if (gameboard.ShouldHaveYahtzeeBonus())
            {
                maxPossible += gameboard.YahtzeeBonus;
            }
            return maxPossible;
        }
    }
}