using System;
using System.Collections.Generic;
using System.Linq;
using CommonUtil;
using UnityEngine;
using UnityEngine.Analytics;
using Yahtzee.Game.MLAgent;
using ILogger = CommonUtil.ILogger;

namespace Yahtzee.Game.Client
{
    public class GamesManager : MonoBehaviour
    {
        private int _gameCount = 0;
        private float _averageScore = 0;
        private int _highScore = 0;
        private int _gamesWithYahtzee = 0;
        private int _gamesWithSectionBonus = 0;

        private LinkedList<int> _scoreLast100 = new LinkedList<int>();
        
        private ILogger Logger
        {
            get { return ServiceFactory.GetService<ILogger>(); }
        }
        
        public void AddAgent(YahtzeePlayerAgent agent)
        {
            agent.GameOverEvent += ProcessGameOverEvent;
        }

        private void ProcessGameOverEvent(GameOverEvent evt)
        {
            float score = evt.Game.GetScore();
            int gameCountNext = _gameCount + 1;
            _averageScore = (_averageScore * _gameCount / gameCountNext) + (score / gameCountNext);
            _gameCount = gameCountNext;
            _highScore = Mathf.Max(evt.Game.GetScore(), _highScore);

            if (evt.Game.Gameboard.ShouldHaveYahtzeeBonus())
            {
                _gamesWithYahtzee++;
            }
            if (evt.Game.Gameboard.HasSectionBonus)
            {
                _gamesWithSectionBonus++;
            }

            double averageLast100 = 0;
            _scoreLast100.AddLast(new LinkedListNode<int>(evt.Game.GetScore()));
            if (_scoreLast100.Count > 100)
            {
                _scoreLast100.RemoveFirst();
                averageLast100 = _scoreLast100.Average();
            }
            
            Logger.Log(LogLevel.Info, "GameOver with score: " + score + "\n Average score for last 100 games: " + averageLast100 + "\n High score: " + _highScore);
            Logger.Log(LogLevel.Info, "_gameCount: " + _gameCount + 
                ", games with yahtzee: " + _gamesWithYahtzee +
                ", games with section bonus: " + _gamesWithSectionBonus);
        }
    }
}