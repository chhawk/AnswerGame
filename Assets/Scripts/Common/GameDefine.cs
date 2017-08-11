using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GlobalSettings
{
    //tag

    //csv
    public const string QuestionCSV = "Csv/Question";
    public const string ItemCSV = "Csv/Item";

    //prefabs相关

    // 变量 ...
}

public enum eGameState
{
    eNone = -1,
    eStartGame = 0,
    eGameWin = 1,
    eGameLost = 2
}

public class GameState
{
    public const int GS_Wait = 1;
    public const int GS_Battle = 2;
    public const int GS_Over = 3;
}

public enum MsgID
{
    PlayerAnswer,
    ItemUse,
}

public interface IState
{
    void Enter(int lastState, params object[] args);
    void Update();
    void Exit(int nextState);

    int ID
    {
        get;
    }

    void OnMessage(MsgID msg, params object[] args);
}
