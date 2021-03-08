using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.AI.PathFinding
{
    class Destination:MonoBehaviour
    {
        //聚集操控力的权重
        public int coherencyWeight = 1;
        //分离操控力的权重
        public int seperatonWeight=2;
        //小组中各个角色的寻路目标点
        private List<Destination> destinations;
        private float velocity;
        public float Velocity { get { return velocity; } }
        //聚集操控力
        private Vector3 coherency;
        //分离操控力
        private Vector3 separation;
        private Vector3 calculatedForce;
        private Vector3 relativePos;
        private void Start()
        {
            //目标点管理器的实例
            destinations = DestinationManager.Instant.destinations;
        }
        //计算操控力，移动目标点
        public void CalculateForce(Vector3 center)
        {
            //计算所有目标点的中心对当前目标点施加的聚集力
            coherency = center - transform.position;
            separation = Vector3.zero;
            //对于目标点列表中的每一个目标点
            foreach (var destination in destinations)
            {
                //如果不是当前目标点，那么求出它对当前目标点产生的分离(排斥)力
                if(destination!=this)
                {
                    relativePos = transform.position - destination.transform.position;
                    separation += relativePos / (relativePos.sqrMagnitude);
                }
            }
            //求出加权和，得到总的操控向量
            calculatedForce = (coherency * coherencyWeight) + (separation * seperatonWeight);
            calculatedForce.y = 0;
            //移动目标点
            transform.GetComponent<Rigidbody>().velocity = calculatedForce * 20;
            velocity = transform.GetComponent<Rigidbody>().velocity.magnitude;
        }

    }
}
