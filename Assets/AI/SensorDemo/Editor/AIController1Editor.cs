using UnityEngine;
using UnityEditor;
using Pathfinding;
using SensorDemo;
public class AIController1Editor : BaseAIEditor
{
    protected override void Inspector()
    {
        base.Inspector();
        var isAIController1 = typeof(AIController1).IsAssignableFrom(target.GetType());
        if(isAIController1)
        {
            PropertyField("patrolWayPoints");
        }
    }
}