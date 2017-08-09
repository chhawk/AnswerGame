using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGameState : IState
{
    float m_fTimer;

    public int ID
    {
        get
        {
            return GameState.GS_Battle;
        }
    }

    public void Enter(int lastState)
    {
        ManagerResolver.Resolve<GameController>().TimerText.transform.parent.gameObject.SetActive(true);
        ManagerResolver.Resolve<GameController>().TimerText.text = "00:00";
        ManagerResolver.Resolve<GameController>().QuestionUI.SetActive(true);
    }

    public void Update()
    {
        m_fTimer += Time.deltaTime;

        int second = (int)m_fTimer;
        int minute = second / 60;
        second = second % 60;
        ManagerResolver.Resolve<GameController>().TimerText.text = string.Format("{0:D2}:{1:D2}", minute, second);
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
