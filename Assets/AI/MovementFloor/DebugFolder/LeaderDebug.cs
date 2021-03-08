using UnityEngine;
using System.Collections;
using Assets.AI.MovementFloor.Base;
namespace Assets.AI.MovementFloor.DebugFolder
{
    //领队调试脚本
    public class LeaderDebug : MonoBehaviour
    {
        [Tooltip("如果有跟随者在前方，距离虚拟点多大会逃避")]
        public float evadeDistance;
        //领队前方的一个点
        private Vector3 center;
        private Vehicle vehicleScript;
        //虚拟点和领队的距离，越大的话跟随者离得越远
        private float LEADER_BEHIND_DIST;
        // Use this for initialization
        void Start()
        {
            vehicleScript = GetComponent<Vehicle>();
            LEADER_BEHIND_DIST = 2;
        }

        // Update is called once per frame
        void Update()
        {
            //计算虚拟点中心
            center = transform.position + vehicleScript.velocity.normalized * LEADER_BEHIND_DIST;
        }
        private void OnDrawGizmos()
        {
            //画出一个位于领队前方的线框球，如果其他角色进入这个范围内，就需要激发逃避行为
            Gizmos.DrawWireSphere(center, evadeDistance);
        }
    }

}
