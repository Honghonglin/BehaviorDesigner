using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
namespace Assets.AI.MovementFloor.Control
{
    //鸟群生成器
    class PlaceObjects:MonoBehaviour
    {
        [Tooltip("要生成的物体")]
        public GameObject objectToplace;
        [Tooltip("数量")]
        public int count;
        //海鸥的初始位置在一个半径为radius的球体内随机产生
        [Tooltip("这个半径里随机生成")]
        public float radius;
        public bool isPlanar;
        private void Awake()
        {
            Vector3 position = Vector3.zero;
            for (int i = 0; i < count; i++)
            {
                position = transform.position + UnityEngine.Random.insideUnitSphere * radius;
                if (isPlanar)
                    position.y = objectToplace.transform.position.y;
                //实例化海鸥预制
                Instantiate(objectToplace, position, Quaternion.identity,transform);
            }
        }
    }
}
