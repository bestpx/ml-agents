namespace Yahtzee.Game.Common.GameCells
{
    public class TwosCell : GameCell 
    {
        public override int EvaluateScore(Hand hand, Gameboard gameboard, bool includeSectionBonus)
        {
            return EvaluateNumberCategoryCell(gameboard, hand, 2, includeSectionBonus);
        }

        public override int MeanExpectation(Gameboard gameboard)
        {
            return 4;
        }

        public override int MaximumPossible(Gameboard gameboard)
        {
            int maxPossible = 10;
            if (gameboard.ShouldHaveYahtzeeBonus())
            {
                maxPossible += gameboard.YahtzeeBonus;
            }
            return maxPossible;
        }
    }
}