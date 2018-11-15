using System.Collections;
using System.Collections.Generic;
using CommonUtil;
using MLAgents;
using Yahtzee.Game.MLAgent;
using ILogger = CommonUtil.ILogger;

public class YahtzeeAcademy : Academy
{
    private ILogger Logger => ServiceFactory.GetService<ILogger>();

    public override void AcademyReset()
    {
        base.AcademyReset();
        Logger.Log(LogLevel.Warning, "Academy reset");
    }
}
