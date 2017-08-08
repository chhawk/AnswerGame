using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine
{
    Dictionary<int, IState> m_DictStates = new Dictionary<int, IState>();

    public IState CurState
    {
        get;
        set;
    }

    public int CurStateID
    {
        get
        {
            if (CurState == null)
                return -1;

            return CurState.ID;
        }
    }

    private bool m_SwitchStateImme = false;      // 立即切换状态 ...

    public void SetSwitchStateImme(bool imme) { m_SwitchStateImme = imme; }

    public void AddState(IState state)
    {
        if (!m_DictStates.ContainsKey(state.ID))
        {
            m_DictStates.Add(state.ID, state);
        }
    }

    public void ReplaceState(IState state)
    {
        if (m_DictStates.ContainsKey(state.ID))
        {
            m_DictStates.Remove(state.ID);
        }

        m_DictStates.Add(state.ID, state);
    }

    public IState FindState(int id)
    {
        if (m_DictStates.ContainsKey(id))
            return m_DictStates[id];

        return null;
    }

    public void ChangeState(int id)
    {
        if (m_DictStates.ContainsKey(id))
        {
            ChangeState(m_DictStates[id]);
        }
    }

    public void ChangeState(IState newstate)
    {
        if (newstate == null)
            return;

        if (!m_SwitchStateImme)
        {
            if (CurState == newstate)
                return;
        }
        
        if (CurState != null)
            CurState.Exit(newstate.ID);

        newstate.Enter(CurStateID);

        CurState = newstate;
    }

    public void Update()
    {
        if (CurState != null)
            CurState.Update();
    }

    public void OnMessage(MsgID msg, params object[] args)
    {
        if (CurState != null)
            CurState.OnMessage(msg, args);
    }
}
