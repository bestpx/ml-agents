using CommonUtil;
using UnityEngine;
using ILogger = CommonUtil.ILogger;
using Logger = CommonUtil.Logger;

namespace Yahtzee.Game.Client.GameActions
{
    public class PlayHandAction : GameAction
    {
        /// <summary>
        /// CellId to play hand in
        /// </summary>
        private readonly int _cellId;
        public PlayHandAction(int cellId) : base()
        {
            _cellId = cellId;
        }

        public override bool IsValid(Common.Game game)
        {
            return game.CanPlayInCell(_cellId);
        }

        public override void Perform(Common.Game game)
        {
            int score = game.GetScore();
            string handStr = game.Hand.ToString();
            game.PlayHand(_cellId);
            Logger.Log(LogLevel.Debug, 
                "Playing hand: " + handStr + " on cell: " + game.GetCellTypeAt(_cellId).Name + ", score: " + (game.GetScore() - score));
        }

        public override void Revert(Common.Game game)
        {
            throw new System.NotImplementedException();
        }

        public override int MeanExpectation(Common.Game game)
        {
            return game.GetCell(_cellId).MeanExpectation(game.Gameboard);
        }

        public override int MaximumPossible(Common.Game game)
        {
            return game.GetCell(_cellId).MaximumPossible(game.Gameboard);
        }
    }
}