using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.AI.MovementFloor.Base;
namespace Assets.AI.MovementFloor.Control
{
    //躲避行为控制器
    class EvadeController:MonoBehaviour
    {
        //领队
        [Tooltip("领队，一般会由GenerateBotsForFollowLeader工厂类来赋值")]
        public GameObject leader;
        //领队的操控脚本
        private Vehicle leaderLocomotion;
        //跟随者的操控脚本
        private Vehicle m_vehicle;
        private bool isPlanar;
        //虚拟点
        private Vector3 leaderAhead;
        //在领队的前方多少为虚拟点
        private float LEADER_BEHIND_DIST;
        private Vector3 dist;
        //逃避距离  这里的距离最好比实际的躲避距离大，这样就可以先激活躲避脚本
        public float evadeDistance;
        //逃避局领导平方，用来比较比较好
        private float sqrEvadeDistance;
        //跟随者的逃避脚本
        private SteeringForEvade evadeScript;


        private void Start()
        {
            leaderLocomotion = leader.GetComponent<Vehicle>();
            evadeScript = GetComponent<SteeringForEvade>();
            m_vehicle = GetComponent<Vehicle>();
            isPlanar = m_vehicle.isPlanar;
            LEADER_BEHIND_DIST = 2.0f;
            sqrEvadeDistance = evadeDistance * evadeDistance;
        }

        private void Update()
        {
            //计算领队前方的一个点
            leaderAhead = leader.transform.position + leaderLocomotion.velocity.normalized * LEADER_BEHIND_DIST;
            //计算角色当前位置与领队的前方某点的距离，如果小于某个值，就需要躲避
            dist = transform.position - leaderAhead;
            if (isPlanar)
                dist.y = 0;
            if(dist.sqrMagnitude<sqrEvadeDistance)
            {
                //如果小于躲避距离，激活躲避行为
                evadeScript.enabled = true;
                Debug.DrawLine(transform.position, leader.transform.position);
            }
            else
            {
                //躲避行为处于非激活状态
                evadeScript.enabled = false;
            }
        }

    }
}
