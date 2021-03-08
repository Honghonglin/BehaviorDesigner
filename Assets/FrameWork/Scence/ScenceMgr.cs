using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Assets.FrameWork.Mono;
using UnityEngine;
using Assets.FrameWork.Event;
using System.Collections;

namespace Assets.FrameWork.Scence
{
    class ScenceMgr:BaseManager<ScenceMgr>
    {
        public void LoadScence(string name,UnityAction unityAction)
        {
            SceneManager.LoadScene(name);
        }
        public void LoadScenceAsyn(string name,UnityAction unityAction)
        {
            MonoMgr.GetInstance().StartCoroutine(ReallyLoadScenceAsyn(name,unityAction));
        }

        private IEnumerator ReallyLoadScenceAsyn(string name ,UnityAction unityAction)
        {
            AsyncOperation asyncOperation=SceneManager.LoadSceneAsync(name);
            while (!asyncOperation.isDone)
            {
                EventCenter.GetInstance().EventTriggle("进度条更新", asyncOperation.progress);
                yield return asyncOperation.progress;
            }
            unityAction();
        }
    }
}
