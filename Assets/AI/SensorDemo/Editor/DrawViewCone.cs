using UnityEngine;
using UnityEditor;
using SensorDemo;
[CustomEditor(typeof(SightSensor))]
public class DrawViewCone : Editor
{
    private float viewDistance;
    private float fieldOfView;

    private void OnSceneGUI()
    {
        SightSensor myTarget = (SightSensor)target;

        viewDistance = myTarget.viewDistance;
        fieldOfView = myTarget.fieldOfView;

        //弧度
        float fieldOfViewinRadians = fieldOfView * 3.14f / 180.0f;
        Vector3 leftRayPoint = myTarget.transform.TransformPoint(new Vector3(-viewDistance * Mathf.Sin(fieldOfViewinRadians), 0, viewDistance * Mathf.Cos(fieldOfViewinRadians)));

        Vector3 from = leftRayPoint - myTarget.transform.position;
        //设置处理颜色
        Handles.color = new Color(0, 1, 1, 0.2f);

        //画出扇形区域 表示视觉区域
        Handles.DrawSolidArc(myTarget.transform.position, myTarget.transform.up/*旋转中心*/, from, fieldOfView * 2, viewDistance);

        Handles.color = new Color(0, 1, 1, 0.1f);
    }

}