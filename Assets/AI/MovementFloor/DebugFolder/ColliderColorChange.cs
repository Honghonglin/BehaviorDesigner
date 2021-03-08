using UnityEngine;
using System.Collections;
using Assets.AI.MovementFloor.Base;

namespace Assets.AI.MovementFloor.DebugFolder
{

    //赋给SteeringForCollisionAvoidanceContain的球体,用来调试
    public class ColliderColorChange : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            //如果和其他碰撞体碰撞，那么碰撞体变为红色
            print("collide0!");
            if (other.gameObject.GetComponent<Vehicle>() != null)
            {
                print("collide!");
                GetComponent<Renderer>().material.color = Color.red;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //碰撞体变为灰色
            GetComponent<Renderer>().material.color = Color.gray;
        }
    }

}
