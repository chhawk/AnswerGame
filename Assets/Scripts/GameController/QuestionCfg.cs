using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ID_QuestionCfg = System.Collections.Generic.Dictionary<int, QuestionCfgInfo>;

public class QuestionCfgInfo
{
    public int ID = -1;
    public string Question;
    public List<string> Answers;
    public int RightAnswer = 0;
}

public class QuestionCfg
{
    static public ID_QuestionCfg QuestionDict= new ID_QuestionCfg();

    List<QuestionCfgInfo> m_CurrentList;
    QuestionCfgInfo m_CurrntQuestion = null;

    public QuestionCfg()
    {
        // 加载配置 ...
        LoadConfig(GlobalSettings.QuestionCSV);
    }

    public void Init()
    {
        m_CurrentList = new List<QuestionCfgInfo>(QuestionDict.Values);
        m_CurrntQuestion = null;
    }


    // 加载配置 ...
    public void LoadConfig(string path)
    {
        new CReadCsvTbBaseNew<QuestionCfgInfo>(path, Callme);
    }

    // 回调 ...
    public void Callme(QuestionCfgInfo config, int column)
    {
        // 空行不读取 ...
        if (config.Question == null)
            return;

        QuestionDict.Add(config.ID, config);
    }

    public QuestionCfgInfo GetRandQuestion(bool next)
    {
        if (m_CurrentList == null)
            return null;

        if (m_CurrentList.Count == 0 && next)
            return null;

        if(next || m_CurrntQuestion == null)
        {
            int rand = Random.Range(0, m_CurrentList.Count);
            m_CurrntQuestion = m_CurrentList[rand];
            m_CurrentList.Remove(m_CurrntQuestion);
        }

        return m_CurrntQuestion;
    }
}
