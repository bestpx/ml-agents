namespace Yahtzee.Game.Common.GameCells
{
    public class SmallStraighCell : GameCell
    {
        public const int SmallStraightValue = 30;
        
        public override int EvaluateScore(Hand hand, Gameboard gameboard, bool includeSectionBonus)
        {
            int value = 0;

            if (hand.GetMaxStreak() >= 4)
            {
                value += SmallStraightValue;
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
            int maxPossible = 30;
            if (gameboard.ShouldHaveYahtzeeBonus())
            {
                maxPossible = gameboard.YahtzeeBonus; // can't qualify for both
            }
            return maxPossible;
        }
    }
}