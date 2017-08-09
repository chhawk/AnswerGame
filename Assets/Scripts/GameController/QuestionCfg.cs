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

public class QuestionCfgReader
{
    static public ID_QuestionCfg QuestionCfg= new ID_QuestionCfg();

    public void Init()
    {
        // 加载配置 ...
        LoadConfig(GlobalSettings.QuestionCSV);
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

        QuestionCfg.Add(config.ID, config);
    }
}
