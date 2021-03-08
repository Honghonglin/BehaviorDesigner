using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace SensorDemo.Core
{
    /// <summary>
    /// 触发器
    /// </summary>
    public class Trigger:MonoBehaviour
    {
        //保存管理中心对象
        protected TriggerSystemManager manager;
        //触发器的位置
        protected Vector3 position;
        //触发器的半径
        public int radius;
        //当前触发器是否需要被移除
        public bool toBeRemoved;
        //这个方法检查作为参数的感知器s是否在触发器的作用范围内(或当前触发器是否能正在被感知器s感觉到)，
        //如果是，那么采取相应的行为，这个方法需要在派生类中实现
        public virtual void Try(Sensor s)
        {

        }
        //这个方法触发更新触发器的内部状态，例如，声音触发器的剩余有效时间
        public virtual void Updateme() { }

        //这个方法检查感知器s是否在触发器的作用范围内(或当前触发器是否能真正被感知器s感觉到)，如果是，返回true，如果不是，返回false，它被Try（）调用
        //需要在派生类中实现
        protected virtual bool isTouchTrigger(Sensor sensor)
        {
            return false;
        }

        private void Awake()
        {
            //查找管理器并保存
            manager = FindObjectOfType<TriggerSystemManager>();
        }

        protected virtual void Start()
        {
            //这时不需要被移除，置为false
            toBeRemoved = false;
        }

    }
}
