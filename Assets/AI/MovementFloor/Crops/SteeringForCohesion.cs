using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.AI.MovementFloor.Base;
using UnityEngine;
namespace Assets.AI.MovementFloor.Crops
{
    //聚集 成群聚集在一起
    [RequireComponent(typeof(Radar))]
    class SteeringForCohesion:Steering
    {
        private Vector3 desireVelocity;
        private Vehicle m_vehicle;
        private float maxSpeed;
        private void Start()
        {
            m_vehicle = GetComponent<Vehicle>();
            maxSpeed = m_vehicle.maxSpeed;
        }
        public override Vector3 Force()
        {
            //操控向量
            Vector3 steeringForce = Vector3.zero;
            //AI角色的邻居的所有邻居的质心，即平均位置
            Vector3 centerOfMass = Vector3.zero;
            //AI角色的邻居的数量
            int neighborCount = 0;
            //遍历邻居列表的每一个邻居
            foreach (GameObject s in GetComponent<Radar>().neighbors)
            {
                //如果s不是当前AI角色
                if((s!=null)&&(s!=this.gameObject))
                {
                    //累加s的位置
                    centerOfMass += s.transform.position;
                    //邻居数量+1
                    neighborCount++;
                }
            }
            //如果邻居数目大于0
            if(neighborCount>0)
            {
                //将位置的累加值除以邻居数量，得到平均值
                centerOfMass /= neighborCount;
                //预期速度为邻居位置平均值与当前位置之差
                desireVelocity = (centerOfMass - transform.position).normalized * maxSpeed;
                //预期速度减去当前速度，求出操控向量
                steeringForce = desireVelocity - m_vehicle.velocity;
            }
            return steeringForce;
        }
    }
}
