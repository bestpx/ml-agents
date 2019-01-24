using System;
using System.Collections.Generic;
using CommonUtil;
using MLAgents;
using NUnit.Framework;
using UnityEngine;
using Yahtzee.Game.Client;
using Yahtzee.Game.Client.GameActions;
using Yahtzee.Game.Common;
using ILogger = CommonUtil.ILogger;

namespace Yahtzee.Game.MLAgent
{
    public class YahtzeePlayerAgent : Agent
    {
        public event Action<GameOverEvent> GameOverEvent = delegate {  };
        private YahtzeeAcademy _academy;
        public float timeBetweenDecisionsAtInference = 0.1f;
        private float timeSinceDecision;
        
        private Common.Game _game;

        private List<GameAction> _actionTable;

        private ILogger Logger
        {
            get { return ServiceFactory.GetService<ILogger>(); }
        }
            
        public override void InitializeAgent()
        {
            Logger.Log(LogLevel.Info, "Initialize Agent");
            _game = ServiceFactory.GetService<GameService>().CreateNewGame();
            _academy = FindObjectOfType<YahtzeeAcademy>();
            var gamesManager = FindObjectOfType<GamesManager>();
            gamesManager.AddAgent(this);
            
            _actionTable = new List<GameAction>()
            {
                new GameActionRoll(),
            };
            
            AddPlayHandActions();
            AddToggleHoldDiceActions();
        }

        private void AddPlayHandActions()
        {
            for (int i = 1; i < 14; i++)
            {
                _actionTable.Add(new PlayHandAction(i));
            }
        }

        /// <summary>
        /// There should be a total of 31 choices
        /// </summary>
        private void AddToggleHoldDiceActions()
        {
            AddToggleHoldDiceActionsRecur(new bool[5], 0);
        }

        private void AddToggleHoldDiceActionsRecur(bool[] array, int index)
        {
            bool[] localArray = new bool[5];
            bool[] localArray2 = new bool[5];
            for (int i = 0; i < 5; i++)
            {
                localArray[i] = array[i];
                localArray2[i] = array[i];
            }
            
            if (index == localArray.Length)
            {
                // don't hold all dice
                if (AllTrue(array))
                {
                    return;
                }
                _actionTable.Add(new ToggleHoldAndRollDiceAction(localArray));
                return;
            }

            localArray[index] = true;
            localArray2[index] = false;
            AddToggleHoldDiceActionsRecur(localArray, index + 1);
            AddToggleHoldDiceActionsRecur(localArray2, index + 1);
        }

        private bool AllTrue(bool[] array)
        {
            bool value = true;
            for (int i = 0; i < array.Length; i++)
            {
                value = value && array[i];
            }

            return value;
        }
    
        public override void CollectObservations()
        {
            // observe gameboard
            float maxPossible = _game.Gameboard.ShouldHaveYahtzeeBonus() ? 80 : 50;
            for (int i = 1; i < 14; i++)
            {
                AddVectorObs(_game.GetScoreInCell(i) / 50); // 13
            }
           
            // observe hand
            for (int i = 0; i < 5; i++)
            {
                AddVectorObs(_game.GetDiceAt(i), 7);  // 7 * 5 = 35
            }
            for (int i = 0; i < 5; i++)
            {
                AddVectorObs(_game.Hand.IsLockedAt(i));  // 5
            }
            // observe gameboard cells, this affects the available decisions
            for (int i = 1; i < 14; i++)
            {
                AddVectorObs(_game.CanPlayInCell(i)); // 13
            }
            // observe potential scores
            for (int i = 1; i < 14; i++)
            {
                AddVectorObs(_game.GetCell(i).EvaluateScore(_game.Hand, _game.Gameboard)); // 13
            }
            AddVectorObs(_game.CanRoll());
            AddVectorObs(_game.CanToggle());
            AddVectorObs(_game.Hand.RollCount);
            AddVectorObs(_game.Gameboard.ShouldHaveYahtzeeBonus()); // 4

            // observe section bonus info
            int leftColumnScoreBefore = _game.Gameboard.GetLeftColumnScoreWithoutYahtzeeBonus();
            AddVectorObs((float)leftColumnScoreBefore / Gameboard.SectionBonusThreshold);
            AddVectorObs(_game.Gameboard.HasSectionBonus);

            // mask actions
            _mask = new List<int>();
            for (int i = 0; i < _actionTable.Count; i++)
            {
                if (!_actionTable[i].IsValid(_game))
                {
                   _mask.Add(i);
                }
            }

            SetActionMask(0, _mask);

            //Added to test if the mask can be learned
            //float[] maskObs = new float[_actionTable.Count];
            //for (var maskIndex = 0; maskIndex < _mask.Count; maskIndex++)
            //{
            //    maskObs[maskIndex] = _mask[maskIndex];
            //}
            //AddVectorObs(maskObs);
            // End of code added to test if the mask can be learned
        }

        private List<int> _mask;
    
        public override void AgentAction(float[] vectorAction, string textAction)
        {
            int actionIndex = Mathf.FloorToInt(vectorAction[0]);
            int scoreCurrentTurn = 0;
            int scoreGainedInLeftColumnWithoutYahtzeeBonus = 0;
            var gameAction = _actionTable[actionIndex];
            if (brain.brainParameters.vectorActionSpaceType == SpaceType.discrete)
            {
                int scoreBefore = _game.Gameboard.GetScoreWithoutSectionBonus();
                int leftColumnScoreBefore = _game.Gameboard.GetLeftColumnScoreWithoutYahtzeeBonus();
                // do actions

                // ignore invalid actions
                if (!gameAction.IsValid(_game))
                {
                    Logger.Log(LogLevel.Error, "Invalid Game action attempted! " + gameAction.GetType());
                    return;
                }
                _actionTable[actionIndex].Perform(_game);
                
                // parse action result
                int scoreAfter = _game.Gameboard.GetScoreWithoutSectionBonus();
                int scoreLeftColumnAfter = _game.Gameboard.GetLeftColumnScoreWithoutYahtzeeBonus();
                scoreCurrentTurn = scoreAfter - scoreBefore;
                scoreGainedInLeftColumnWithoutYahtzeeBonus = scoreLeftColumnAfter - leftColumnScoreBefore;
            }

            // Reward agent
            //float highestPossible = _game.Gameboard.ShouldHaveYahtzeeBonus() ? 80.0f : 50.0f;
            //float sectionBonusPercentage = (float) scoreGainedInLeftColumnWithoutYahtzeeBonus / Gameboard.SectionBonusThreshold *
            //    Gameboard.SectionBonus;
            float normalizedReward = scoreCurrentTurn / 50.0f;
            SetReward(normalizedReward);
            Logger.Log(LogLevel.Debug, "normalizedReward:" + normalizedReward);
            if (_game.IsGameOver()) // gameover
            {
                EndTraining();
            }
        }

        private void EndTraining()
        {
            GameOverEvent(new GameOverEvent(_game));
            Logger.Log(LogLevel.Debug, "Game finished with score: " + _game.GetScore());
            Done();
        }
    
        public override void AgentReset()
        {
            Logger.Log(LogLevel.Debug, "AgentReset");
            _game = ServiceFactory.GetService<GameService>().CreateNewGame();
        }
        
        public void FixedUpdate()
        {
            WaitTimeInference();
        }

        private void WaitTimeInference()
        {
            if (!_academy.GetIsInference())
            {
                RequestDecision();
            }
            else
            {
                if (timeSinceDecision >= timeBetweenDecisionsAtInference)
                {
                    timeSinceDecision = 0f;
                    RequestDecision();
                }
                else
                {
                    timeSinceDecision += Time.fixedDeltaTime;
                }
            }
        }
    }
}