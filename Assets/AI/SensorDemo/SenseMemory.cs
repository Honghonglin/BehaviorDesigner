using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SensorDemo.Core;
namespace SensorDemo
{
    //记忆感知
    //让角色具有记忆
    public class SenseMemory:MonoBehaviour
    {
        //已经在列表中？
        private bool alreadyInList = false;
        //记存留时间
        public float memoryTime = 4.0f;
        //记忆列表
        public List<MemoryItem> memoryItems = new List<MemoryItem>();
        //此时需要从记忆列表中删除的项
        private List<MemoryItem> removeList = new List<MemoryItem>();
        //记忆列表中寻找玩家信息
        public bool FindInList()
        {
            foreach (MemoryItem item in memoryItems)
                if (item.g.tag == "Player")
                    return true;
            return false;
        }
        //向记忆列表中添加一个项
        public void AddToList(GameObject g,Sensor.SensorType type)
        {
            alreadyInList = false;
            //如果该项已经在列表中，那么更新最后感知时间等信息
            foreach (MemoryItem memoryItem in memoryItems)
            {
                if (g = memoryItem.g)
                {
                    alreadyInList = true;
                    memoryItem.lastMemoryTime = Time.time;
                    memoryItem.memoryTimeLeft = memoryTime;
                    memoryItem.sensorType = type;
                    break;
                }
            }
            //如果不在列表中，新建记忆项并加入列表中
            if(!alreadyInList)
            {
                MemoryItem newItem = new MemoryItem(g, Time.time, memoryTime, type);
                memoryItems.Add(newItem);
            }
        }

        private void Update()
        {
            removeList.Clear();
            //遍历所有项，找到哪些超时需要“忘记”的项，删除
            foreach (MemoryItem memoryItem in memoryItems)
            {
                memoryItem.memoryTimeLeft -= Time.deltaTime;
                if(memoryItem.memoryTimeLeft<0)
                {
                    removeList.Add(memoryItem);
                }
                else
                {
                    //对于没删的项，画出一条线，表示仍然在记忆中
                    if(memoryItem.g!=null)
                    {
                        Debug.DrawLine(transform.position, memoryItem.g.transform.position, Color.blue);
                    }
                }
            }
            foreach (var item in removeList)
            {
                memoryItems.Remove(item);
            }
        }
    }

    public class MemoryItem
    {
        //感知到的游戏对象
        public GameObject g;
        //最近的感知时间
        public float lastMemoryTime;
        //还能存留在记忆中的时间
        public float memoryTimeLeft;
        //通过哪种方式感知到该对象，视觉为1，听觉为0.66
        public Sensor.SensorType sensorType;
        public MemoryItem(GameObject objectToAdd,float time,float timeLeft,Sensor.SensorType type)
        {
            g = objectToAdd;
            lastMemoryTime = time;
            memoryTimeLeft = time;
            sensorType = type;
        }


    }

}
