using UnityEngine;
using System.Collections;
using Assets.AI.MovementFloor.Base;
/// <summary>
/// 排队
/// </summary>
public class SteeringForQueue :Steering
{
    [Tooltip("前方虚拟点距离")]
    public float MAX_QUEUE_AHEAD;
    [Tooltip("虚拟点圆半径")]
    public float MAX_QUEUE_RADIUS;
    //在范围中的AI
    private Collider[] colliders;
    public LayerMask layersChecked;
    //得到AI的运动属性类
    private Vehicle m_vehicle;
    //layermask的实际值
    private int layerid;
    //转化为掩码
    private LayerMask layerMask;

    private void Start()
    {
        m_vehicle = GetComponent<Vehicle>();
        //设置碰撞检测时的掩码
        //得到一个int值表示第几个layer
        layerid = LayerMask.NameToLayer("vehicles");
        //向左移  如果移一位，那就是10 那第一层就被命中
        layerMask = 1 << layerid;
    }

    public override Vector3 Force()
    {
        Vector3 velocity = m_vehicle.velocity;
        //速度单位化
        Vector3 normalizedVelocity=velocity.normalized;
        //计算出角色的前方一点
        Vector3 ahead = transform.position + normalizedVelocity * MAX_QUEUE_AHEAD;
        //如果以ahead点为中心，MAX_QUEUE_AHEAD的球体内有其他角色
        //并且只检测vehicles层 
        colliders = Physics.OverlapSphere(ahead, MAX_QUEUE_RADIUS, layerMask);
        if(colliders.Length>0)
        {
            //对于使用位于这个球体内的其他角色，如果他们的速度任意一个比当前角色的速度更慢
            //当前角色放慢速度，避免发生碰撞
            foreach (Collider collider in colliders)
            {
                if((collider.gameObject!=gameObject)&&(collider.gameObject.GetComponent<Vehicle>().velocity.magnitude<velocity.magnitude))
                {
                    //降低速度
                    m_vehicle.velocity *= 0.5f;
                    break;
                }
            }
        }
        return Vector3.zero;
    }

}
