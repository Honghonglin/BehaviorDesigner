using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Pathfinding.Util
{
    class FireRange:MonoBehaviour
    {
        //玩家的火力范围
        public float fireRange;
        //火力范围内的路点需要增加的代价值
        public int penalty;
        //火力攻击的控制角度
        public float fieldOfAttack = 45;
        //在场景中显示一些火力线
        private void OnDrawGizmos()
        {
            //正前方火力线
            Vector3 frontRayPoint = transform.position + (transform.forward * fireRange);
            float fieldOfAttackinRadians = fieldOfAttack * 3.14f / 180f;
            for (int i = 0; i < 11; i++)
            {
                RaycastHit hit;
                float angel = -fieldOfAttackinRadians + fieldOfAttackinRadians * 0.2f * (float)i;/*将90度分为10份，有11个对应角度*/
                //将本地坐标转换为世界坐标
                Vector3 raypoint = transform.TransformPoint(new Vector3(fireRange * Mathf.Sin(angel), 0, fireRange * Mathf.Cos(angel)));
                Vector3 rayDirection = raypoint - transform.position;
                //当遇到障碍物时，终止火力线
                if(Physics.Raycast(transform.position,rayDirection,out hit,fireRange))
                {
                    if(hit.transform.gameObject.layer==LayerMask.NameToLayer("obstacle")/*这里和Physics.Raycast的layermask设置不一样*/)
                    {
                        //这里增加的Vector3(0,1,0)是因为我们的角色模型的
                        //transform.position点y值为0过低，投射射线时无法与碰撞体相交，因此适当抬高射线的初始点      
                        Debug.DrawLine(transform.position + new Vector3(0, 1, 0), hit.point, Color.red);
                        continue;
                    }
                }
                Debug.DrawLine(transform.position + new Vector3(0, 1, 0), raypoint + new Vector3(0, 1, 0), Color.red);
            }
        }
    }
}
