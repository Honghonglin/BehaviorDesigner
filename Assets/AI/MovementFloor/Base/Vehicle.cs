using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.AI.MovementFloor.Base
{
    //要求要附加这个脚本就要有Steering
    [RequireComponent(typeof(Steering))]
    //角色控制基类
    public class Vehicle : MonoBehaviour
    {
        //这个AI角色包含的操纵行为
        private Steering[] steerings;
        //设置这个AI角色能达到的最大速度
        public float maxSpeed = 10;
        //设置能施加到这个AI的力的最大值
        public float maxForce = 100;
        //最大速度的平方，通过预先算出并储存
        protected float sqrMaxSpeed;
        //AI角色的质量
        public float mass = 1;
        //AI角色的速度
        public Vector3 velocity;
        //转向时的速度
        public float damping = 0.9f;
        //操纵力的计算间隔时间，为了达到更高的帧率，操纵力不需要每帧都更新
        public float computeInterval = 0.2f;
        //是否在二维平面上，如果是的话，计算两个Gameobject的距离时，忽略y值的不同
        public bool isPlanar = true;
        //计算得到的操纵力
        public Vector3 steeringForce;
        //AI角色的加速度
        protected Vector3 acceleration;
        //计时器
        private float timer;
        protected void Start()
        {
            steeringForce = Vector3.zero;
            sqrMaxSpeed = maxSpeed * maxSpeed;
            timer = 0;

            //即使是没有激活的也会得到
            steerings = GetComponents<Steering>();
        }
        protected void Update()
        {
            timer += Time.deltaTime;

            steeringForce = Vector3.zero;

            //如果距离上次计算操纵力的时间间隔大于computeInternal
            //再次计算
            if(timer>computeInterval)
            {
                //将操纵行为列表中的所有行为对应的操作里进行带权重的求和
                foreach (var s in steerings)
                {
                    if (s.enabled)
                        steeringForce += s.Force() * s.weight;
                }
                //限制操作力的值不大于maxForce
                steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);
                //力除以质量，求出加速度
                acceleration = steeringForce / mass;
                //重新开始计时
                timer = 0;
            }
        }

    }
}