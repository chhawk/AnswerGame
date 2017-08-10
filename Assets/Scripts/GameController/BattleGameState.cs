using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleGameState : IState
{
    float m_fTimer;
    float m_fAnswerTimer = 0;

    public int ID
    {
        get
        {
            return GameState.GS_Battle;
        }
    }

    public void Enter(int lastState, params object[] args)
    {
        ManagerResolver.Resolve<GameController>().TimerText.transform.parent.gameObject.SetActive(true);
        ManagerResolver.Resolve<GameController>().TimerText.text = "00:00";

        BeginAnswer();
    }

    void BeginAnswer()
    {
        m_fAnswerTimer = ManagerResolver.Resolve<GameController>().AnswerTime;

        Player player = GameManager.Instance.GetCurPlayer(true);
        ManagerResolver.Resolve<GameController>().FrameUI.SetActive(true);
        Transform ui = player.m_PlayerInfoUI.transform;
        ManagerResolver.Resolve<GameController>().FrameUI.transform.SetPositionAndRotation(ui.position, ui.rotation);

        QuestionCfgInfo question = GameManager.Instance.GetCurQuestion(true);
        if (question != null)
        {
            Text qText = ManagerResolver.Resolve<GameController>().QuestionUI.transform.Find("Question").transform.GetComponent<Text>();
            qText.text = question.Question;

            ManagerResolver.Resolve<GameController>().QuestionUI.SetActive(true);
            Button[] btns = ManagerResolver.Resolve<GameController>().QuestionUI.GetComponentsInChildren<Button>();
            int[] answerId = { 0, 1, 2, 3 };
            answerId = Utility.RandomSort<int>(answerId);
            int index = 0;
            foreach (Button btn in btns)
            {
                Text t = btn.transform.GetComponentInChildren<Text>();
                t.text = question.Answers[answerId[index]];
                btn.enabled = player.IsLocal;
                MyPointEvent.AutoAddListener(btn, OnQuitDefBtnClick, answerId[index]);
                ++index;
            }

            player.StartAnswer();
        }
    }

    void OnQuitDefBtnClick(UIBehaviour ui, EventTriggerType eventtype, object message, byte count)
    {
        int id = (int)message;
        QuestionCfgInfo question = GameManager.Instance.GetCurQuestion();
        if(id == question.RightAnswer)
        {
            Debug.Log("right!!");

            BeginAnswer();
        }
        else
        {
           Player player = GameManager.Instance.GetCurPlayer();
           Debug.LogError("wrong!! local:" + player.IsLocal);
           ManagerResolver.Resolve<GameController>().GameStateCallback(player.IsLocal ? eGameState.eGameLost : eGameState.eGameWin);
        }
    }

    public void Update()
    {
        m_fTimer += Time.deltaTime;

        int second = (int)m_fTimer;
        int minute = second / 60;
        second = second % 60;
        ManagerResolver.Resolve<GameController>().TimerText.text = string.Format("{0:D2}:{1:D2}", minute, second);

        if(m_fAnswerTimer > 0.0f)
        {
            ManagerResolver.Resolve<GameController>().AnswerTimeText.text = string.Format("{0}", (int)m_fAnswerTimer);

            m_fAnswerTimer -= Time.deltaTime;

            if (m_fAnswerTimer <= 0.0f)
            {
                Player player = GameManager.Instance.GetCurPlayer();
                ManagerResolver.Resolve<GameController>().GameStateCallback(player.IsLocal ? eGameState.eGameLost : eGameState.eGameWin);
            }
        }
    }

    public void Exit(int nextState)
    {
    }

    public void OnMessage(MsgID msg, params object[] args)
    {
        switch (msg)
        {
            case MsgID.PlayerAnswer:
                {
                    bool right = (bool)args[0];
                    OnQuitDefBtnClick(null, EventTriggerType.Cancel, right ? GameManager.Instance.GetCurQuestion().RightAnswer : 4, 0);
                }
                break;
            default:
                break;
        }
    }
}
