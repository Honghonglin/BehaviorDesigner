using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
namespace Assets.FrameWork.Event
{
    class EventCenter:BaseManager<EventCenter>
    {
        public Dictionary<string, UnityAction<object>> EvevtDic { get; private set; } = new Dictionary<string, UnityAction<object>>();

        public void AddEventLisener(string name,UnityAction<object> unityAction)
        {
            if(EvevtDic.ContainsKey(name))
            {
                EvevtDic[name] += unityAction;
            }
            else
            {
                EvevtDic.Add(name, unityAction);
            }
        }
        public void RemoveEventListener(string name,UnityAction<object> unityAction)
        {
            if (EvevtDic.ContainsKey(name))
                EvevtDic[name] -= unityAction;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">事件名</param>
        /// <param name="obj">触发者</param>
        public void EventTriggle(string name,object obj)
        {
            if(EvevtDic.ContainsKey(name))
            {
                EvevtDic[name].Invoke(obj);
            }
        }
        public void Clear()
        {
            EvevtDic.Clear();
        }
    }
}
