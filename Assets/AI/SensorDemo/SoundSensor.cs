using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SensorDemo.Core;
namespace SensorDemo
{
    public class SoundSensor:Sensor
    {
        //定义感知体的听觉范围
        public float hearingDistance = 30.0f;

        private Blackboard bb;

        private SenseMemory memoryScript;
        

        //private AIController controller;
        private void Start()
        {
            //controller = GetComponent<AIController>();
            //设置感知器类型为声音感知器
            sensorType = SensorType.sound;
            //向管理器注册这个感知器
            manager.RegisterSensor(this);


            bb = GameObject.FindGameObjectWithTag("Blackbord").GetComponent<Blackboard>();

            memoryScript = GetComponent<SenseMemory>();
        }

        private void Update()
        {
            
        }

        public override void Notify(Trigger trigger)
        {
            //当前感知器能够听到触发器的声音时被调用，做出相应行为，这里打印信息，并走向声音的位置
            print("I hear some sound at" + gameObject.transform.position + Time.time);
            //ControllerColliderHit.MoveTarget(trigger.gameObject.transform.position);

            if (memoryScript != null)
            {
                //添加到记忆列表中
                memoryScript.AddToList(trigger.gameObject, SensorType.sound);
            }

            //在黑板上记录玩家位置和更新时间
            bb.playerLastPosition = trigger.gameObject.transform.position;
            bb.lastSensedTime = Time.time;
        }
    }
}
