using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UserGuide
{
    /// <summary>
    /// 新手引导管理
    /// </summary>
    class GuideManagers : MonoBehaviour
    {
        /// <summary>
        /// 引导步骤数组（如：第一步-》第二步。。。。）
        /// </summary>
        public List<GuideUIItem> guideList = new List<GuideUIItem>();
        /// <summary>
        /// 当前数组索引
        /// </summary>
        private int currentIndex = 0;
        /// <summary>
        /// 是否完成所有的新手引导
        /// </summary>
        private bool isFinish = false;
        /// <summary>
        /// 遮罩对象
        /// </summary>
        private GameObject maskPrefabs;

        private void Start()
        {
            Next();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Next()
        {
            if (isFinish || currentIndex > guideList.Count)
            {
                return;
            }
            //注销上一步按钮点击事件
            if (currentIndex != 0 && guideList[currentIndex - 1].go.GetComponent<EventTriggerListener>() != null)
            {
                EventTriggerListener.GetListener(guideList[currentIndex - 1].go).ClearOnClick();
            }

            if (maskPrefabs == null)
            {
                maskPrefabs = Instantiate(Resources.Load<GameObject>("CircleGuidance_Panel"), transform);
            }
            //初始化遮罩
            maskPrefabs.GetComponent<CircleGuidance>().Init(guideList[currentIndex].go.GetComponent<Image>()); ;

            currentIndex++;
            //给当前按钮添加点击事件
            if (currentIndex < guideList.Count)
            {
                EventTriggerListener.GetListener(guideList[currentIndex - 1].go).OnClick += (go) =>
                {
                    Next();
                };
            }
            //最后一个按钮点击事件处理
            else if (currentIndex == guideList.Count)
            {
                EventTriggerListener.GetListener(guideList[currentIndex - 1].go).OnClick += (go) =>
                {
                    maskPrefabs.gameObject.SetActive(false);
                    //注销最后一个按钮的点击事件
                    EventTriggerListener.GetListener(guideList[currentIndex - 1].go).ClearOnClick();
                };
                isFinish = true;
            }
        }
    }
    /// <summary>
    /// 引导ui数组
    /// </summary>
    [Serializable]
    public class GuideUIItem
    {
        /// <summary>
        /// 引导步骤对象
        /// </summary>
        public GameObject go;

        public GuideUIItem(GameObject go)
        {
            this.go = go;
        }
    }
}
