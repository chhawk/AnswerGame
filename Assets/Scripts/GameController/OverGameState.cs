using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverGameState : IState
{
    public int ID
    {
        get
        {
            return GameState.GS_Over;
        }
    }

    public void Enter(int lastState)
    {
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
