using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SensorDemo.Core;
namespace SensorDemo
{
    class HealthSensor:Sensor
    {
        private void Start()
        {
            //设置感知器类型
            sensorType = SensorType.health;

            //向管理器注册这个感知器
            manager.RegisterSensor(this);
        }

        public override void Notify(Trigger trigger)
        {
            //做某些处理
        }
    }
}
