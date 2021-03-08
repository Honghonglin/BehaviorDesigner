using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace SensorDemo.Core
{
    /// <summary>
    /// 触发管理器
    /// </summary>
    public class TriggerSystemManager:MonoBehaviour
    {
        //初始化当前感知器列表
        List<Sensor> currentSensor = new List<Sensor>();
        //初始化当前触发器列表
        List<Trigger> currentTriggers = new List<Trigger>();
        //记录当前时刻需要被移除的感知器 例如感知体死亡，需要移除感知器时
        List<Sensor> sensorsToRemove;
        //记录当前时刻需要被移除的触发器，例如触发器已过期
        List<Trigger> triggersTopRemove;
        private void Start()
        {
            sensorsToRemove = new List<Sensor>();
            triggersTopRemove = new List<Trigger>();
        }

        /// <summary>
        /// 更新触发器
        /// </summary>
        private void UpdateTriggers()
        {
            //对于当前触发器列表中的每一个触发器t
            foreach (var trigger in currentTriggers)
            {
                //如果t需要被移除
                if(trigger.toBeRemoved)
                {
                    //将t加入需要移除的触发器列表中（这是由于不能再foreach中直接移除 否者会改变list大小导致报错）
                    triggersTopRemove.Add(trigger);
                }
                else
                {
                    //更新触发器内部信息
                    trigger.Updateme();
                }
            }
            foreach (Trigger trigger in triggersTopRemove)
            {
                currentTriggers.Remove(trigger);
            }
        }
        /// <summary>
        /// 触发触发器
        /// </summary>
        private void TryTriggers()
        {
            //对于当前感知器列表中的每一个感知器s
            foreach (var s in currentSensor)
            {
                //如果s所对应的感知体还存在（没有因死亡而被销毁）
                if(s.gameObject!=null)
                {
                    //对于当前触发器列表中的每一个触发器t
                    foreach (var t in currentTriggers)
                    {
                        //检查s是否在t的作用范围内，并且做出相应的响应
                        t.Try(s);
                    }
                }
                else
                {
                    //将感知器s加入到需要移除的感知器列表中
                    sensorsToRemove.Add(s);
                }
            }

            //对于需要移除的感知器列表中的每一个感知器s，从当前感知器列表中移除s
            foreach (var s in sensorsToRemove)
                currentSensor.Remove(s);
        }



        private void Update()
        {
            //更新所有触发器内部状态
            UpdateTriggers();
            //迭代所有感知器和触发器，做出相应的行为
            TryTriggers();
        }

        //用于注册触发器
        public void ResgisterTrigger(Trigger t)
        {
            print("registering trigger:" + t.name);
            //将参数触发器t加入到当前触发器列表中
            currentTriggers.Add(t);
        }

        //用于注册感知器
        public void RegisterSensor(Sensor s)
        {
            print("registering sensor:" + s.name + s.sensorType);
            //将参数感知器加入到当前感知器列表中
            currentSensor.Add(s);
        }

    }
}
