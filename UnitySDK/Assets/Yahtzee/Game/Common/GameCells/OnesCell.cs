namespace Yahtzee.Game.Common.GameCells
{
    public class OnesCell : GameCell
    {
        public override int EvaluateScore(Hand hand, Gameboard gameboard, bool includeSectionBonus = false)
        {
            return EvaluateNumberCategoryCell(gameboard, hand, 1, includeSectionBonus);
        }

        public override int MeanExpectation(Gameboard gameboard)
        {
            return 3;
        }

        public override int MaximumPossible(Gameboard gameboard)
        {
            int maxPossible = 5;
            if (gameboard.ShouldHaveYahtzeeBonus())
            {
                maxPossible += gameboard.YahtzeeBonus;
            }
            return maxPossible;
        }
    }
}