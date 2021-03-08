using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine;
namespace Assets.UserGuide
{
    class EventTriggerListener: EventTrigger
    {
        #region 变量
        //带参数是为了方便取得绑定了UI事件的对象  
        public delegate void UIDelegate(GameObject go);
        public event UIDelegate OnEnter;
        public event UIDelegate OnExit;
        public event UIDelegate OnDown;
        public event UIDelegate OnUp;
        public event UIDelegate OnClick;
        public event UIDelegate onInitializePotentialDrag;
        public event UIDelegate onBeginDrag;
        public event UIDelegate onDrag;
        public event UIDelegate onEndDrag;
        public event UIDelegate onDrop;
        public event UIDelegate onScroll;
        public event UIDelegate onUpdateSelected;
        public event UIDelegate onSelect;
        public event UIDelegate onDeselect;
        public event UIDelegate onMove;
        public event UIDelegate onSubmit;
        public event UIDelegate onCancel;
        #endregion

        public static EventTriggerListener GetListener(GameObject go)
        {
            EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
            if (listener == null) listener = go.AddComponent<EventTriggerListener>();
            return listener;
        }

        #region 方法
        /// <summary>
        /// 鼠标进入物体
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter?.Invoke(gameObject);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            OnExit?.Invoke(gameObject);
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            OnDown?.Invoke(gameObject);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            OnUp?.Invoke(gameObject);
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(gameObject);
        }
        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            onInitializePotentialDrag?.Invoke(gameObject);
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDrag?.Invoke(gameObject);
        }
        public override void OnDrag(PointerEventData eventData)
        {
            onDrag?.Invoke(gameObject);
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag?.Invoke(gameObject);
        }
        public override void OnDrop(PointerEventData eventData)
        {
            onDrop?.Invoke(gameObject);
        }
        public override void OnScroll(PointerEventData eventData)
        {
            onScroll?.Invoke(gameObject);
        }
        public override void OnUpdateSelected(BaseEventData eventData)
        {
            onUpdateSelected?.Invoke(gameObject);
        }
        public override void OnSelect(BaseEventData eventData)
        {
            onSelect?.Invoke(gameObject);
        }
        public override void OnDeselect(BaseEventData eventData)
        {
            onDeselect?.Invoke(gameObject);
        }
        public override void OnMove(AxisEventData eventData)
        {
            onMove?.Invoke(gameObject);
        }
        public override void OnSubmit(BaseEventData eventData)
        {
            onSubmit?.Invoke(gameObject);
        }
        public override void OnCancel(BaseEventData eventData)
        {
            onCancel?.Invoke(gameObject);
        }
        public void ClearOnClick()
        {
            Delegate[] delegates = OnClick.GetInvocationList();
            foreach (var @delegate in delegates)
            {
                OnClick -= @delegate as UIDelegate;
            }
        }
        #endregion
    }
}
