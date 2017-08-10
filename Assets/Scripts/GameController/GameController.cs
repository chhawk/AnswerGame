using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    StateMachine m_StateMachine;

    public GameObject FrameUI, QuestionUI, GameOverUI, MenuUI;
    public Text TimerText, ReadyGoText, AnswerTimeText;
    public float AnswerTime = 5.0f;

    public Player[] Players;

    public int State
    {
        get
        {
            return m_StateMachine == null ? -1 : m_StateMachine.CurStateID;
        }
    }

    void Awake()
    {
        ManagerResolver.Register<GameController>(this);
    }

    // Use this for initialization
    void Start ()
    {
        GameManager.Instance.Init(Players);

        m_StateMachine = new StateMachine();
        m_StateMachine.AddState(new WaitToGameState());
        m_StateMachine.AddState(new BattleGameState());
        m_StateMachine.AddState(new OverGameState());

        GameManager.Instance.GameState = eGameState.eStartGame;

        ChangeState(GameState.GS_Wait);
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_StateMachine != null)
            m_StateMachine.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = MenuUI.activeSelf;
            MenuUI.SetActive(!isActive);
        }
    }

    public void ChangeState(int gs, params object[] args)
    {
        m_StateMachine.ChangeState(gs, args);
    }

    public void StartCountDown()
    {
        StartCoroutine(CountDown(3));
    }

    IEnumerator CountDown(int time)
    {
        ReadyGoText.gameObject.SetActive(true);

        while (time > 0)
        {
            ReadyGoText.text = time.ToString();
            time--;
            ReadyGoText.gameObject.SetActive(true);

            yield return new WaitForSeconds(1.0f);
            ReadyGoText.gameObject.SetActive(false);
        }

        ReadyGoText.text = "Go!";
        ReadyGoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        ReadyGoText.gameObject.SetActive(false);
        ChangeState(GameState.GS_Battle);
    }

    public void PlayerAnswer(float answerTime, bool right)
    {
        StartCoroutine(EnumPlayerAnswer(answerTime, right));
    }

    IEnumerator EnumPlayerAnswer(float time, bool right)
    {
        yield return new WaitForSeconds(time);

        m_StateMachine.OnMessage(MsgID.PlayerAnswer, right);
    }

    public void GameStateCallback(eGameState gamestate)
    {
        Debug.Log(gamestate);

        switch (gamestate)
        {
            case eGameState.eStartGame:
                {
                    if (GameOverUI != null)
                        GameOverUI.SetActive(false);
                }
                break;

            case eGameState.eGameWin:
            case eGameState.eGameLost:
                {
                    ChangeState(GameState.GS_Over, gamestate);
                }
                break;

            default:
                break;
        }
    }

    public void OnQuit()
    {
        GameManager.Instance.OnQuit();
    }
}
