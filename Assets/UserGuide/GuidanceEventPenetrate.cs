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
    /// 事件渗透
    /// </summary>
    class GuidanceEventPenetrate :MonoBehaviour,ICanvasRaycastFilter
    {
        private Image targetImage;
        //设置目标Image
        public void SetTargetImage(Image target)
        {
            targetImage = target;
        }
        //是否在范围内，在返回false
        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            if (targetImage == null)
            {
                return true;
            }
            //sp是否在targetImage范围
            return !RectTransformUtility.RectangleContainsScreenPoint(targetImage.rectTransform, sp, eventCamera);
        }
    }
}
