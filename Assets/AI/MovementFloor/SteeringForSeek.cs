using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.AI.MovementFloor.Base;
namespace Assets.AI.MovementFloor
{
    //靠近  会在终点来回转
    [RequireComponent(typeof(Vehicle))]
    class SteeringForSeek:Steering
    {
        //需要寻求的目标物体
        public GameObject target;
        //预期速度
        private Vector3 desiredVelocity;
        //获得被操控AI角色，以便查询测个AI角色的最大速度等信息;
        private Vehicle m_vehicle;
        //最大速度
        private float maxSpeed;
        //是否仅在二维平面上运动
        private bool isPlaner;

        private void Start()
        {
            //获得被操纵AI角色，并读取AI角色运行的最大速度，是否仅在平面上运动
            m_vehicle = GetComponent<Vehicle>();
            maxSpeed = m_vehicle.maxSpeed;
            isPlaner = m_vehicle.isPlanar;
        }
        public override Vector3 Force()
        {
            desiredVelocity = (target.transform.position - transform.position).
                normalized * maxSpeed;
            

            //返回操纵向量，即预期速度与当前速度的差;
            Vector3 returnForce = desiredVelocity - m_vehicle.velocity;

            if (isPlaner)
                returnForce.y = 0;
            return returnForce;
        }

    }
}
