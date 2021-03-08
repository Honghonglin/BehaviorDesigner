using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Pathfinding;
using System.Collections;

namespace Assets.AI.PathFinding
{
    public enum MovementState:int
    {
        IDLE,
        MOVING,
        ORGANIZING
    }
    class Boid:MonoBehaviour
    {
        public float movementSpeed = 1;
        public GameObject target;
        //当前运动状态
        public MovementState CurrentMovementStatue { get; set; }
        //单元之间彼此吸引的强度，这个值设置得越大，单元之间就越接近，在这个栗子
        //中，寻路功能本身就是趋向于为这些单元寻找相近的路线，因此这个值可以设置为0
        public float coherencyWeight = 0;
        //单元之间彼此分离的强度，这个值设置得越大，单元之间就会越趋向远离
        public float seperationWeight = 1;
        //角色改变朝向的速度；
        public float turnSpeed = 4;
        private Vector3 relativePos;

        //聚集行为中得到的操控向量
        private Vector3 coherency;
        //分离行为中得到的操控向量
        private Vector3 seperation;
        //集群行为产生的总操控力
        private Vector3 boidBehaviorForce;
        //集群的成员列表
        List<Boid> boids;
        private CharacterController controller;
        //雷达扫描区域的半径
        public float radius = 1;
        //雷达每秒进行多少次扫描
        public int pingPerSecond = 10;
        //雷达的扫描频率
        public float PingFrequency
        {
            get
            {
                return (1 / pingPerSecond);
            }
        }

        //雷达扫描时，监视那一层
        public LayerMask radarLayers;
        //neighbor表记录位于雷达扫描半径以内的单元
        private List<Boid> neightbors = new List<Boid>();
        private Collider[] detected;
        //与寻路部分相关的变量
        //处理路径计算的Seeker类
        private Seeker seeker;
        //单元将要跟随的路径
        private Path path;
        //单元跟随路径时的当前waypoint
        private int currentWaypoint = 0;
        //当单位与当前waypoint距离小于nextWayPoint时，可以继续向下一个waypoint移动
        private float nextWayPoint = 1;
        //判断单元是否到达路径终点
        private bool journeyComplete = true;
        //寻路部分传递给CharacterController.Move的向量
        private Vector3 pathDirection;
        //小队中所有单元位置的平均值，即中心
        private Vector3 center;
        //来自BoidBehaviors函数的操控力
        private Vector3 steerForce;
        //FollowPath函数返回向量
        private Vector3 seekForce;
        //考虑到操控力和路径跟随的共同作用，最终传递给CharacterController的向量
        private Vector3 driveForce = Vector3.zero;
        //新的曹翔
        private Vector3 newForward;
        //获得CharacterController和Seeker，并且找到场景中所有Boids，加入boids表中
        //并设置当前移动状态为IDLE；开始雷达扫描器
        private void Start()
        {
            //获得角色控制器组件
            controller = GetComponent<CharacterController>();
            //获得Seeker组件
            seeker = GetComponent<Seeker>();
            Boid[] foundBoid = FindObjectsOfType(typeof(Boid)) as Boid[];
            boids = new List<Boid>();
            //对于场景每一个Boid，将它加入boids列表中
            foreach (var b in foundBoid)
            {
                boids.Add(b);
            }
            //设置当前移动状态
            CurrentMovementStatue = MovementState.ORGANIZING;
            StartCoroutine("StartTick", PingFrequency);
        }
        //等待freq秒，调用RadarScan()函数，扫描附近的邻居
        private IEnumerator StartTick(float freq)
        {
            yield return new WaitForSeconds(freq);
            
        }
        //扫描周围的邻居
        private void RadarScan()
        {
            //清空邻居列表
            neightbors.Clear();
            //检测在半径为radius的球内的所有邻居
            detected = Physics.OverlapSphere(transform.position, radius, radarLayers);
            //对于检测到的每个碰撞体
            foreach (Collider collider in detected)
            {
                //如果它时Boid类型并且不是当前AI角色
                if(collider.GetComponent<Boid>()!=null&&collider.gameObject!=this.gameObject)
                {
                    //加入邻居列表中
                    Boid foundBoid = collider.GetComponentInParent<Boid>() as Boid;
                    neightbors.Add(foundBoid);
                }
            }
            //如果邻居数量为0，且当前不是Movingt和ORGANIZING状态
            if(neightbors.Count==0&&CurrentMovementStatue!=MovementState.MOVING&&CurrentMovementStatue!=MovementState.ORGANIZING)
            {
                //将当前状态设置为IDLE状态
                Debug.LogWarning(CurrentMovementStatue);
                CurrentMovementStatue = MovementState.IDLE;
            }
            StartCoroutine("StartTick", PingFrequency);
        }
        //计算Coherancy和Separeation的合理，并且返回这个合力向量，供CharacterController使用
        public Vector3 BoidBehaviors()
        {
            Vector3 boidBehaviorForce;
            //计算聚集操控力
            coherency = center - transform.position;
            //计算分离操控力
            seperation = Vector3.zero;
            //对于邻居里欸奥中的每一个Boid
            foreach (var neightbor in neightbors)
            {
                //如果不是当前AI角色
                if(neightbor!=this)
                {
                    //求出neightbor引起的排斥力(分离）并累加
                    relativePos = transform.position - neightbor.transform.position;
                    seperation += relativePos / relativePos.sqrMagnitude;
                }
            }
            //总的集群操控力是聚集力与分离力的加权和
            boidBehaviorForce = (coherency * coherencyWeight) + (seperation * seperationWeight);
            boidBehaviorForce.y = 0;
            boidBehaviorForce *= Time.deltaTime;
            return boidBehaviorForce;
        }

        private void Update()
        {
            center = Vector3.zero;
            if(boids.Count>0)
            {
                //对于集群中的每个个体Boid
                foreach (var b in boids)
                {
                    //将位置累加到center中
                    center += b.transform.position;
                }
                center = center / boids.Count;
            }
            //操控行为产生的向量
            steerForce = Vector3.zero;
            //seek寻路产生的向量
            seekForce = Vector3.zero;
            //如果正在路上，没有到终点，且当前运动状态是"MOVING"
            if(!journeyComplete&&CurrentMovementStatue==MovementState.MOVING)
            {
                //调用FollloewPath函数，得到跟随这条路径所需要的"力"
                seekForce = FollowPath();
            }
            else
            {
                //如果已经到达终点，那么将当前运动状态置为ORGANIZING
                if(CurrentMovementStatue!=MovementState.ORGANIZING&&CurrentMovementStatue!=MovementState.IDLE)
                {
                    CurrentMovementStatue = MovementState.ORGANIZING;
                }
            }
            //如果当前运动状态是ORGANIZING
            if(CurrentMovementStatue==MovementState.ORGANIZING)
            {
                //调用BoidBehavior函数，求出操控力
                steerForce = BoidBehaviors();
            }
            //总的移动两为操控力产生的移动两和“路径跟随力”产生的移动量之和
            driveForce = steerForce + seekForce;
            print(steerForce);
            //控制角色移动
            controller.Move(driveForce);
            //如果正在路上还没到达终点，且当前运动状态时MOVING
            if(!journeyComplete&&CurrentMovementStatue==MovementState.MOVING)
            {
                //转向移动方向
                TurnToFaceMovementDirection(driveForce);
            }

        }
        //利用Seeker类，计算从单元的当前位置到目标位置的路径
        public void CalculatePath()
        {
            if(target==null)
            {
                Debug.LogWarning("Target is null.Aborting Pathfinding...");
                return;
            }
            //发出A*寻路请求，当寻路完毕，返回结果会调用OnPathComplete
            seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
            journeyComplete = false ;
        }

        //当完成新路径的计算是，进行路径属性的设置
        public void OnPathComplete(Path p)
        {
            if(p.error)
            {
                Debug.Log("Can't find path!");
                return;
            }
            path = p;
            currentWaypoint = 0;
            //然后将单元状态设置为MOVING
            CurrentMovementStatue = MovementState.MOVING;
        }

        //计算跟随路径所需要的移动量
        public Vector3 FollowPath()
        {
            if(path==null ||currentWaypoint>=path.vectorPath.Count||target==null)
            {
                return Vector3.zero;
            }
            //已到达路径终点
            if(currentWaypoint>=path.vectorPath.Count||Vector3.Distance(transform.position,target.transform.position)<0.2f)
            {
                journeyComplete = true;
                return Vector3.zero;
            }
            
            //计算行走方向以及需要移动的量
            pathDirection = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            pathDirection *= movementSpeed * Time.deltaTime;
            pathDirection.y = 0;
            //当前位置和当前目标点的向量距离
            Vector3 off = (path.vectorPath[currentWaypoint] - transform.position);
            //忽略调y轴偏移
            off.y = 0;
            //如果角色已经很接近当前路径点，那么可向下一个路径点行走
            if (off.magnitude <nextWayPoint)
            {
                currentWaypoint++;
            }
            return pathDirection;
        }
        //转向移动方向
        private void TurnToFaceMovementDirection(Vector3 newVel)
        {
            //如果移动速度大于0，且新的速度方向大于某个阀值（防止抖动）
            if(movementSpeed>0&&newVel.sqrMagnitude>0.00005f)
            {
                float step = turnSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, newVel.normalized, step/*允许最大弧度变化*/, 0.0f/*允许的最大矢量变化*/);
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }

    }
}
