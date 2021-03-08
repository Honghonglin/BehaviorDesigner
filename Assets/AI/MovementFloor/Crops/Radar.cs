using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.AI.MovementFloor.Base;
using Assets.AI.MovementFloor.Helper;
namespace Assets.AI.MovementFloor.Crops
{
    //检测附近AI角色
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Radar : MonoBehaviour
    {
        [Tooltip("分片数")]
        public int Segments = 60;         //分割数
                                          //碰撞体的数组
        private Collider[] colliders;
        //计时器
        private float timer = 0;
        //邻居列表
        public List<GameObject> neighbors;
        //无需每帧进行检测，该变量设置检测的时间间隔
        public float checkInterval = 0.3f;

        //设置领域半径
        public float detectRadius = 10f;

        //设置检测哪一层的游戏对象
        public LayerMask layersChecked;

        //角度
        [Tooltip("扇形角度")]
        [Range(0, 360)]
        public float angel;

        private void OnDrawGizmos()
        {
            //设置画的颜色
            Gizmos.color = Color.black;
            //在目标周围画白色线框球，显示出减速范围
            Gizmos.DrawWireSphere(transform.position, detectRadius);
            Gizmos.color = new Color(1, 1, 1, 0.5f);
            Gizmos.DrawMesh(Detector.CreateMesh(detectRadius, 0, angel, Segments), transform.position);
        }

        private void Start()
        {
            //初始化邻居化列表
            neighbors = new List<GameObject>();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            //如果距离上次检测的时间大于所设置的检测时间间隔,那么再次检测
            if (timer > checkInterval)
            {
                //清除邻居列表
                neighbors.Clear();
                //查找当前AI角色领域内的所有碰撞体
                colliders = Physics.OverlapSphere(transform.position, detectRadius, layersChecked);

                List<GameObject> gamelist = new List<GameObject>();
                foreach (var collider in colliders)
                {
                    //扇形检测器
                    if (Detector.Sector(angel, collider.gameObject, gameObject, Segments, detectRadius))
                        gamelist.Add(collider.gameObject);
                }
                //对于每一个检测到的碰撞体，获取Vehicle组件，并且加入邻居列表
                for (int i = 0; i < gamelist.Count; i++)
                {
                    if (gamelist[i].GetComponent<Vehicle>())
                        neighbors.Add(gamelist[i].gameObject);
                }
                //计时器归0
                timer = 0;
            }
        }
    }
}