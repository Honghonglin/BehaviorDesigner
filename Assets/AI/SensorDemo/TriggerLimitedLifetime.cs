using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorDemo.Core;
namespace SensorDemo
{
    /// <summary>
    /// 存在一段时间就消失的触发器 具有特定的生命周期
    /// 例如枪响产生声音
    /// </summary>
    class TriggerLimitedLifetime:Trigger
    {
        //该触发器的持续时间
        protected int lifetime;
        public override void Updateme()
        {
            //持续时间倒计时，如果剩余持续时间小于等于0，那么将它标记为需要移除
            if(--lifetime<=0)
            { 
                toBeRemoved = true;
            }
        }
        protected override void Start()
        {
            base.Start();
        }
    }
}
