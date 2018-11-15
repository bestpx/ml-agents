using System;

namespace Yahtzee.Game.Common.GameCells
{
    public abstract class GameCell
    {
        public int CellId { get; set; }
        protected GameCell() {}

        /// <summary>
        /// score in the gamecell, -1 for not played
        /// </summary>
        private int _score = -1;

        private bool _hasYahtzeeBonus;
        
        /// <summary>
        /// Evaluate score by hand and gameboard status
        /// </summary>
        /// <returns></returns>
        public abstract int EvaluateScore(Hand hand, Gameboard gameboard);

        public abstract int MeanExpectation(Gameboard gameboard);
        public abstract int MaximumPossible(Gameboard gameboard);

        public void PlayHand(Hand hand, Gameboard gameboard)
        {
            _score = EvaluateScore(hand, gameboard);
            _hasYahtzeeBonus = gameboard.ShouldHaveYahtzeeBonus() && hand.IsYahtzee();
        }

        public bool HasPlayed()
        {
            return _score != -1;
        }

        /// <summary>
        /// Evaluate score for given gameboard state, hand and numberCategory
        /// </summary>
        /// <param name="gameboard"></param>
        /// <param name="hand"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        protected int EvaluateNumberCategoryCell(Gameboard gameboard, Hand hand, int number)
        {
            int score = hand.GetNumberOfRollsOfValue(number) * number;
            score += YahtzeeBonus(gameboard, hand);
            return score;
        }

        /// <summary>
        /// The yahtzee bonus score the current hand should be getting
        /// </summary>
        /// <param name="gameboard"></param>
        /// <param name="hand"></param>
        /// <returns></returns>
        protected int YahtzeeBonus(Gameboard gameboard, Hand hand)
        {
            return gameboard.ShouldHaveYahtzeeBonus() && hand.IsYahtzee() ? gameboard.YahtzeeBonus : 0;
        }

        public int Score => Math.Max(_score, 0);

        public int ScoreWithoutYahtzeeBonus(Gameboard gameboard)
        {
            if (_hasYahtzeeBonus)
            {
                return Score - gameboard.YahtzeeBonus;
            }

            return Score;
        }
    }
}