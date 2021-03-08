using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.FrameWork.Mono;
using UnityEngine;
using Assets.FrameWork.Event;
namespace Assets.FrameWork.InputFold
{
    
    class InputMrg:BaseManager<InputMrg>
    {
        public bool IsStart { get; set; } = false;
        public InputMrg()
        {
            MonoMgr.GetInstance().AddUpdateListener(Update);
        }
        public void StartOrEndCheck(bool isOpen)
        {
            IsStart = isOpen;
        }
        private void CallBack(KeyCode keyCode)
        {
            if (Input.GetKeyDown(keyCode))
                EventCenter.GetInstance().EventTriggle("某键按下",keyCode);
            if (Input.GetKeyUp(keyCode))
                EventCenter.GetInstance().EventTriggle("某键抬起", keyCode);
        }

        private void Update()
        {
            if (!IsStart)
                return;
            CallBack(KeyCode.W);
            CallBack(KeyCode.S);
            CallBack(KeyCode.A);
            CallBack(KeyCode.D);   
        }
        //改键功能
    }
}
