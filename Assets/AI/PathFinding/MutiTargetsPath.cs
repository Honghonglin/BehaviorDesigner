using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Pathfinding;
namespace Assets.AI.PathFinding
{
    /// <summary>
    /// 这个要用Path Finding pro才可以 使用S
    /// </summary>
    class MutiTargetsPath:MonoBehaviour
    {
        public Transform targetPoints;
        private CharacterController controller;
        //一个Path类对象，表示路径
        public Path path;
        //角色每秒的速度
        public float speed = 80;
        public float curRotSpeed = 6.0f;
        //当觉得与一个航点的距离小于这个值时，角色便可转向路径上的下一个航点
        public float nextWaypointDistance = 3;
        //角色正朝其行进的航点
        private int currenWaypoint = 0;

        private void Start()
        {
            //获得Seeker组件
            Seeker seeker = GetComponent<Seeker>();
            controller = GetComponent<CharacterController>();
            //设置路径完成时的回调函数
            seeker.pathCallback = OnPathComplete;
            //设置寻路的目标点数组，即targetPoints的所有子物体的位置
            Vector3[] endPoints = new Vector3[targetPoints.childCount];
            int c = 0;
            foreach (Transform child in targetPoints)
            {
                endPoints[c] = child.position;
                c++;
            }
            //由于这里我们只需要找到最近的路径，所以将最后一个参数选为false
            //seeker.StartMultiTargetPath(transform.position, endPoints, false);
        }

        private void FixedUpdate()
        {
            if (path == null)
            {
                return;
            }
            //如果当前路点编号大于这条路径上路点的总数，娜美已经到达路径的终点
            if (currenWaypoint >= path.vectorPath.Count)
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
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedTime * curRotSpeed);
            //如果当前位置与当前路点的距离小于一个给定值，可以转向下一个路点
            if (Vector3.Distance(transform.position, path.vectorPath[currenWaypoint]) < nextWaypointDistance)
            {
                currenWaypoint++;
                return;
            }
        }

        public void OnPathComplete(Path p)
        {
            Debug.Log("Find the path" + p.error);
            //如果找到一条路径，那么保存，并把第一个路点设置为当前路点
            if (!p.error)
            {
                path = p;
                currenWaypoint = 0;
            }
        }
    }
}
