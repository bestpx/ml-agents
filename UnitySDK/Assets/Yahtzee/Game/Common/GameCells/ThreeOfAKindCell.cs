namespace Yahtzee.Game.Common.GameCells
{
    public class ThreeOfAKindCell : GameCell
    {
        public override int EvaluateScore(Hand hand, Gameboard gameboard, bool includeSectionBonus)
        {
            int value = 0;

            if (hand.HasThreeOfAKind())
            {
                value += hand.GetSum();
            }

            value += YahtzeeBonus(gameboard, hand);
            return value;
        }

        public override int MeanExpectation(Gameboard gameboard)
        {
            return 12;
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