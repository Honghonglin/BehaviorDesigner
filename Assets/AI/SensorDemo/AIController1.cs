using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pathfinding;
using UnityEngine;
namespace SensorDemo
{
    [Serializable]
    public class AIController1:AIPath
    {
        public int health;
        public float arriveDistance = 1.0f;
        //巡逻的路点
        public Transform patrolWayPoints;
        //目标点预制体
        public GameObject targetPrefab;
        //可以停止追逐，开始射击的距离
        public float shootingDistance = 7.0f;
        //射击状态重新转换到追逐状态的距离
        public float chasingDistance = 8.0f;
        //现在一般使用Animator了，但是Animation更容易代码控制
        //private Animation ani;
        //黑板对象
        private Blackboard bb;
        //当前路点索引
        private int wayPointIndex = 0;
        //最近感知到玩家的位置
        private Vector3 personalLastSighting;
        //上次的玩家位置
        private Vector3 previousSighting;
        //路点的数组
        private Vector3[] wayPoints;
        //记忆对象
        private SenseMemory memory;

        public enum FSMState
        {
            Patrolling=0,//巡逻状态
            Chasing,//追逐状态
            Shooting,//射击状态
        }


        private FSMState state;

        protected override void Start()
        {
            base.Start();
            health = 30;
            patrolWayPoints = GameObject.Find("Points").transform;
            //ani = GetComponent<Animation>();
            //获得黑板对象
            bb = GameObject.FindGameObjectWithTag("Blackbord").GetComponent<Blackboard>();

            personalLastSighting = bb.resetPosition;
            previousSighting = bb.resetPosition;

            //获得记忆对象
            memory = GetComponent<SenseMemory>();

            
            state = FSMState.Patrolling;

            //保存所有路点的一个数组
            wayPoints = new Vector3[patrolWayPoints.childCount];
            int c = 0;

            //遍历patrolWayPoints的子物体
            foreach (Transform child in patrolWayPoints)
            {
                wayPoints[c] = child.position;
                c++;
            }

            destination = wayPoints[0];
            base.Start();
        }


        protected override void Update()
        {
            //如果玩家的位置发生变化，更新
            if(bb.playerLastPosition!=previousSighting)
            {
                personalLastSighting = bb.playerLastPosition;
            }

            switch (state)
            {
                case FSMState.Patrolling:
                    Patrolling();
                    break;
                case FSMState.Chasing:
                    Chasing();
                    break;
                case FSMState.Shooting:
                    Shooting();
                    break;
                default:
                    break;
            }

            previousSighting = bb.playerLastPosition;

        }


        bool CanSeePlayer()
        {
            //如果玩家还在记忆中，那么认为能“看到”玩家
            if (memory != null)
            {
                return memory.FindInList();
            }
            else
                return false;
        }


        void Shooting()
        {
            print("Shooting");
            state = FSMState.Shooting;
            //ani.Play("StandingFire");

            //如果玩家位置被重置，即每个AI士兵都看不到玩家，那么重新进入巡逻状态
            if (personalLastSighting == bb.resetPosition)
            {
                state=FSMState.Patrolling;
            }
            //如果玩家位置更新，可以再次开始追逐
            //玩家位置更新才检测，否则不检测  这种只能然AI停下来射击  后期改进
            if ((personalLastSighting != previousSighting) && Vector3.Distance(tr.position, personalLastSighting) > chasingDistance)
            {
                Debug.Log("change to chasing again........");
                state = FSMState.Chasing;
            }
        }


        void Chasing()
        {
            print("Chasing");
            state = FSMState.Chasing;
            destination = personalLastSighting;
            if (personalLastSighting == bb.resetPosition)
            {
                state = FSMState.Patrolling;
                destination= wayPoints[wayPointIndex];
            }
            //如果距离玩家很近，并且能看到玩家，则转换为射击状态，否则继续追逐
            else if ((Vector3.Distance(transform.position, destination)) < shootingDistance && CanSeePlayer())
            {
                state = FSMState.Shooting;
            }
            else
                //执行寻路
                base.Update();
            
            //ani.CrossFade("Run");
        }




        void Patrolling()
        {
            print("Patrolling");

            state = FSMState.Patrolling;

            //当靠近目标点为多少时为到达
            if (Vector3.Distance(transform.position, destination) < 3)
            {
                //如果到达最后一个路点，重新从第一个路点开始，否者设置为下一个路点
                if(wayPointIndex==wayPoints.Length-1)
                {
                    wayPointIndex = 0;
                    destination = wayPoints[wayPointIndex];
                }
                else
                {
                    wayPointIndex++;
                    destination = wayPoints[wayPointIndex];
                }
            }
            //执行寻路操作
            base.Update();

            //ani.CrossFade("Walk");

            //如果某个AI士兵看到玩家，进入状态状态
            if (personalLastSighting != bb.resetPosition)
            {
                state = FSMState.Chasing;
            }

        }
    }
}
