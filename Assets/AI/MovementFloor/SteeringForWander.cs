using UnityEngine;
using Assets.AI.MovementFloor.Base;
namespace Assets.AI.MovementFloor
{
    //需要注意,这个函数的效果与频率相关
    //徘徊
    class SteeringForWander:Steering
    {
        [Header("特殊属性")]
        [Tooltip("徘徊半径，即Wander圈的半径")]
        public float wanderRadius;
        [Tooltip("徘徊距离, 即Wander圈凸处在AI角色前面的距离")]
        public float wanderDistance;
        [Tooltip("每秒加到目标的随机唯一的最大值")]
        public float wanderJitter;
        [Space(50f)]
        public bool isPlanar;
        private Vector3 desireVelocity;
        private Vehicle m_vehicle;
        private float maxSpeed;
        private Vector3 circleTarget;
        private Vector3 wanderTarget;
        private void Start()
        {
            m_vehicle = GetComponent<Vehicle>();
            maxSpeed = m_vehicle.maxSpeed;
            isPlanar = m_vehicle.isPlanar;
            //选取圆圈上的一个点作为初始点
            circleTarget = new Vector3(wanderRadius * 0.707f, 0, wanderRadius * 0.707f);
        }
        public override Vector3 Force()
        {
            //计算随机位移
            Vector3 randomDisplacement = new Vector3((UnityEngine.Random.value - 0.5f) * 2, (UnityEngine.Random.value - 0.5f) * 2, (UnityEngine.Random.value - 0.5f) * 2)*wanderJitter;
            if (isPlanar)
            {
                randomDisplacement.y = 0;
            }
            //随机唯一加到初始点上，得到新的位置
            circleTarget += randomDisplacement;
            //由于新位置很可能不在圆周上，因此需要投影到圆周上;
            circleTarget = circleTarget.normalized * wanderRadius;
            //之前计算处的值是相对与AI角色和AI角色的向前方向的,需要转换为世界坐标
            //计算出目标的坐标
            wanderTarget = m_vehicle.velocity.normalized * wanderDistance/*Wander距离坐标*/ + circleTarget + transform.position;
            //计算预期速度,返回操纵向量
            Vector3 returnvelocity = (wanderTarget - transform.position).normalized * maxSpeed;
            return returnvelocity;
        }
    }
}
