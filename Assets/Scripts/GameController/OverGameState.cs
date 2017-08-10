using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverGameState : IState
{
    const float OverTime = 5f;

    float m_fTimer;

    public int ID
    {
        get
        {
            return GameState.GS_Over;
        }
    }

    public void Enter(int lastState, params object[] args)
    {
        GameManager.Instance.GameState = (eGameState)args[0];

        if(GameManager.Instance.GameState == eGameState.eGameWin)
        {
            ManagerResolver.Resolve<GameController>().QuestionUI.SetActive(false);
            if(ManagerResolver.Resolve<GameController>().GameOverUI != null)
            {
                ManagerResolver.Resolve<GameController>().GameOverUI.SetActive(true);
                Animator anim = ManagerResolver.Resolve<GameController>().GameOverUI.transform.GetComponent<Animator>();
                anim.SetTrigger("Victory");
            }

        }
        else
        {
            ManagerResolver.Resolve<GameController>().QuestionUI.SetActive(false);
            if (ManagerResolver.Resolve<GameController>().GameOverUI != null)
            {
                ManagerResolver.Resolve<GameController>().GameOverUI.SetActive(true);
                Animator anim = ManagerResolver.Resolve<GameController>().GameOverUI.transform.GetComponent<Animator>();
                anim.SetTrigger("Defeat");
            }
        }

        m_fTimer = Time.realtimeSinceStartup + OverTime;
    }

    public void Update()
    {
        if (Time.realtimeSinceStartup > m_fTimer)
        {
            ManagerResolver.Resolve<GameController>().OnQuit();
        }
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
