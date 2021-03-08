using UnityEngine;
using UnityEditor;
using SensorDemo;
[CustomEditor(typeof(SenseMemory))]
public class DrawMemory : Editor
{
    private Vector3 knowPos;
    private float timeStamp;
    private float timeLeft;
    private float knowType;

    private void OnSceneGUI()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.blue;

        SenseMemory myTarget = target as SenseMemory;
        //对于有SenseMemory组件的物体的memoryItems进行遍历
        foreach (MemoryItem memoryItem in myTarget.memoryItems)
        {
            //显示文本框；表示记忆中的信息
            Handles.Label(memoryItem.g.transform.position + Vector3.up, "Position:" + memoryItem.g.transform.position.ToString()
                + "\nSensor Type:" + memoryItem.sensorType.ToString()
                + "\nSense Time Stamp:" + memoryItem.lastMemoryTime.ToString()
                + "\nMemory Time Left:" + memoryItem.memoryTimeLeft.ToString(), style);

            //显示信息
            GUILayout.BeginArea(new Rect(Screen.width - 100, Screen.height - 80, 90, 50));
            GUILayout.EndArea();
        }
    }

}