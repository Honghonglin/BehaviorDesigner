using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.AI.MovementFloor.Base;
namespace Assets.AI.MovementFloor
{
    //离开
    class SteeringForFlee:Steering
    {
        public GameObject target;
        //设置使AI角色意识到危险并开始逃跑的范围;
        public float fearDistance = 20;
        private Vector3 desireVelocity;
        private Vehicle m_vehicle;
        private float maxSpeed;
        private bool isPlanar;
        private void Start()
        {
            m_vehicle = GetComponent<Vehicle>();
            maxSpeed = m_vehicle.maxSpeed;
            isPlanar = m_vehicle.isPlanar;
        }
        public override Vector3 Force()
        {

            Vector3 tmpPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 tmpTargetPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
            //如果AI角色与目标的距离大于逃跑距离，那么返回向量0
            if (Vector3.Distance(tmpPos, tmpTargetPos) > fearDistance)
                return Vector3.zero;
            //预期速度
            desireVelocity = (transform.position - target.transform.position).normalized * maxSpeed;

            //结果速度方向
            Vector3 returnForce = desireVelocity - m_vehicle.velocity;
            if (isPlanar)
                returnForce.y = 0;
            return returnForce;

            
        }
    }
}
