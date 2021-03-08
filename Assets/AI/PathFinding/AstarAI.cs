using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Pathfinding;
namespace Assets.AI.PathFinding
{
    class AstarAI:MonoBehaviour
    {
        //转向速度
        public float turnspeed = 10;
        //目标位置
        public Vector3 targetPosition;
        //声明一个Seeker类对象
        private Seeker seeker;
        private CharacterController controller;
        //一个Path类对象 表示路径
        public Path path;
        //角色每秒的速度
        public float speed = 100;
        //当角色与一个航点的距离小于这个值时，角色便可转向路径上的下一个航点
        public float nextWaypointDistance = 3;
        //角色正朝其行进的航点
        private int currenWaypoint = 0;
        private void Start()
        {
            //获得对Seeker组件的引用
            seeker = GetComponent<Seeker>();
            controller = GetComponent<CharacterController>();
            //注册回调函数，在Astar Pah完成寻路后调用该函数 完成寻路后就生成一条路径 
            seeker.pathCallback += OnPathComplete;
            //调用StartPath函数，开始到目标的寻路
            seeker.StartPath(transform.position, targetPosition);
        }

        private void FixedUpdate()
        {
            if (path == null)
            {
                return;
            }
            //如果当前路点编号大于这条路径上路点的总数，娜美已经到达路径的终点
            if(currenWaypoint>=path.vectorPath.Count)
            {
                Debug.Log("End of Path Reached");
                return;
            }
            //计算出去往当前路点所需的行走方向和距离，控制角色移动
            Vector3 dir = (path.vectorPath[currenWaypoint] - transform.position).normalized;
            dir *= speed * Time.fixedDeltaTime;
            controller.SimpleMove(dir);
            //角色转向目标
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedTime * turnspeed/100);
            //如果当前位置与当前路点的距离小于一个给定值，可以转向下一个路点
            if(Vector3.Distance(transform.position,path.vectorPath[currenWaypoint])<nextWaypointDistance)
            {
                currenWaypoint++;
                return;
            }
        }
        //当寻路结束后调用这个函数
        public void OnPathComplete(Path p)
        {
            Debug.Log("Find the path" + p.error);
            //如果找到一条路径，那么保存，并把第一个路点设置为当前路点
            if(!p.error)
            {
                path = p;
                currenWaypoint = 0;
            }
        }
       
        private void OnDisable()
        {
            //禁用就移除
            seeker.pathCallback -= OnPathComplete;
        }
    }
}
