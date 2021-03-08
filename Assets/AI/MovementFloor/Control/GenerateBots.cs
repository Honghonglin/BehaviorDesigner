using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.AI.MovementFloor.Control
{
    class GenerateBots:MonoBehaviour
    {
        public GameObject botPrefab;
        public int botCount;
        public GameObject target;
        //长方体"盒子"定义了随机生成 AI的初始位置
        public float minX = 81.29f;
        public float maxX = 91f;
        public float minZ = 324.21f;
        public float maxZ = 345f;
        public float Yvalue = 11f;
        private void Start()
        {
            Vector3 spawnPosition;
            GameObject bot;
            for (int i = 0; i < botCount; i++)
            {
                //随机选择一个生成点，实例化预制体
                spawnPosition = new Vector3(UnityEngine.Random.Range(minX, maxX), Yvalue, UnityEngine.Random.Range(minZ, maxZ));
                bot = Instantiate(botPrefab, spawnPosition, Quaternion.identity,transform) as GameObject;
                bot.GetComponent<SteeringForArrive>().target = target;
            }
        }
    }
}
