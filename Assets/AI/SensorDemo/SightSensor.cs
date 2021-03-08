using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SensorDemo.Core;
using SensorDemo;

namespace SensorDemo
{
    //感知视觉信息，在触发器中处理
    public class SightSensor:Sensor
    {
        //定义这个AI角色的视域范围
        public float fieldOfView = 45;
        //定义这个AI角色最远能看到的距离
        public float viewDistance = 100f;
        //这个是一个自己写的Controller 可以自动寻路
        //private AIController controller;
        private AIController1 controller;

        //黑板对象
        private Blackboard bb;

        /// <summary>
        /// 记忆对象
        /// </summary>
        private SenseMemory memoryScript;
        private void Start()
        {
            //controller = GetComponent<AIController>();
            controller = GetComponent<AIController1>();
            //设置感知器类型为视觉类型
            sensorType = SensorType.sight;
            //向管理注册这个感知器，管理器会将它加入当前感知器列表中
            manager.RegisterSensor(this);

            bb = GameObject.FindGameObjectWithTag("Blackbord").GetComponent<Blackboard>();

            memoryScript = GetComponent<SenseMemory>();
        }

        public override void Notify(Trigger trigger)
        {
            //当感知器能够真正感觉到某个触发器的信息时被调用，产生相应的行为或做出某些决策
            //这里打印出相关信息，在感知体和触发器之间画一条红色连线，然后角色走向看到的物体
            print("I see a " + trigger.gameObject.transform.position);
            Debug.DrawLine(transform.position, trigger.transform.position, Color.red);

            //如果看到的是玩家
            if(trigger.tag=="Player")
            {
                //在黑板上记录玩家位置和更新时间
                bb.playerLastPosition = trigger.gameObject.transform.position;
                bb.lastSensedTime = Time.time;
            }


            if(memoryScript!=null)
            {
                //添加到记忆列表中
                memoryScript.AddToList(trigger.gameObject, SensorType.sight);
            }
            //controller.MoveToTarget()
        }
        private void OnDrawGizmos()
        {
            Vector3 frontRayPoint = transform.position + (transform.forward * viewDistance);
            float fieldOfViewinRadians = fieldOfView * 3.14f / 180.0f;
            //最左检测区域
            //转换本地坐标为世界坐标 受位置和缩放的影响
            Vector3 leftRayPoint = transform.TransformPoint(new Vector3(-viewDistance * Mathf.Sin(fieldOfViewinRadians), 0, viewDistance * Mathf.Cos(fieldOfViewinRadians)));
            Vector3 rightRayPoint = transform.TransformPoint(new Vector3(viewDistance * Mathf.Sin(fieldOfViewinRadians), 0, viewDistance * Mathf.Cos(fieldOfViewinRadians)));
            Debug.DrawLine(transform.position + new Vector3(0, 1, 0), frontRayPoint  + new Vector3(0, 1, 0), Color.green);
            Debug.DrawLine(transform.position + new Vector3(0, 1, 0), leftRayPoint + new Vector3(0, 1, 0), Color.red);
            Debug.DrawLine(transform.position + new Vector3(0, 1, 0), rightRayPoint + new Vector3(0, 1, 0), Color.blue);
        }

    }
}
