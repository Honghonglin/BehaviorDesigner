using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.AI.MovementFloor.Base
{
    //所有操纵行为的基类
    public abstract class Steering :MonoBehaviour
    {
        //每个操纵力的权重
        public float weight = 1;
        private void Start()
        {
            
        }
        private void Update()
        {
            
        }
        //计算操纵力的方法
        public virtual Vector3 Force()
        {
            return new Vector3(0, 0, 0);
        }
    }
}
