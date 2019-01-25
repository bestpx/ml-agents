using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using CommonUtil;
using Yahtzee;
using Yahtzee.Game.Common;
using Yahtzee.Game.Common.GameCells;

public class NewEditModeTest 
{
    [Test]
    public void GameboardTest()
    {
        ServiceFactory.Register<CommonUtil.ILogger>(new CommonUtil.Logger(LogLevel.Trace));
        var gameboard = GameboardFactory.MakeClassicGameboard();
        var hand = new Hand(new int[]{1,2,3,4,5, 6,6,6,6,6, 1,2,3,4,1, 1,2,3,4,5});
        Assert.IsTrue(gameboard.GetScore() == 0);
        
        hand.Roll(); // hand: 1,2,3,4,5
        // test each cell
        Assert.IsTrue(gameboard.EvaluateHandWithCell(1, hand) == 1);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(2, hand) == 2);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(3, hand) == 3);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(4, hand) == 4);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(5, hand) == 5);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(6, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(7, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(8, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(9, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(10, hand) == 30);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(11, hand) == 40);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(12, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(13, hand) == 15);
        
        Assert.IsTrue(hand.CanRoll());
        hand.Roll(); // hand: 6,6,6,6,6
        // test each cell
        Assert.IsTrue(gameboard.EvaluateHandWithCell(1, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(2, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(3, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(4, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(5, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(6, hand) == 30);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(7, hand) == 30);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(8, hand) == 30);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(9, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(10, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(11, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(12, hand) == 50);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(13, hand) == 30);
        
        // score a yahtzee
        gameboard.PlayHand(12, hand);
        
        // eval again
        Assert.IsTrue(gameboard.EvaluateHandWithCell(1, hand) == 50);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(2, hand) == 50);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(3, hand) == 50);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(4, hand) == 50);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(5, hand) == 50);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(6, hand) == 80);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(7, hand) == 80);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(8, hand) == 80);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(9, hand) == 50);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(10, hand) == 50);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(11, hand) == 50);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(12, hand) == 50);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(13, hand) == 80);

        Assert.IsTrue(hand.CanRoll());
        // hand: 1,2,3,4,1
        hand.Roll();
        // eval again
        Assert.IsTrue(gameboard.EvaluateHandWithCell(1, hand) == 2);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(2, hand) == 2);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(3, hand) == 3);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(4, hand) == 4);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(5, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(6, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(7, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(8, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(9, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(10, hand) == 30);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(11, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(12, hand) == 0);
        Assert.IsTrue(gameboard.EvaluateHandWithCell(13, hand) == 11);

        Assert.IsTrue(!hand.CanRoll());
    }


    [Test]
    public void GamePlayTest() 
    {
        ServiceFactory.Register<CommonUtil.ILogger>(new CommonUtil.Logger(LogLevel.Trace));
        // Use the Assert class to test conditions.
        var game = ServiceFactory.GetService<GameService>().CreateNewGame();
        
        // verify initial conditions
        Assert.IsTrue(!game.IsGameOver());
        Assert.IsTrue(game.GetScore() == 0);
        Assert.IsTrue(game.GetScoreInCell(1) == 0);
        Assert.IsTrue(game.GetScoreInCell(2) == 0);
        Assert.IsTrue(game.GetScoreInCell(3) == 0);
        Assert.IsTrue(game.GetScoreInCell(4) == 0);
        Assert.IsTrue(game.GetScoreInCell(5) == 0);
        Assert.IsTrue(game.GetScoreInCell(6) == 0);
        Assert.IsTrue(game.GetScoreInCell(7) == 0);
        Assert.IsTrue(game.GetScoreInCell(8) == 0);
        Assert.IsTrue(game.GetScoreInCell(9) == 0);
        Assert.IsTrue(game.GetScoreInCell(10) == 0);
        Assert.IsTrue(game.GetScoreInCell(11) == 0);
        Assert.IsTrue(game.GetScoreInCell(12) == 0);
        Assert.IsTrue(game.GetScoreInCell(13) == 0);
        Assert.IsTrue(!game.CanPlayInCell(1));
        Assert.IsTrue(game.CanRoll());
        Assert.IsTrue(!game.CanToggle());
        
        // roll for the first time
        game.Roll();
        Assert.IsTrue(game.CanToggle());
        Assert.IsTrue(game.CanPlayInCell(1));
        Assert.IsTrue(game.CanPlayInCell(2));
        Assert.IsTrue(game.CanPlayInCell(3));
        Assert.IsTrue(game.CanPlayInCell(4));
        Assert.IsTrue(game.CanPlayInCell(5));
        Assert.IsTrue(game.CanPlayInCell(6));
        Assert.IsTrue(game.CanPlayInCell(7));
        Assert.IsTrue(game.CanPlayInCell(8));
        Assert.IsTrue(game.CanPlayInCell(9));
        Assert.IsTrue(game.CanPlayInCell(10));
        Assert.IsTrue(game.CanPlayInCell(11));
        Assert.IsTrue(game.CanPlayInCell(12));
        Assert.IsTrue(game.CanPlayInCell(13));
        game.ToggleHand(new bool[]{true, true, true, true, true});
        Assert.IsTrue(!game.CanRoll());
        game.ToggleHand(new bool[]{false, true, false, false, false});
        Assert.IsTrue(game.CanRoll());
        
        // roll for two more times
        game.Roll();
        game.Roll();
        
        Assert.IsTrue(!game.CanRoll());
        
        // game score
        Assert.IsTrue(game.GetScore() == 0);
        // score in cell
        game.PlayHand(1);
    }
}
