using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.AI.MovementFloor.Base;
using UnityEngine;
namespace Assets.AI.MovementFloor
{
    [RequireComponent(typeof(SteeringForArrive))]
    class SteeringForLeaderFollowing:Steering
    {
        //虚拟目标点
        private Vector3 target;
        private Vector3 desiredVelocity;
        private Vehicle m_vehicle;
        //最大速度
        private float maxSpeed;
        private bool isPlanar;
        [Tooltip("领队游戏体")]
        public GameObject leader;
        //领队的控制脚本
        private Vehicle leaderController;
        //领队速度
        private Vector3 leaderVelocity;
        //跟随者落后领队的距离
        private float LEADER_BEHIND_DIST = 2.0f;
        private SteeringForArrive steeringForArrive;
        private Vector3 randomOffset;
        private void Start()
        {
            m_vehicle = GetComponent<Vehicle>();
            maxSpeed = m_vehicle.maxSpeed;
            isPlanar = m_vehicle.isPlanar;
            leaderController = leader.GetComponent<Vehicle>();
            //为抵达行为指定目标点
            steeringForArrive = GetComponent<SteeringForArrive>();
            //生成虚拟目标点物体
            steeringForArrive.target = new GameObject("arriveTarget");
            steeringForArrive.target.transform.position = leader.transform.position;
        }
        public override Vector3 Force()
        {
            leaderVelocity = leaderController.velocity;
            //计算虚拟目标点
            target = leader.transform.position + LEADER_BEHIND_DIST * (-leaderVelocity).normalized;
            steeringForArrive.target.transform.position = target;
            //操纵力由其他的脚本来控制
            return Vector3.zero;
        }
    }
}
