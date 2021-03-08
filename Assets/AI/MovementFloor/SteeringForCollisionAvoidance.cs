using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.AI.MovementFloor.Base;

namespace Assets.AI.MovementFloor
{
    //躲避
    //在多个合力的作用下不一定可以完全躲开
    class SteeringForCollisionAvoidance:Steering
    {
        private bool isPlanar;
        private Vector3 desireVelocity;
        private Vehicle m_vehicle;
        //最大速度
        private float maxSpeed;
        //最大的操纵力
        private float maxForce;
        [Tooltip("避免障碍所产生的操纵力的值,被角色的最大施加力限制")]
        public float avoidanceForce;
        [Tooltip("能向前看到的最大距离")]
        public float MAX_SEE_AHEAD = 2.0f;
        [Tooltip("不用赋值，场景中的所有碰撞体组成的数据，将障碍物的Tag设置为obstacle")]
        [SerializeField]
        [Space(20)]
        private GameObject[] allColliders;
        private void Start()
        {
            m_vehicle = GetComponent<Vehicle>();
            maxSpeed = m_vehicle.maxSpeed;
            maxForce = m_vehicle.maxForce;
            isPlanar = m_vehicle.isPlanar;
            //如果避免障碍所产生的操纵力大于最大操纵力，将它截断到最大操纵力
            if(avoidanceForce>maxForce)
            {
                avoidanceForce = maxForce;
            }
            //存储场景中的所有碰撞体，即Tag为obstacle的那些游戏体
            allColliders = GameObject.FindGameObjectsWithTag("obstacle");
        }

        public override Vector3 Force()
        {
            RaycastHit hit;
            Vector3 force = Vector3.zero;
            //角色的速度
            Vector3 velocity = m_vehicle.velocity;
            //速度单位化
            Vector3 normalizedVelocity = velocity.normalized;
            //计算当前速度和最大速度的比值，范围为0~1,防止物体旋转时看见物体也避开
            float dynamic_length = velocity.magnitude / maxSpeed;
            //画出出一条射线，需要考察与这条射线相交的碰撞体
            Debug.DrawLine(transform.position, transform.position + normalizedVelocity * MAX_SEE_AHEAD * dynamic_length);
            if(Physics.Raycast(transform.position,normalizedVelocity,out hit,MAX_SEE_AHEAD * dynamic_length))
            {
                //如果射线与某个碰撞体相交，表示可能与该碰撞体发生碰撞
                Vector3 ahead = transform.position + normalizedVelocity * MAX_SEE_AHEAD * dynamic_length;
                //计算避免碰撞所需要的操纵力
                force = ahead - hit.collider.transform.position;
                //躲避力，也可是force*=avoidanceForce 会变，我这不会
                force = force.normalized*avoidanceForce;

                if (isPlanar)
                    force.y = 0;

                //将这个碰撞体的颜色变为绿色,其他变为灰色
                foreach (GameObject c in allColliders)
                {
                    //要躲避的碰撞体变为黑色
                    if(hit.collider.gameObject==c)
                    {
                        c.GetComponent<Renderer>().material.color = Color.black;
                    }
                    else
                    {
                        c.GetComponent<Renderer>().material.color = Color.white;
                    }
                }

            }
            else
            {
                //如果向前看的有限范围内，没有发生碰撞的可能
                //将所有碰撞体设为灰色
                foreach (GameObject c in allColliders)
                {
                    c.GetComponent<Renderer>().material.color = Color.white;
                }
            }
            //返回操纵力
            return force;
        }
    }
}
