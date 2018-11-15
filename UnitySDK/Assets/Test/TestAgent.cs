using System;
using System.Collections.Generic;
using CommonUtil;
using MLAgents;
using UnityEngine;
using Yahtzee.Game.Client;
using Yahtzee.Game.Client.GameActions;
using ILogger = CommonUtil.ILogger;
using Random = System.Random;

namespace Test
{
    public class TestAgent : Agent
    {
        public float timeBetweenDecisionsAtInference = 0.1f;
        private float timeSinceDecision;

        private TestAcademy _academy;
        private int _actionCount;
        private static int _gameCount;

        private List<GameAction> _actionTable;

        private ILogger Logger
        {
            get { return ServiceFactory.GetService<ILogger>(); }
        }
            
        public override void InitializeAgent()
        {
            Logger.Log(LogLevel.Info, "Initialize Agent");
            _academy = FindObjectOfType<TestAcademy>();

            _actionTable = new List<GameAction>();
            for (int i = 0; i < 45; i++)
            {
                _actionTable.Add(new GameActionRoll());
            }
        }
    
        public override void CollectObservations()
        {
            // dummy
            for (int i = 0; i < 70; i++)
            {
                AddVectorObs(1);
            }
            
            // mask actions
            _mask = new List<int>();
            // mask some random actions
            for (int i = 0; i < _actionTable.Count; i++)
            {
                if (UnityEngine.Random.value > 0.5f)
                {
                    _mask.Add(i);
                }
            }

            SetActionMask(0, _mask);
        }
        
        private List<int> _mask;
    
        public override void AgentAction(float[] vectorAction, string textAction)
        {
            int actionIndex = Mathf.FloorToInt(vectorAction[0]);
            var gameAction = _actionTable[actionIndex];
            if (brain.brainParameters.vectorActionSpaceType == SpaceType.discrete)
            {
                // ignore invalid actions
                if (_mask.Contains(actionIndex))
                {
                    Logger.Log(LogLevel.Error, "Invalid Game action attempted! " + gameAction.GetType());
                    return;
                }
            }
            
            // pretend GameAction was taken
            _actionCount++;
            
            SetReward(0.1f);
            if (_actionCount > 78) // gameover
            {
                EndTraining();
            }
        }

        private void EndTraining()
        {
            Logger.Log(LogLevel.Info, "EndTraining, _gameCount: " + ++_gameCount);
            Done();
        }
    
        public override void AgentReset()
        {
            Logger.Log(LogLevel.Debug, "AgentReset");
        }
        
        public void FixedUpdate()
        {
            if (!IsDone())
            {
                WaitTimeInference();
            }
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