using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CommonUtil;
using UnityEngine;
using Yahtzee;
using ILogger = CommonUtil.ILogger;
using Logger = CommonUtil.Logger;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private LogLevel _level = LogLevel.Info;
    
    private void Awake()
    {
        ServiceFactory.Register<ILogger>(new Logger(_level));
    }
}
