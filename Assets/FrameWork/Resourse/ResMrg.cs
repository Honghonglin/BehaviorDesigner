using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.FrameWork.Mono;
using System.Collections;
using UnityEngine.Events;

namespace Assets.FrameWork.Resourse
{
    class ResMrg:BaseManager<ResMrg>
    {
        //同步
        public T Load<T>(string name)where T:UnityEngine.Object
        {
            T res = Resources.Load<T>(name);
            if (res is GameObject)
                return GameObject.Instantiate(res);
            else//不需要实例化的
                return res;
        }
        //外部无法直接得到加载的资源，必须在委托中做
        public void LoadAsyn<T>(string name,UnityAction<T> unityAction) where T:UnityEngine.Object
        {
            MonoMgr.GetInstance().StartCoroutine(ReallyLoadAsync<T>(name,unityAction));
        }
        private IEnumerator ReallyLoadAsync<T>(string name,UnityAction<T> unityAction)where T:UnityEngine.Object
        {
            ResourceRequest resourceRequest=Resources.LoadAsync<T>(name);
            yield return resourceRequest;
            //和同步类似
            if (resourceRequest.asset is GameObject)
            {
                unityAction(GameObject.Instantiate(resourceRequest.asset) as T);
            }
            else
                unityAction(resourceRequest.asset as T);

        }


    }
}
