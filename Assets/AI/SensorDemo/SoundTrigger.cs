using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorDemo.Core;
using UnityEngine;
namespace SensorDemo
{
    class SoundTrigger:TriggerLimitedLifetime
    {
        //判断感知体是否能听到声音触发器发出的声音，如果能，通知感知器
        public override void Try(Sensor s)
        {
            if (isTouchTrigger(s))
            {
                s.Notify(this);
            }
        }

        //判断感知体是否能听到声音触发器发出的声音
        protected override bool isTouchTrigger(Sensor sensor)
        {
            GameObject g = sensor.gameObject;
            //如果感知器能够感知声音
            if(sensor.sensorType==Sensor.SensorType.sound)
            {
                //如果感知体声音触发器的距离在声音触发器的作用范围内，返回true
                //且在这个感知体的听力范围
                if((Vector3.Distance(transform.position,g.transform.position))< radius&& (Vector3.Distance(transform.position, g.transform.position))<(sensor as SoundSensor).hearingDistance)
                {
                    return true;
                }
            }
            return false;
        }
        protected override void Start()
        {
            //设置该触发器的持续时间
            lifetime = 3;
            //调用基类的Start（）函数
            base.Start();

            //将这个触发器交融与到管理器的触发列表中
            manager.ResgisterTrigger(this);
        }

        /// <summary>
        /// 生成调试球  调试球范围检测感知体  触发范围
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, radius);
        }

    }
}
