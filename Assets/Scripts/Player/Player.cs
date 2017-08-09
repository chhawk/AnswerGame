using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player 
{
    public GameObject m_PlayerInfoUI;

    [HideInInspector]
    public bool IsLocal
    {
        get;
        set;
    }

    int fRightRate = 90;

    public void StartAnswer()
    {
        if (IsLocal)
            return;

        ManagerResolver.Resolve<GameController>().PlayerAnswer(2, (Random.Range(0, 100) < fRightRate));
    }
}
