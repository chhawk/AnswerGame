using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UISceneController : MonoBehaviour {

    private GameObject m_LoadingUI;
    private GameObject m_MainMenuUI;
    private GameObject m_QuitGameUI;

    public Image m_LoadingProgress;
    public Button[] m_StartLoadButton;

    void Awake()
    {
    }

    // Use this for initialization
    void Start ()
    {
        m_LoadingUI = transform.Find("Panelloading").gameObject;
        m_MainMenuUI = transform.Find("Panelmain").gameObject;
        m_QuitGameUI = m_MainMenuUI.transform.Find("QuitGame").gameObject;
        Button quitBtn = m_QuitGameUI.transform.GetComponentInChildren<Button>();
        MyPointEvent.AutoAddListener(quitBtn, OnQuit, null);

        if (m_StartLoadButton != null)
        {
            for (int i = 0; i < m_StartLoadButton.Length; i++)
            {
                if (m_StartLoadButton[i] != null)
                    MyPointEvent.AutoAddListener(m_StartLoadButton[i], StartLoadEvent, null);
            }
        }

        if (GameManager.Instance.GameState > eGameState.eStartGame)
        {
            ShowGameResult();
        }

    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = m_QuitGameUI.activeSelf;
            m_QuitGameUI.SetActive(!isActive);
        }
    }

    void OnQuit(UIBehaviour ui, EventTriggerType eventtype, object message, byte count)
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(int index)
    {
        float displayProgress = 0;
        float toProgress = 0;

        AsyncOperation async = SceneManager.LoadSceneAsync(index);
        async.allowSceneActivation = false;
        while (async.progress < 0.9f)
        {
            toProgress = async.progress;
            while (displayProgress < toProgress)
            {
                displayProgress += 0.01f;

                m_LoadingProgress.fillAmount = displayProgress;
                yield return new WaitForEndOfFrame();
            }
        }

        toProgress = 1.0f;
        while (displayProgress < toProgress)
        {
            displayProgress += 0.05f;
            m_LoadingProgress.fillAmount = displayProgress;
            yield return new WaitForEndOfFrame();
        }

        async.allowSceneActivation = true;
        Debug.Log("Loading complete");
    }

    public void StartLoad(int index)
    {
        m_MainMenuUI.SetActive(false);
        m_LoadingUI.SetActive(true);

        StartCoroutine(LoadLevel(index));
    }

    void StartLoadEvent(UIBehaviour ui, EventTriggerType eventtype, object message, byte count)
    {
        ui.gameObject.SetActive(false);
        StartLoad(1);
    }

    void ShowGameResult()
    {
        Transform tfm = m_MainMenuUI.transform.Find("Result");
        if (tfm != null)
        {
            tfm.gameObject.SetActive(true);

            if (GameManager.Instance.GameState == eGameState.eGameWin)
            {
                tfm.Find("Gamefinally").GetChild(0).gameObject.SetActive(true);
                tfm.Find("Gamefinally").GetChild(1).gameObject.SetActive(false);

                tfm = tfm.Find("Starbackground");
                for (int i = 0; i < 2; i++)
                {
                    Animator ani = tfm.GetChild(i).GetComponent<Animator>();
                    ani.SetBool("donghua", true);
                }
            }
            else
            {
                tfm.Find("Gamefinally").GetChild(0).gameObject.SetActive(false);
                tfm.Find("Gamefinally").GetChild(1).gameObject.SetActive(true);
            }
        }
    }
}
