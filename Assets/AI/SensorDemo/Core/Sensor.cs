using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace SensorDemo.Core
{
    /// <summary>
    /// 感知器
    /// </summary>
    public class Sensor:MonoBehaviour
    {
        protected TriggerSystemManager manager;
        //感知器类型
        public enum SensorType
        {
            sight,
            sound,
            health
        }

        public SensorType sensorType;
        private void Awake()
        {
            //查找管理器并保存
            manager = FindObjectOfType<TriggerSystemManager>();
        }
        //感知触发器
        public virtual void Notify(Trigger trigger)
        {

        }

    }
}
