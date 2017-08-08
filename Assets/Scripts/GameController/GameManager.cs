using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager
{
    static GameManager instance;

    byte m_AssignID = 0;

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
    }

    public byte GetAssignID()
    {
        return m_AssignID++;
    }
 

    public void OnGameOver(eGameState state)
    {
        GameState = state;
    }
}
