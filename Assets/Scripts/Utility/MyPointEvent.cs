using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class MyPointEvent : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IScrollHandler/*, IUpdateSelectedHandler*/, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler
{

    public delegate void CallBack(UIBehaviour ui, EventTriggerType eventtype, object message, byte count);

    class EventData 
    {
        public UIBehaviour uiBehaviour;
        public object message;
        public CallBack funCallBack;
        public bool checkCount = false;
        public byte count = 0;
    }

    private Dictionary<EventTriggerType, EventData> Event_Data_Dict = new Dictionary<EventTriggerType, EventData>();

    private static MyPointEvent AutoGetPointEvent(GameObject obj)
    {
        MyPointEvent trigger = obj.GetComponent<MyPointEvent>();
        if (trigger==null)
        {
            trigger = obj.AddComponent<MyPointEvent>();
        }
        return trigger;
    }

    public static void AutoAddListener(UIBehaviour obj, CallBack callback, object param, EventTriggerType eventType = EventTriggerType.PointerClick, bool checkCount = false)
    {
        MyPointEvent trigger = AutoGetPointEvent(obj.gameObject);
        trigger.AddListener(obj, callback,param, eventType, checkCount);
    }

    public void AddListener(UIBehaviour ui, CallBack callback, object param, EventTriggerType eventType, bool checkCount)
    {

        if (!Event_Data_Dict.ContainsKey(eventType))
        {
            EventData data = new EventData();
            data.uiBehaviour = ui;
            data.funCallBack = callback;
            data.message = param;
            data.checkCount = checkCount;
            Event_Data_Dict.Add(eventType, data);
        }
        else
        {
            Event_Data_Dict[eventType].uiBehaviour = ui;
            Event_Data_Dict[eventType].funCallBack = callback;
            Event_Data_Dict[eventType].message = param;
            Event_Data_Dict[eventType].checkCount = checkCount;
        }

    }

    private void ExcuteCallBack(EventTriggerType type)
    {
        if(Event_Data_Dict.ContainsKey(type))
        {
            EventData data = Event_Data_Dict[type];

            if (data.uiBehaviour != null && data.uiBehaviour.enabled && data.funCallBack != null)
            {
                if(!data.checkCount)
                    data.funCallBack(data.uiBehaviour, type, data.message, data.count);
                else
                {
                    data.count++;

                    if (data.count == 1)
                        StartCoroutine(CheckCount(data, type));
                }
            }
        }
    }

    System.Collections.IEnumerator CheckCount(EventData data, EventTriggerType type)
    {
        data.funCallBack(data.uiBehaviour, type, data.message, data.count);
        
        yield return new WaitForSeconds(0.2f);

        if(data.count > 1)
            data.funCallBack(data.uiBehaviour, type, data.message, data.count);

        data.count = 0;
    }


    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.PointerEnter);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.PointerExit);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.Drag);
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.Drop);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.PointerDown);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.PointerUp);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.PointerClick);
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.Select);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.Deselect);
    }

    public virtual void OnScroll(PointerEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.Scroll);
    }

    public virtual void OnMove(AxisEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.Move);
    }

    //public virtual void OnUpdateSelected(BaseEventData eventData)
    //{
    //    ExcuteCallBack(EventTriggerType.UpdateSelected);
    //}

    public virtual void OnInitializePotentialDrag(PointerEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.InitializePotentialDrag);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.BeginDrag);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.EndDrag);
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.Submit);
    }

    public virtual void OnCancel(BaseEventData eventData)
    {
        ExcuteCallBack(EventTriggerType.Cancel);
    }
}
