using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.AI.MovementFloor.Base
{
    //控制AI角色移动(真正控制
    public class AILocormotion:Vehicle
    {
        //AI角色控制器
        private CharacterController controller;
        //AI角色的Rigidbody
        private Rigidbody theRigidbody;
        //AI角色每次的移动距离
        private Vector3 moveDistance;

        new void  Start()
        {
            //获得角色控制器(如果有的话)
            controller = GetComponent<CharacterController>();
            //获得AI角色的Rigidbody(如果有的话)
            theRigidbody = GetComponent<Rigidbody>();
            moveDistance = Vector3.zero;
            //调用基类的Start()函数进行初始化
            base.Start();
        }

        private void FixedUpdate()
        {
            //计算AI角色的速度
            velocity += acceleration * Time.fixedDeltaTime;
            //限制速度，速度要低于最大速度
            if (velocity.sqrMagnitude > sqrMaxSpeed)
                velocity = velocity.normalized * maxSpeed;
            //计算AI角色的移动距离
            moveDistance = velocity * Time.fixedDeltaTime;
            //如果AI角色在平面上移动，那么将Y置为0
            if (isPlanar)
            {
                velocity.y = 0;
                moveDistance.y = 0;
            }

            //如果已经为AI角色添加了角色控制器，那么利用角色控制器控制移动
            if (controller != null)
            {
                //向某个方向速度
                controller.SimpleMove(velocity);
            }
            //如果角色没有角色控制器，也没有Rigidbody；
            //或者AI角色拥有Rigidbody，但是要由动力学的方式控制它的运动;
            else if (theRigidbody == null || theRigidbody.isKinematic/*是否动力学*/)
                transform.position += moveDistance;
            //用Rigidbody控制AI角色的运动
            else
                theRigidbody.MovePosition(theRigidbody.position + moveDistance);
            if(velocity.sqrMagnitude>0.00001)
            {
                //通过当前朝向与速度方向的插值，计算新的朝向
                Vector3 newForward = Vector3.Slerp(transform.forward, velocity, damping * Time.deltaTime);
                if(isPlanar)
                {
                    newForward.y = 0;
                }
                transform.forward = newForward;
            }
            //显示路径
            Debug.DrawLine(transform.position- moveDistance, transform.position ,Color.red,8);
            //播放行走动画
            //  gameObject.GetComponent<Animation>().Play("walk");
        }
    }
}
