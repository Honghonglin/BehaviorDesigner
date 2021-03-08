using UnityEngine;
using System.Collections;
namespace NFSM
{
    public class FSM : MonoBehaviour
    {
        //玩家的Tarnsform组件
        protected Transform playerTransform;

        //下一个巡逻点或玩家的位置，取决于当前的状态
        protected Vector3 destPos;

        //巡逻点的数组
        protected GameObject[] pointList;

        //子弹设计速率
        protected float shootRate;
        //距离上一次设计的时间
        protected float elapsedTime;

        protected virtual void Initialize() { }
        protected virtual void FSMUpadte() { }
        protected virtual void FSMFiredUpdate() { }
        private void Start()
        {
            //用于FSM初始化
            Initialize();
        }
        private void Update()
        {
            //每帧更新FSM
            FSMUpadte();
        }

        private void FixedUpdate()
        {
            //以固定的时间周期更新FSM
            FSMFiredUpdate();
        }

    }
}

