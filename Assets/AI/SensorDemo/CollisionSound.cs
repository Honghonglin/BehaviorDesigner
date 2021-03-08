using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
namespace SensorDemo
{
    /// <summary>
    /// 声音预制体
    /// </summary>
    class CollisionSound:MonoBehaviour
    {
        public AudioClip collisionSound;
        private void Start()
        {
            GetComponent<AudioSource>().PlayOneShot(collisionSound);
        }
    }
}
