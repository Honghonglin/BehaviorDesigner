using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.AI.MovementFloor.Base;
namespace Assets.AI.MovementFloor
{
    //逃避
    class SteeringForEvade:Steering
    {
        public GameObject target;
        private Vector3 desiredVelocity;
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
            Vector3 toTarget = target.transform.position - transform.position;
            //向前预测的时间
            float lookaheadTime = toTarget.magnitude / (m_vehicle.velocity.magnitude + target.GetComponent<Vehicle>().velocity.magnitude);
            //计算预期速度
            desiredVelocity = (transform.position - (target.transform.position + target.GetComponent<Vehicle>().velocity * lookaheadTime)).normalized * maxSpeed;
            //返回操控向量
            Vector3 returnVelocity = desiredVelocity - m_vehicle.velocity;
            if (isPlanar)
                returnVelocity.y = 0;
            return returnVelocity;
        }
    }
}
