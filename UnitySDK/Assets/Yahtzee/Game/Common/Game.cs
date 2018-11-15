using System;
using System.Text;
using CommonUtil;
using UnityEngine;
using Yahtzee.Game.Common.GameCells;
using ILogger = CommonUtil.ILogger;
using Logger = CommonUtil.Logger;

namespace Yahtzee.Game.Common
{
    /// <summary>
    /// Contains gameboard state and other meta data for the game
    /// </summary>
    public class Game
    {
        private string _gameId;
        private Gameboard _gameboard;
        private Hand _hand;
        
        private Game() {}

        Game(string gameId, Gameboard gameboard)
        {
            _gameId = gameId;
            _gameboard = gameboard;
            _hand = new Hand();
        }

        #region queries
        public bool IsGameOver()
        {
            return _gameboard.AllCellsPlayed();
        }

        public int GetScore()
        {
            return _gameboard.GetScore();
        }

        public int GetScoreInCell(int cellId)
        {
            return _gameboard.GetScoreInCell(cellId);
        }

        public bool CanRoll()
        {
            return _hand.CanRoll();
        }

        public bool CanPlayInCell(int cellId)
        {
            return _hand.HasRolled() && !_gameboard.HasPlayedInCell(cellId);
        }

        public bool CanToggle()
        {
            return _hand.HasRolled() && !_hand.HasChangedLockedDiceSinceLastRoll && !_hand.IsLastRoll();
        }

        public int GetDiceAt(int index)
        {
            return _hand.GetRollAt(index);
        }

        public Type GetCellTypeAt(int cellId)
        {
            return _gameboard.GetCellType(cellId);
        }
        
        public GameCell GetCell(int cellId)
        {
            return _gameboard.GetCell(cellId);
        }

        public Gameboard Gameboard => _gameboard;

        public Hand Hand => _hand;

        #endregion

        #region actions
        public void Roll()
        {
            if (_hand.CanRoll())
            {
                _hand.Roll();
            }
            else
            {
                Debug.Log("Roll");
            }
        }

        public void PlayHand(int cellId)
        {
            _gameboard.PlayHand(cellId, _hand);
            _hand = new Hand();
        }

        public void ToggleHand(bool[] toggle)
        {
            _hand.SetLockStatus(toggle);
        }

        #endregion

        /// <summary>
        /// Factory method
        /// </summary>
        public static class GameFactory
        {
            public static Game MakeClassicGame()
            {
                var gameboard = GameboardFactory.MakeClassicGameboard();
                var guid = Guid.NewGuid();
                return new Game(guid.ToString(), gameboard);
            }
        }
    }
}