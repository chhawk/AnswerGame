using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemGui : MonoBehaviour
{
    public Sprite[] ItemSprite;

    struct ItemData
    {
        public byte m_nId;
        public byte m_nNum;
        public float m_fCDTime;
        public Button m_Button;
        public Image m_CDImg;
        public Text m_CDTimeText;
    }
    ItemData[] m_ItemData;

    void Awake()
    {
        ManagerResolver.Register<ItemGui>(this);
    }

    // Use this for initialization
    void Start ()
    {

    }

    public void OnSetupItem(ItemSetup[] itemSetup)
    {
        if (itemSetup.Length == 0)
            return;

        gameObject.SetActive(false);

        m_ItemData = new ItemData[itemSetup.Length];
        byte index = 0;
        Transform childTfm = null;
        Image img;
        Text t;
        foreach (ItemSetup it in itemSetup)
        {
            ItemData item = new ItemData();
            item.m_nId = it.id;
            item.m_nNum = it.num;
            item.m_fCDTime = 0.0f;
            childTfm = transform.GetChild(index);
            item.m_Button = childTfm.GetComponent<Button>();
            item.m_CDImg = childTfm.Find("CD").GetComponent<Image>();
            item.m_CDTimeText = item.m_CDImg.transform.GetComponentInChildren<Text>();
            m_ItemData[index] = item;

            MyPointEvent.AutoAddListener(item.m_Button, OnItemBtnClick, index);

            childTfm.gameObject.SetActive(true);

            img = childTfm.Find("Image").GetComponent<Image>();
            byte imgId = ItemCfg.ItemDict[item.m_nId].m_nImgIndex;
            if (imgId < ItemSprite.Length)
                img.sprite = ItemSprite[imgId];

            t = childTfm.Find("Text").GetComponent<Text>();
            t.text = ItemCfg.ItemDict[item.m_nId].m_cName;

            t = childTfm.Find("Num").GetComponent<Text>();
            t.text = item.m_nNum.ToString();

            bool enable = (ItemCfg.ItemDict[item.m_nId].m_fCDTime >= 0f);
            item.m_Button.enabled = enable;
            item.m_CDImg.gameObject.SetActive(!enable);

            if (ItemCfg.ItemDict[item.m_nId].m_nRelive > 0)//方便回调UI
            {
                ManagerResolver.Resolve<GameController>().ReliveItem.id = index;
                ManagerResolver.Resolve<GameController>().ReliveItem.num = item.m_nNum;
            }

            index++;
        }
    }

    public void OnItemBtnClick(UIBehaviour ui, EventTriggerType eventtype, object message, byte count)
    {
        byte index = (byte)message;
        byte itemId = m_ItemData[index].m_nId;
        Debug.Log("Item click:" + index + " itemId:" + itemId);

        if (m_ItemData[index].m_nNum <= 0)
            return;

        m_ItemData[index].m_nNum--;

        ManagerResolver.Resolve<GameController>().OnMessage(MsgID.ItemUse, itemId);

        Text t = m_ItemData[index].m_Button.transform.Find("Num").GetComponent<Text>();
        if (t != null)
            t.text = m_ItemData[index].m_nNum.ToString();

        if(m_ItemData[index].m_nNum == 0)
        {
            m_ItemData[index].m_CDImg.gameObject.SetActive(true);
            m_ItemData[index].m_Button.enabled = false;
        }
        else if (ItemCfg.ItemDict[itemId].m_fCDTime > 0f )
        {
            m_ItemData[index].m_fCDTime = ItemCfg.ItemDict[itemId].m_fCDTime;
            m_ItemData[index].m_CDImg.gameObject.SetActive(true);
            m_ItemData[index].m_Button.enabled = false;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        for (int i = 0; i < m_ItemData.Length; i++)
        {
            if (m_ItemData[i].m_fCDTime > 0)
            {
                m_ItemData[i].m_fCDTime -= Time.deltaTime;

                byte id = m_ItemData[i].m_nId;

                if (m_ItemData[i].m_fCDTime <= 0f)
                {
                    m_ItemData[i].m_fCDTime = 0f;
                    m_ItemData[i].m_CDImg.gameObject.SetActive(false);

                    m_ItemData[i].m_Button.enabled = true;
                }
                else
                {
                    m_ItemData[i].m_CDImg.fillAmount = m_ItemData[i].m_fCDTime / ItemCfg.ItemDict[id].m_fCDTime;
                    int cdTime = (int)Mathf.Ceil(m_ItemData[i].m_fCDTime);
                    if (cdTime > 60)
                    {
                        cdTime /= 60;
                        m_ItemData[i].m_CDTimeText.text = cdTime.ToString() + "M";
                    }
                    else
                        m_ItemData[i].m_CDTimeText.text = cdTime.ToString() + "S";
                }
            }
        }
    }

    public void OnChangePlayer(bool isLocal)
    {
        gameObject.SetActive(isLocal);
    }
}
