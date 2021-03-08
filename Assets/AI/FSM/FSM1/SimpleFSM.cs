using UnityEngine;
using System.Collections;
using NFSM;
public class SimpleFSM : FSM
{
    //枚举，定义状态机的四种状态，巡逻，追逐，攻击，死亡
    public enum FSMState
    {
        Patrol,
        Chase,
        Attack,
        Dead,
    }

    //定义开始追逐玩家的距离
    public float chaseDistance = 40f;
    //定义开始攻击玩家的距离
    public float attackDistance = 20f;
    //距离巡逻点小于这个值时，认为已经到达巡逻点
    public float arriveDistance = 3.0f;

    //子弹的生成点
    public Transform bulletSpawnPoint;

    private CharacterController controller;
    //private Animation animComponent;

    //AI角色的当前状态
    public FSMState curState;

    //AI角色的速度
    public float walkSpeed = 80;
    public float runSpeed = 160;

    //AI角色的转向速度
    public float curRotSpeed = 6;
    //子弹预制体
    public GameObject Bullet;

    //AI角色是否死亡
    private bool bDead;
    //角色的生命值
    private int health;

    //初始化FSM
    protected override void Initialize()
    {
        base.Initialize();
        //设置FSM的当前状态为巡逻状态
        curState = FSMState.Patrol;


        bDead = false;
        elapsedTime = 0.0f;
        shootRate = 3.0f;
        health = 100;

        //获取巡逻点的集合
        pointList = GameObject.FindGameObjectsWithTag("PatrolPoints");

        //获得角色控制器和动画组件
        controller = GetComponent<CharacterController>();
        //animComponent = GetComponent<Animation>();

        //随机选择一个巡逻点
        FindNextPoint();

        //找到玩家，并保存玩家的Transform组件
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        if(!playerTransform)
        {
            print("Player doesn't exit,please add one player with Tag 'Player'");
        }
    }


    protected override void FSMUpadte()
    {
        base.FSMUpadte();
        //判断当前状态，调用想要的函数进行状态更新
        switch(curState)
        {
            case FSMState.Patrol:
                UpdatePatrolState();
                break;
            case FSMState.Chase:
                UpdateChaseState();
                break;
            case FSMState.Attack:
                UpdateAttackState();
                break;
            case FSMState.Dead:
                UpdateDeadState();
                break;
        }

        elapsedTime += Time.deltaTime;

        //如果生命值小于等于0，那么设置当前状态为死亡
        if (health <= 0)
            curState = FSMState.Dead;
    }

    //更新巡逻状态
    protected void UpdatePatrolState()
    {
        print("Patroling");
        Vector3 dir=transform.position-playerTransform.position;
        Vector3 destdir = transform.position - destPos;
        destdir.y = 0;
        dir.y = 0;
        //如果已到达当前巡逻点，那么寻找下一个巡逻点
        if (destdir.magnitude<=arriveDistance)
        {
            print("Reached to the destination point,calculating the next point");
            FindNextPoint();
        }
        //检查与玩家的距离，如果距离较近，那么转换到追逐状态
        else if(dir.magnitude<=chaseDistance)
        {
            print("Swich to chase state");
            curState = FSMState.Chase;
        }

        //向目标点转向
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //向前移动
        controller.SimpleMove(transform.forward * Time.deltaTime * walkSpeed);

        //播放行走动画
        //animComponent.CrossFade("Walk");
    }

    //更新追逐状态
    protected void UpdateChaseState()
    {
        print("Chasing");
        //将目标位置设置为玩家的位置
        destPos = playerTransform.position;

        //检查与玩家的距离，如果在攻击范围，转换到攻击状态
        //如果玩家离开，那么回到巡逻状态
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist <= attackDistance)
        {
            curState = FSMState.Attack;
        }
        else if (dist >= chaseDistance)
        {
            curState = FSMState.Patrol;
        }

        //向目标点转向
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //向前移动
        controller.SimpleMove(transform.forward * Time.deltaTime * runSpeed);
        //播放奔跑动画
        //animComponent.CrossFade("Run");
    }

    //更新攻击状态
    protected void UpdateAttackState()
    {
        print("Attacking");
        Quaternion targetRotation;

        //设置目标点为玩家位置
        destPos = playerTransform.position;

        //检查与玩家的距离
        float dist = Vector3.Distance(transform.position, playerTransform.position);

        //如果与玩家距离在攻击距离与追逐距离之间，转换为追逐状态
        if(dist>=attackDistance&&dist<chaseDistance)
        {
            curState = FSMState.Chase;
            return;
        }

        //如果玩家离开，回到巡逻状态
        else if (dist >= chaseDistance)
        {
            curState = FSMState.Patrol;
            return;
        }

        //转向目标点
        targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //发射子弹
        ShootBullet();

        //播放射击动画
        //animComponent.CrossFade("StandingFire");
    }
    //当AI角色与子弹发生碰撞时，减少生命值
    private void OnCollisionEnter(Collision collision)
    {
        //判断是敌方子弹
        //if(collision.gameObject.tag=="Bullet"&&)
        //{
        //    health -= collision.gameObject.GetComponent<Bullet>().damage;
        //}
    }


    //寻找下一个巡逻点，随机的从巡逻点数组中选择一个
    protected void FindNextPoint()
    {
        print("Finding next point");
        int rndIndex = Random.Range(0, pointList.Length);
        destPos = pointList[rndIndex].transform.position;
    }

    private void ShootBullet()
    {
        //判断距离上次发射子弹的时间是否大于子弹发生速率，如果大于，可以再次发射
        if (elapsedTime >= shootRate)
        {
            //在bulletSpawnPoint位置生成子弹
            GameObject bulletObj = Instantiate(Bullet, bulletSpawnPoint.transform.position, transform.rotation) as GameObject;
            bulletObj.GetComponent<Bullet>().Go();
            elapsedTime = 0.0F;
        }
    }
    protected void UpdateDeadState()
    {
        //如果bDead还是flase，那么将它置为true，并且播放死亡动画
        if (!bDead)
        {
            bDead = true;
        }
    }
}
