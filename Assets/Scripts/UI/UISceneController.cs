using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UISceneController : MonoBehaviour {

    private GameObject m_LoadingUI;
    private GameObject m_MainMenuUI;

    public Image m_LoadingProgress;
    public Button[] m_StartLoadButton;

    void Awake()
    {
        ManagerResolver.Register<UISceneController>(this);
    }

    // Use this for initialization
    void Start ()
    {
        m_LoadingUI = transform.Find("Panelloading").gameObject;
        m_MainMenuUI = transform.Find("Panelmain").gameObject;

        if (m_StartLoadButton != null)
        {
            for (int i = 0; i < m_StartLoadButton.Length; i++)
            {
                if (m_StartLoadButton[i] != null)
                    MyPointEvent.AutoAddListener(m_StartLoadButton[i], StartLoadEvent, null);
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
		
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

}
