using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo
{
    public byte m_nID;
    public string m_cName;
    public byte m_nImgIndex;
    public float m_fCDTime;
    public byte m_nRelive;
    public byte m_nAvoid;
    public byte m_nAddTime;
    public byte m_nRemoveWrong;
}

public class ItemCfg
{
    static public Dictionary<byte, ItemInfo> ItemDict = new Dictionary<byte, ItemInfo>();


    public ItemCfg()
    {
        // 加载配置 ...
        LoadConfig(GlobalSettings.ItemCSV);
    }

    public void Init()
    {
    }


    // 加载配置 ...
    public void LoadConfig(string path)
    {
        new CReadCsvTbBaseNew<ItemInfo>(path, Callme);
    }

    // 回调 ...
    public void Callme(ItemInfo config, int column)
    {
        // 空行不读取 ...
        if (config.m_cName == null)
            return;

        ItemDict.Add(config.m_nID, config);
    }
}
