using UnityEngine;
using System.Collections;
using Assets.AI.MovementFloor.Base;
/// <summary>
///队列中避免障碍物
/// </summary>
public class SteeringForCollisionAvoidanceQueue : Steering
{
    private bool isPlanar;
    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    //最大速度
    private float maxSpeed;
    //最大力，限制avoidanceForce
    private float maxForce;
    //分开力
    public float avoidanceForce;
    //角色最远能看到的距离
    public float MAX_SEE_AHEAD;
    //所有障碍
    private GameObject[] allColliders;
    //实际值
    private int layerid;
    //掩码
    private LayerMask layerMask;

    private void Start()
    {
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
        maxForce = m_vehicle.maxForce;
        isPlanar = m_vehicle.isPlanar;
        if(avoidanceForce>maxForce)
        {
            avoidanceForce = maxForce;
        }
        allColliders = GameObject.FindGameObjectsWithTag("obstacle");
        layerid = LayerMask.NameToLayer("obstacle");
        layerMask = 1 << layerid;
    }
    //计算碰撞避免所需的操控力，这里利用掩码，只考虑与场景中其他角色的碰撞
    public override Vector3 Force()
    {
        RaycastHit hit;
        Vector3 force = Vector3.zero;
        Vector3 velocity = m_vehicle.velocity;
        Vector3 normalizedVelocity = velocity.normalized;
        if(Physics.Raycast(transform.position,normalizedVelocity,out hit,MAX_SEE_AHEAD,layerMask))
        {
            Vector3 ahead = transform.position + normalizedVelocity * MAX_SEE_AHEAD;
            //计算偏移力方向
            force = (ahead - hit.collider.transform.position).normalized;
            force *= avoidanceForce;
            if(isPlanar)
            {
                force.y = 0;
            }
        }
        return force;
    }
}
