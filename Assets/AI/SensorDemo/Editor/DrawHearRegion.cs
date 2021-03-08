using UnityEngine;
using System.Collections;
using UnityEditor;
using SensorDemo;

//SoundSensor的调试脚本
[CustomEditor(typeof(SoundSensor))]
public class DrawHearRegion : Editor
{
    private float radius;

    private void OnSceneGUI()
    {
        SoundSensor myTarget = target as SoundSensor;
        radius = myTarget.hearingDistance;

        Handles.color = new Color(0, 0.8f, 0.4f, 0.2f);

        //画出圆形区域 表示听觉区域
        Handles.DrawSolidDisc(myTarget.transform.position, myTarget.transform.up, radius);

        Handles.color = new Color(0, 1, 1, 0.1f);
    }
}
