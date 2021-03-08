using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.FrameWork.Resourse;
using UnityEngine.Events;

namespace Assets.FrameWork.SingleInstant
{
    public class PoolData
    {
        public GameObject fatherObj;
        public List<GameObject> poolList;
        public PoolData(GameObject obj,GameObject poolObj)
        {
            fatherObj = new GameObject(obj.name);
            fatherObj.transform.parent = poolObj.transform;
            poolList = new List<GameObject>();
            PushObj(obj);
        }
        public GameObject GetObj()
        {
            GameObject obj ;
            obj=poolList[0];
            poolList.RemoveAt(0);
            obj.SetActive(true);
            obj.transform.parent = null;
            return obj;
        }
        public void PushObj(GameObject obj)
        {
            poolList.Add(obj);
            obj.transform.parent = fatherObj.transform;
            obj.SetActive(false);
        }
    }
    class PoolMgr
    {
        public Dictionary<string, PoolData> PoolDic { get; set; } = new Dictionary<string, PoolData>();
        private GameObject poolobj;
        
        public void Getobj(string name,UnityAction<GameObject> unityAction)
        {
            GameObject obj =null;
            if (PoolDic.ContainsKey(name) && PoolDic[name].poolList.Count>0)
            {
                obj = PoolDic[name].GetObj();
                unityAction(obj);
            }
            else
            {
                ResMrg.GetInstance().LoadAsyn<GameObject>(name,o=> {
                    o.name = name;
                    unityAction(o);
                });
                //obj.name = name;
            }
        }

        public void Pushobj(string name,GameObject obj)
        {
            if (poolobj == null)
                poolobj = new GameObject("Pool");
            if(PoolDic.ContainsKey(name))
            {
                PoolDic[name].PushObj(obj);
            }
            else
            {
                PoolDic.Add(name, new PoolData(obj,poolobj));
            }
        }
        public void Clear()
        {
            poolobj = null;
            PoolDic.Clear();
        }

    }
}
