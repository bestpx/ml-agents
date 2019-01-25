namespace Yahtzee.Game.Common.GameCells
{
    public class LargeStraightCell : GameCell
    {
        public const int LargeStraightValue = 40;
        public override int EvaluateScore(Hand hand, Gameboard gameboard, bool includeSectionBonus)
        {
            int value = 0;

            if (hand.GetMaxStreak() >= 5)
            {
                value += LargeStraightValue;
            }

            value += YahtzeeBonus(gameboard, hand);
            return value;
        }

        public override int MeanExpectation(Gameboard gameboard)
        {
            return 25;
        }

        public override int MaximumPossible(Gameboard gameboard)
        {
            int maxPossible = 40;
            if (gameboard.ShouldHaveYahtzeeBonus())
            {
                maxPossible = gameboard.YahtzeeBonus; // not += because can't qualify for both at the same time
            }
            return maxPossible;
        }
    }
}