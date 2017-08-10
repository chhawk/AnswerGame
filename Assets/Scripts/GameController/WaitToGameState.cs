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
        if (ManagerResolver.Resolve<GameController>().FrameUI != null)
            ManagerResolver.Resolve<GameController>().FrameUI.SetActive(false);
        if (ManagerResolver.Resolve<GameController>().TimerText != null)
            ManagerResolver.Resolve<GameController>().TimerText.transform.parent.gameObject.SetActive(false);
        if (ManagerResolver.Resolve<GameController>().ReadyGoText != null)
            ManagerResolver.Resolve<GameController>().ReadyGoText.gameObject.SetActive(false);
        if (ManagerResolver.Resolve<GameController>().QuestionUI != null)
            ManagerResolver.Resolve<GameController>().QuestionUI.SetActive(false);
        if (ManagerResolver.Resolve<GameController>().GameOverUI != null)
            ManagerResolver.Resolve<GameController>().GameOverUI.SetActive(false);

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
