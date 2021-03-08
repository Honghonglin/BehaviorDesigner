using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.AI.MovementFloor.Base;
namespace Assets.AI.MovementFloor
{
    //跟随
    class SteeringFollowPath:Steering
    {
        //由节点数组表示的路径
        public GameObject[] waypoints;
        //目标点
        private Transform target;
        //当前的路点
        private int currentNode;
        [Tooltip("与路点的距离小于这个值时，认为已经到达，可以像下一个路点出发;")]
        public float arriveDistance=0;
        private float sqrArriveDistance;
        //路点的数量
        private int numberOfNode;
        //操纵力
        private Vector3 force;
        //预期速度
        private Vector3 desiredVelocity;
        private Vehicle m_vehicle;
        private bool isPlanar;
        private float maxSpeed;

        [Tooltip("当与目标小于这个距离时，开始减速")]
        [Range(0,20)]
        public float slowDownDistance;

        [Tooltip("到结束结点的时候是否回到原点进行循环")]
        public bool isLoop=false;

        private void Start()
        {
            m_vehicle = GetComponent<Vehicle>();
            isPlanar = m_vehicle.isPlanar;
            maxSpeed=m_vehicle.maxSpeed;
            numberOfNode = waypoints.Length;
            //设置当前路点为第0个路点
            currentNode = 0;
            //设置当前路点为目标点
            target = waypoints[currentNode].transform;
            if (numberOfNode == 0)
                Debug.LogWarning("没有给waypoints赋值");
            if (slowDownDistance < arriveDistance)
                Debug.LogWarning("减速距离比达到距离小，检查是否错误");
            sqrArriveDistance = Mathf.Pow(arriveDistance, 2);
        }
        public override Vector3 Force()
        {
            if (numberOfNode == 0)
            {
                Debug.LogWarning("没有给waypoints赋值");
                return Vector3.zero;
            }
            force = Vector3.zero;
            //目标点和当前物体的距离  指向目标点
            Vector3 dist = target.position - transform.position;

            if (isPlanar)
                dist.y = 0;

            
            //如果当前结点已经是路点数组中的最后一个
            if (currentNode==numberOfNode-1)
            {
                //如果与当前路点的距离大于减速距离
                if(dist.magnitude>slowDownDistance)
                {
                    //求出预期速度
                    desiredVelocity = dist.normalized * maxSpeed;
                    //计算操纵向量
                    force = desiredVelocity - m_vehicle.velocity;
                }
                else
                {
                    if(isLoop)
                    {
                        //如果可以开始靠近下一个路点，将下一个路点设置为目标
                        currentNode = 0;
                        target = waypoints[currentNode].transform;
                    }
                    else
                    {
                        //与当前路点的距离小于减速距离
                        desiredVelocity = dist - m_vehicle.velocity;
                        force = desiredVelocity - m_vehicle.velocity;
                    }
                }
            }
            else
            {
                //当前路点不是路点数组中的最后一个，即正走向中间路点
                if(dist.sqrMagnitude<sqrArriveDistance)
                {
                    //如果与当前路点距离的平方小于到达距离的平方
                    //如果可以开始靠近下一个路点，将下一个路点设置为目标
                    currentNode++;
                    target = waypoints[currentNode].transform;
                }
                else
                {
                    //计算预期速度和操纵向量
                    desiredVelocity = dist.normalized * maxSpeed;
                    force = desiredVelocity - m_vehicle.velocity;
                }
            }
            return force;
        }
    }
}
