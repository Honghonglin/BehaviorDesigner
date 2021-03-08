using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.AI.PathFinding
{
    class DestinationManager:MonoBehaviour
    {
        public GameObject destinationObjectToMove;
        public GameObject destinationPrefab;
        //目标管理器实例
        private static DestinationManager instant;
        public static DestinationManager Instant { get { return instant; } }
        //小组成员的所有目标点的列表
        public List<Destination> destinations;
        private List<Boid> boids = new List<Boid>();
        //目标点是否已经处于稳定位置
        private bool destinationAreDoneMoving = false;
        //目标点是否赋值
        private bool destinationAreAssighned = true;
        private Ray ray;
        private RaycastHit hitInfo;
        //目标圆的半径
        public float destCircleRadius = 1;
        //是否生成目标点（是否需要生成目标点，生成目标点实例，并且按照操控力移动到稳定点
        private bool generateDestination = true;
        private Vector3[] offset;
        private void Awake()
        {
            instant = this;
            destinations = new List<Destination>();
            FindBoids();
            offset = new Vector3[13];
            offset[0] = new Vector3(0, 0, 0);
            offset[1] = new Vector3(1, 0, 0);
            offset[2] = new Vector3(0.5f, 0, 0.87f);
            offset[3] = new Vector3(-0.5f, 0, 0.87f);
            offset[4] = new Vector3(-1, 0, 0);
            offset[5] = new Vector3(-0.5f, 0, -0.87f);
            offset[6] = new Vector3(0.5f, 0, -0.87f);
            offset[7] = new Vector3(0.87f, 0, 0.5f);
            offset[8] = new Vector3(0, 0, 1);
            offset[9] = new Vector3(-0.87f, 0, 0.5f);
            offset[10] = new Vector3(-0.87f, 0, -0.5f);
            offset[11] = new Vector3(0, 0, -1f);
            offset[12] = new Vector3(-0.87f, 0, 0.5f);
        }

        //将小队中的成员都需要加入boids列表中
        public void FindBoids()
        {
            boids.Clear();
            Boid[] foundBoids = FindObjectsOfType(typeof(Boid)) as Boid[];
            foreach (var item in foundBoids)
            {
                boids.Add(item);
            }
        }

        //放置各个目标点
        private void placeDestination(Vector3 hitPoint)
        {
            int index = 0;
            float radius = destCircleRadius;
            //对于小队成员列表的每个成员
            foreach (var item in boids)
            {
                //如果需要控制力的方式为它生成目标点
                if(generateDestination)
                {
                    //在指定的位置初始化目标点Prefab
                    GameObject des = Instantiate(destinationPrefab, hitPoint + radius * offset[index++], Quaternion.identity) as GameObject;
                    //将Destination组件加入目标点列表中
                    destinations.Add(des.GetComponent<Destination>());
                    item.target = des;
                }
                else
                {
                    //直接将指定的位置作为目标点赋给这个成员
                    item.target.transform.position = hitPoint + radius * offset[index++];
                }
                //我们只放置了13个元素的数组
                //其中12个可重复用
                //它们分别均匀分布在以单击的目标点为中心的两个圆周上
                //如果小组成员超过12，那么增加圆周半径，继续摆放
                if(index>12)
                {
                    index = 1;
                    radius *= 4;
                }
                item.CurrentMovementStatue = MovementState.MOVING;
            }
            //一旦执行完成生产就设置为false，下一次为true需要重新计算路径（即重新设置目标点)
            destinationAreAssighned = false;
            //生成完成后，这些目标点要移动，到稳定点保证各个目标点分开
            destinationAreDoneMoving = false;
            //是否生成目标点，因为这个函数生成了所以这里置为flase
            generateDestination = false;
            return;
        }


        private void Update()
        {
          if(Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray.origin,ray.direction,out hitInfo))
                {
                    if(hitInfo.collider.gameObject.layer==LayerMask.NameToLayer("Ground"))
                    {
                        //重新设置中心目标点，为小组中成员生成所有目标点
                        placeDestination(hitInfo.point);
                    }
                    return;
                }
            }
          //没有目标点就返回
            if (destinations.Count == 0)
                return;
            Vector3 center = Vector3.zero;
            Vector3 velocity = Vector2.zero;
            //求出所有目标位置的平均值
            foreach (var item in destinations)
            {
                center += item.transform.position;
            }
            Vector3 destinationCenter = center / destinations.Count;
            //如果目标点都已经达到稳定状态，但是还并未计算路径
            if(destinationAreDoneMoving&&!destinationAreAssighned)
            {
                //调用AssignNodes函数，为所有小队成员计算路径
                AssignNodes();
                //已发现计算路径请求
                destinationAreAssighned = true;
                return;
            }
            int destinationStopped = 0;
            //对于目标点列表的每一个目标点
            foreach (Destination item in destinations)
            {
                //调用Destination中的CalculateForce函数计算操控力
                item.CalculateForce(destinationCenter);
                //将目标点的当前速度累加
                velocity += item.GetComponent<Rigidbody>().velocity;
                //如果当前目标点的速度小于一个阀值,那么可以认为它已经基本停止
                //将已经停止的目标点的数量+1;
                if (item.Velocity < 1)
                    destinationStopped++;
            }
            //如果所有目标点的速度小于一个阀值，说明目标点已经到达稳定状态
            Vector3 destinationVelocity = velocity / destinations.Count;
            if (destinationVelocity.magnitude < 1)
                destinationStopped = destinations.Count;
            //如果所有目标点都已经近似停止
            if (destinationStopped==destinations.Count)
            {
                //对于所有目标点，将速度设置为0，使它停止运动，稳定在当前位置
                foreach (var item in destinations)
                {
                    item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                destinationAreDoneMoving = true;
            }


        }


        //调用CalculatePath函数，为小队中的每一个成员计算路径
        private void AssignNodes()
        {
            for (int i = 0; i < boids.Count; i++)
            {
                boids[i].CalculatePath();
            }
        }
    }
}
