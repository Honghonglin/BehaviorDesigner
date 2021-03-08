using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SensorDemo.Core;
using System.Collections;

namespace SensorDemo
{
    //血包供给器
    class TriggerHealthGiver:TriggerRespawning
    {
        //设置每次增加的生命值
        public int healthGiven = 10;

        //检测当前触发器是否是活动的，并且感知是否在这个触发器的作用范围内
        public override void Try(Sensor s)
        {
            if(isActive&&isTouchTrigger(s))
            {
                //AIController controller = s.GetComponent<AIController>();
                //if (controller != null)
                //{
                //增加生命值
                //controller.health += healthGiven;
                //显示当前生命值
                //print("now my health is :" + controller.health);
                //将它颜色变为绿色
                GetComponent<MeshRenderer>().material.color = Color.green;
                //调用Coroutine开始计时
                //调用感知器的Notify函数，以便感知体做出相应行动
                StartCoroutine("TurnColorBack");
                s.Notify(this);
                //}
                //else
                //print("Can't get health script!");
            }

            //将这个触发器置为非活动状态
            Deactivate();
        }

        //过了3秒后，生命值供给器变为黑色，表示处于非激活状态
        //事实上，当增加生命值后便立刻变为非激活状态，只是为了更容易观察，才多等3秒再变色
        IEnumerator TurnColorBlack()
        {
            yield return new WaitForSeconds(3);
            GetComponent<MeshRenderer>().material.color = Color.black;
        }


        //检查感知器是否在这个触发器的作用范围内
        protected override bool isTouchTrigger(Sensor sensor)
        {
            GameObject g = sensor.gameObject;
            if(sensor.sensorType==Sensor.SensorType.health)
            {
                //触发器与感知器的距离是否小于触发器的作用半径
                if((Vector3.Distance(transform.position,g.transform.position))<radius)
                {
                    return true;
                }
            }
            return false;
        }


        protected  override void Start()
        {
            //设置两次活动状态之间的间隔时间
            numUpdateBetweenRespawns = 6000;

            //调用基类的Start()函数
            base.Start();

            //向管理器注册这个触发器
            manager.ResgisterTrigger(this);
        }

        //在sence中显示触发半径
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

    }
}
