using UnityEngine;
using System.Collections;
namespace SensorDemo
{
    //共享士兵彼此感知到的信息
    public class Blackboard : MonoBehaviour
    {
        //最近一次感知到玩家时，玩家的位置
        public Vector3 playerLastPosition;
        //当没有感知到玩家时，设置的位置
        public Vector3 resetPosition;
        //上次更新玩家信息的时间
        public float lastSensedTime = 0;
        //重置玩家位置前等待的时间
        public float resetTime = 1.0f;

        // Use this for initialization
        void Start()
        {
            //初始感知到玩家的位置
            playerLastPosition = new Vector3(100, 100, 100);
            resetPosition = new Vector3(100, 100, 100);
        }

        // Update is called once per frame
        void Update()
        {
            //距离上次更新玩家的时间超过resetTime,那么重置玩家位置
            if (Time.time - lastSensedTime > resetTime)
            {
                playerLastPosition = resetPosition;
            }
        }
    }
}