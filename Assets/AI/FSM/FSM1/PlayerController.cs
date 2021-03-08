using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //private Animation anim;
    private CharacterController controller;
    private Transform _t;

    private float input_x;
    private float input_y;
    public float antiBunny = 0.75f;
    private Vector3 _velocity = Vector3.zero;
    private float _speed = 1;
    private float gravity = 20;

    private float rotateAngle;
    private float targetAngle = 0;
    private float currenAngle;
    private float yVelocity = 0.0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        //anim = GetComponent<Animation>();
        _t = transform;

        currenAngle = targetAngle = HorizontalAngle(transform.forward);
    }
    void Update()
    {
        //鼠标横向移动控制物体横向旋转
        rotateAngle = Input.GetAxis("Mouse X") * Time.deltaTime * 50;
        targetAngle += rotateAngle;

        currenAngle = Mathf.SmoothDampAngle(currenAngle, targetAngle, ref yVelocity, 0.3f);
        transform.rotation = Quaternion.Euler(0, currenAngle, 0);

        //这里控制了如果前一次没有动，忽然动了鼠标，开始时的第一帧会加速
        float input_modifier = (input_x != 0.0f && input_y != 0.0f) ? 0.7071f : 1.0f;

        input_x = Input.GetAxis("Horizontal");
        input_y = Input.GetAxis("Vertical");

        _velocity = new Vector3(input_x * input_modifier, -antiBunny, input_y * input_modifier);
        //转换向量由本地到世界坐标 受缩放和旋转影响
        _velocity = _t.TransformDirection(_velocity) * _speed;

        _velocity.y -= gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);


        //if((input_y>0.01f)||(rotateAngle>0.01f)||(rotateAngle<-0.01f))
        //{
        //    anim.CrossFade("Walk");
        //}

        //if (input_y < -0.01f)
        //    anim.CrossFade("WalkBackwards");

        //if (input_x > 0.01f)
        //    anim.CrossFade("StrafeWalkRight");

        //if (input_x < -0.01f)
        //    anim.CrossFade("StrafeWalkLeft");

        //if(Input.GetButton("Fire1"))
        //{
        //    anim.Play("StandingFire");
        //}
        //else
        //{
        //    anim.CrossFade("Idle");
        //}
    }
    private float HorizontalAngle(Vector3 direction)
    {
        if (direction.x > 0)
        {
            //返回的是弧度 所以要乘上57.29578f
            float num = Mathf.Atan2(direction.x, direction.z) * 57.29578f;
            if (num < 0f)
            {
                //转为全部相对于
                num += 180f;
            }
            return num;
        }
        else if (direction.x < 0)
        {
            //返回的是弧度 所以要乘上57.29578f
            float num = Mathf.Atan2(direction.x, direction.z) * 57.29578f;
            if (num > 0f)
            {
                //转为全部相对于
                num += 180f;
            }
            else
            {
                num += 360f;
            }
            return num;
        }
        else
        {
            if (direction.y > 0)
            {
                return 0;
            }
            else
            {
                return 180f;
            }
        }
    }
}
