using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.AI.MovementFloor.Base;
namespace Assets.AI.MovementFloor
{
    //追逐
    [RequireComponent(typeof(Vehicle))]
    class SteeringForPursuit:Steering
    {
        public GameObject target;
        private Vector3 desiredVelocity;
        private Vehicle m_vehicle;
        private float maxSpeed;
        private bool isPlaner;
        private void Start()
        {
            m_vehicle = GetComponent<Vehicle>();
            maxSpeed = m_vehicle.maxSpeed;
            isPlaner = m_vehicle.isPlanar;
        }
        public override Vector3 Force()
        {
            Vector3 toTarget = target.transform.position - transform.position;
            //计算追逐者的向前与逃避者向前之间的夹角
            float relativeDirection = Vector3.Dot(transform.forward, target.transform.forward);
            //操纵向量
            Vector3 returnForce;
            //如果夹角大于0，且追逐者基本面对着逃避者，那么直接向逃避者当前位置移动
            //Vector3.Dot()点积 可以判断两个向量的夹角 并且只有当追逐者的朝向和被追逐者的反向夹角的cos值在-0.95~-1时才算面对着
            if ((Vector3.Dot(toTarget,transform.forward)>0)&&(relativeDirection<-0.95f))
            {
                //计算预期速度
                desiredVelocity = (target.transform.position - transform.position).normalized * maxSpeed;
                //返回操纵向量
                returnForce = desiredVelocity - m_vehicle.velocity;
            }
            //计算预期时间，正比于追逐者与逃避者的距离，反比与追逐者和逃避者的速度和
            float lookheadTime = toTarget.magnitude / (m_vehicle.velocity.magnitude + target.GetComponent<Vehicle>().velocity.magnitude);
            //计算预期速度
            desiredVelocity = (target.transform.position + target.GetComponent<Vehicle>().velocity * lookheadTime - transform.position).normalized * maxSpeed;
            returnForce = desiredVelocity - m_vehicle.velocity;
            if (isPlaner)
                returnForce.y = 0;
            return returnForce;
        }
    }
}
