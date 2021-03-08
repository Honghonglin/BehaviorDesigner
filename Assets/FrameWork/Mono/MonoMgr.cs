using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.FrameWork.Mono
{
    //这样就实现了一个可以执行Update的类
    //只要将其他对象的函数添加入事件
    //可以提供外部的协程方法
    class MonoMgr:BaseManager<MonoMgr>
    {
        public MonoController Controller { get; private set; }
        public MonoMgr()
        {
            //构造唯一的一个MonoController对象
            GameObject obj = new GameObject("MonoController");
            Controller = obj.AddComponent<MonoController>();
        }

        public void AddUpdateListener(UnityAction unityAction)
        {
            Controller.AddUpdateListener(unityAction);
        }

        public void DeleteUpdateListener(UnityAction unityAction)
        {
            Controller.DeleteUpdateListener(unityAction);
        }
        //协程方法1，有多个这里就先不写了
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return Controller.StartCoroutine(routine);
        }

    }
}
