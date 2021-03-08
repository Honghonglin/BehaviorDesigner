using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SensorDemo.Core;
namespace SensorDemo
{
    public class SightTrigger:Trigger
    {
        public override void Try(Sensor s)
        {
            //如果感知器能感觉到这个触发器，那么向感知器发出通知，感知体做出相应的决策或行为
            if (isTouchTrigger(s))
            {
                s.Notify(this);
            }
        }

        //判断感知器能否感知到这个触发器
        protected override bool isTouchTrigger(Sensor sensor)
        {
            GameObject g = sensor.gameObject;
            //如果这个感知器能够感知到视觉信息
            if(sensor.sensorType==Sensor.SensorType.sight)
            {
                RaycastHit hit;
                Vector3 rayDirection = transform.position - g.transform.position;
                rayDirection.y = 0;
                //判断感知体的向前放与物体所在方向的夹角是否在视域范围内
                if((Vector3.Angle(rayDirection,g.transform.forward))<(sensor as SightSensor).fieldOfView)
                {
                    //在视线距离内是否存在其他障碍无遮挡，如果没有障碍物，则返回true
                    if(Physics.Raycast(g.transform.position+new Vector3(0,1,0),rayDirection,out hit,(sensor as SightSensor).viewDistance))
                    {
                        if(hit.collider.gameObject==gameObject)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        //更新触发器的内部信息，由于带有视觉触发器的AI角色可能是运动的，因此不要停止更新这个触发器位置
        public override void Updateme()
        {
            position = transform.position;
        }

        protected override void Start()
        {
            base.Start();
            //向管理器注册这个触发器，管理器会把它加入当前触发器列表中
            manager.ResgisterTrigger(this);
        }
    }
}
