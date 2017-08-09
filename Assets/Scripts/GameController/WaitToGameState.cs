using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToGameState : IState
{
    public int ID
    {
        get
        {
            return GameState.GS_Wait;
        }
    }

    public void Enter(int lastState, params object[] args)
    {
        ManagerResolver.Resolve<GameController>().StartCountDown();
    }

    public void Update()
    {
    }

    public void Exit(int nextState)
    {
    }

    public void OnMessage(MsgID msg, params object[] args)
    {
        //switch (msg)
        //{
        //    default:
        //        break;
        //}
    }
}
