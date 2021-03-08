using UnityEngine;
using System.Collections;
namespace Assets.AI.MovementFloor.Control
{
    //生成跟随者脚本
    public class GenerateBotsForFollowLeader : MonoBehaviour
    {
        public GameObject botPrefab;
        public GameObject leader;
        public int botCount;
        //长方形"盒子"定义随机生成的AI的初始位置
        public float minX = 81.29f;
        public float maxX = 91f;
        public float minZ = 324.21f;
        public float maxZ = 345f;
        public float Yvalue = 11f;



        // Use this for initialization
        void Start()
        {
            Vector3 spawnPosition;
            GameObject bot;
            for (int i = 0; i < botCount; i++)
            {
                //随机选择一个生成点，实例化预制体
                spawnPosition = new Vector3(UnityEngine.Random.Range(minX, maxX), Yvalue, UnityEngine.Random.Range(minZ, maxZ));
                bot = Instantiate(botPrefab, spawnPosition, Quaternion.identity, transform) as GameObject;
                //设置领队
                bot.GetComponent<SteeringForLeaderFollowing>().leader = leader;
                //设置躲避者
                bot.GetComponent<SteeringForEvade>().target = leader;
                //一开始先不开启躲避 使用
                bot.GetComponent<SteeringForEvade>().enabled = false;
                //设置躲避开启器的领队
                bot.GetComponent<EvadeController>().leader = leader;
            }
        }
    }

}
