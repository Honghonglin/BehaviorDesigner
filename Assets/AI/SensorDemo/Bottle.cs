using UnityEngine;
using System.Collections;
namespace SensorDemo
{
    public class Bottle : MonoBehaviour
    {
        public GameObject collisionPrefab;
        /// <summary>
        /// 玩家或者AI进入触发声音效果
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Ground")
            {
                Instantiate(collisionPrefab, transform.position, Quaternion.identity);

                Destroy(this);
            }
        }
    }

}