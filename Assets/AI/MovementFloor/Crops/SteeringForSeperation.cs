using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.AI.MovementFloor.Base;

namespace Assets.AI.MovementFloor.Crops
{
    //群中邻居保持适当距离----分离
    [RequireComponent(typeof(Radar))]
    class SteeringForSeperation:Steering
    {
        [Tooltip("可接受的距离，小于这个的时候乘以惩罚系数")]
        public float comfortDistance = 1;
        [Tooltip("AI角色与邻居自己距离过近时的惩罚系数")]
        public float multipplierInsideComfortDistance = 2;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            //在目标周围话白色线框球，显示出减速范围
            Gizmos.DrawWireSphere(transform.position, multipplierInsideComfortDistance);
        }
        public override Vector3 Force()
        {
            Vector3 steeringForce = Vector3.zero;
            //遍历这个AI角色的邻居列表中的每个邻居
            foreach (GameObject s in GetComponent<Radar>().neighbors)
            {
                //如果s不是当前AI角色
                if((s!=null)&&(s!=gameObject))
                {
                    //计算当前AI角色与邻居s之间的距离
                    Vector3 NeighbortoObject =transform.position- s.transform.position;
                    float length = NeighbortoObject.magnitude;
                    //计算这个邻居引起的操控力(可以认为是排斥力，大小与距离成反比)
                    steeringForce += NeighbortoObject.normalized / length;
                    //如果二者之间距离小于可接受距离，排斥力再乘以一个额外因子
                    if (length < comfortDistance)
                        steeringForce *= multipplierInsideComfortDistance;
                }
            }
            return steeringForce;
        }
    }
}
