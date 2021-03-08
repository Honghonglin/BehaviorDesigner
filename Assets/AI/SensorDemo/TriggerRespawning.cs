using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorDemo.Core;
namespace SensorDemo
{
    /// <summary>
    /// 对于被实体触发后在一定时间内保持非活动状态的触发器  比如武器和血包  
    /// 被捡起后在一段时间内处于非活动状态 之后重新变为活动状态  王者荣耀血包
    /// </summary>
    class TriggerRespawning:Trigger
    {
        //两次活跃之间的间隔
        protected int numUpdateBetweenRespawns;

        //距离下次再生还需要等待的时间
        protected int numUpdatesRemainingUntilRespawn;

        //当前是否是活动状态
        protected bool isActive;

        //设置isActive为活动状态
        protected void SetActive()
        {
            isActive = true;
        }

        //设置isActive为非活动状态
        protected void SetInactive()
        {
            isActive = false;
        }

        //将触发器设置为非活动状态
        protected void Deactivate()
        {
            //设置isActive变量为非活动的
            SetInactive();

            //重置剩余时间变量为两次活动之间的间隔时间
            numUpdatesRemainingUntilRespawn = numUpdateBetweenRespawns;
        }


        public override void Updateme()
        {
            //倒计时，如果剩余变为活动时间小于等于0，且目前是非活动的
            if ((--numUpdatesRemainingUntilRespawn <= 0) && !isActive)
            {
                //将触发器设置为活动状态
                SetActive();
            }
        }

        protected override void Start()
        {
            //当前是活动的
            isActive = true;
            base.Start();
        }
    }
}
