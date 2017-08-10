using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager
{
    static GameManager instance;

    int m_nCurrentPlayer = -1;

    QuestionCfgReader m_QuestionCfg;

    public List<Player> m_ListPlayer = new List<Player>();

    public eGameState GameState
    {
        get;
        set;
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();

            return GameManager.instance; 
        }
            
    }

    GameManager()
    {
        GameState = eGameState.eNone;

        m_QuestionCfg = new QuestionCfgReader();
    }

    public void Init(Player[] players)
    {
        m_QuestionCfg.Init();

        bool local = true;
        foreach (Player p in players)
        {
            p.IsLocal = local;
            local = false;
            m_ListPlayer.Add(p);
        }
    }

    public Player GetCurPlayer(bool next = false)
    {
        if(m_nCurrentPlayer == -1)
        {
            m_nCurrentPlayer = Random.Range(0, m_ListPlayer.Count);
        }
        else if(next)
        {
            m_nCurrentPlayer++;
            if (m_nCurrentPlayer >= m_ListPlayer.Count)
                m_nCurrentPlayer = 0;
        }

        return m_ListPlayer[m_nCurrentPlayer];
    }

    public QuestionCfgInfo GetCurQuestion(bool next = false)
    {
        return m_QuestionCfg.GetRandQuestion(next);
    }

    public void OnQuit()
    {
        m_nCurrentPlayer = -1;
        m_ListPlayer.Clear();

        SceneManager.LoadSceneAsync(0);
    }

}
