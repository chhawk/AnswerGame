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

        OnChangePlayer();
    }

    void OnChangePlayer()
    {
        m_fAnswerTimer = ManagerResolver.Resolve<GameController>().AnswerTime;

        Player player = GameManager.Instance.GetCurPlayer(true);
        ManagerResolver.Resolve<GameController>().FrameUI.SetActive(true);
        Transform ui = player.m_PlayerInfoUI.transform;
        ManagerResolver.Resolve<GameController>().FrameUI.transform.SetPositionAndRotation(ui.position, ui.rotation);
        ManagerResolver.Resolve<ItemGui>().OnChangePlayer(player.IsLocal);

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
                btn.gameObject.SetActive(true);
                Text t = btn.transform.GetComponentInChildren<Text>();
                t.text = question.Answers[answerId[index]];
                btn.enabled = player.IsLocal;
                MyPointEvent.AutoAddListener(btn, OnAnswerBtnClick, answerId[index]);
                ++index;
            }

            player.StartAnswer();
        }
    }

    void OnAnswerBtnClick(UIBehaviour ui, EventTriggerType eventtype, object message, byte count)
    {
        int id = (int)message;
        QuestionCfgInfo question = GameManager.Instance.GetCurQuestion();
        if(id == question.RightAnswer)
        {
            Debug.Log("right!!");

            OnChangePlayer();
        }
        else
        {
            RequestEnd();
        }
    }

    void RequestEnd()
    {
        Player player = GameManager.Instance.GetCurPlayer();
        Debug.LogError("wrong!! local:" + player.IsLocal);
        if (player.IsLocal)
            ManagerResolver.Resolve<GameController>().TryLost();
        else
            ManagerResolver.Resolve<GameController>().GameStateCallback(eGameState.eGameWin);
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
                RequestEnd();
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
                    OnAnswerBtnClick(null, EventTriggerType.PointerClick, right ? GameManager.Instance.GetCurQuestion().RightAnswer : 4, 0);
                }
                break;

            case MsgID.ItemUse:
                {
                    byte id = (byte)args[0];
                    OnItemUsed(id);
                }
                break;

            default:
                break;
        }
    }

    void OnItemUsed(byte id)
    {
        ItemInfo info = ItemCfg.ItemDict[id];
        
        if (info.m_nRelive > 0) //复活
        {
            OnChangePlayer();
        }

        if (info.m_nAvoid > 0) //免答
        {
            OnChangePlayer();
        }

        if (info.m_nRemoveWrong > 0) //挑错
        {
            Button[] btns = ManagerResolver.Resolve<GameController>().QuestionUI.GetComponentsInChildren<Button>();
            foreach (Button btn in btns)
            {
            }
        }

        if(info.m_nAddTime > 0) //延时
        {
            m_fAnswerTimer += info.m_nAddTime;
        }
    }
}
