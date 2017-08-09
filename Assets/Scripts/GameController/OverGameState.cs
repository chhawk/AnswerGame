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

    public void Enter(int lastState, params object[] args)
    {
        eGameState gamestate = (eGameState)args[0];

        if(gamestate == eGameState.eGameWin)
        {
            ManagerResolver.Resolve<GameController>().QuestionUI.SetActive(false);
            //ManagerResolver.Resolve<GameController>().GameOverUI.SetActive(true);
            //Animator anim = ManagerResolver.Resolve<GameController>().GameOverUI.transform.GetComponent<Animator>();
            //anim.SetTrigger("Victory");

        }
        else
        {
            ManagerResolver.Resolve<GameController>().QuestionUI.SetActive(false);
            //ManagerResolver.Resolve<GameController>().GameOverUI.SetActive(true);
            //Animator anim = ManagerResolver.Resolve<GameController>().GameOverUI.transform.GetComponent<Animator>();
            //anim.SetTrigger("Defeat");
        }
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
