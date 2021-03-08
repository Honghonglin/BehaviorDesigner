using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.AI.MovementFloor.Base;
namespace Assets.AI.MovementFloor
{
    //抵达
    class SteeringForArrive:Steering
    {
        
        [Header("UnKnown")]
        [Tooltip("这两个暂时我不知干嘛")]
        public float arrivalDistance = 0.3f;
        public float characterRadius = 1.2f;
        [Header("Known")]
        private bool isPanar;
        //当与目标小于这个距离的时候，开始减速
        public float slowDownDistance;
        //在范围内时，使用这个参数调节操控力
        [Tooltip("在范围内时这个参数调节操控力")]
        [Range(0,2)]
        public float parama=1;
        //目标
        public GameObject target;
        private Vector3 desireVelocity;
        private Vehicle m_vehicle;
        private float maxSpeed;

        private void Start()
        {
            m_vehicle = GetComponent<Vehicle>();
            maxSpeed = m_vehicle.maxSpeed;
            isPanar = m_vehicle.isPlanar;
        }

        public override Vector3 Force()
        {
            //计算AI角色与目标之间的距离
            Vector3 toTarget = target.transform.position - transform.position;
            //预期速度
            Vector3 desireVelocity;
            //返回的操控向量
            Vector3 returnForce;
            //距离的向量值
            float distance = toTarget.magnitude;
            //如果与目标之间的距离大于所设置的减速半径
            if(distance>slowDownDistance)
            {
                //预期速度是AI角色与目标点之间的距离,全速前进
                desireVelocity = toTarget.normalized * maxSpeed;
                //返回预期速度与当前速度的差
                returnForce = desireVelocity - m_vehicle.velocity;
            }
            else
            {
                //计算预期速度，并返回预期速度与当前速度的差
                desireVelocity = toTarget*parama - m_vehicle.velocity;
                //返回预期速度与当前速度的差
                returnForce = desireVelocity - m_vehicle.velocity;
            }
            if (isPanar)
                returnForce.y = 0;
            return returnForce;
        }
        private void OnDrawGizmos()
        {
            //在目标周围话白色线框球，显示出减速范围
            Gizmos.DrawWireSphere(target.transform.position, slowDownDistance);
        }
    }
}
