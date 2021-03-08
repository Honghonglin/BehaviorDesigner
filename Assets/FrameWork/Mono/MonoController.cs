using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;

namespace Assets.FrameWork.Mono
{
    class MonoController:MonoBehaviour
    {
        public UnityAction UnityAction { get; private set; }
        private void Update()
        {
            UnityAction?.Invoke();
        }
        public void AddUpdateListener(UnityAction unityAction)
        {
            UnityAction += unityAction;
        }
        public void DeleteUpdateListener(UnityAction unityAction)
        {
            UnityAction -= unityAction;
        }
    }
}
