using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //************************************所有字段*****************************************
    public static Player Instance;
    //设置跑力道
    [Range(1, 1000)]
    public float runForce = 20;
    //设置走力道
    public float walkForce;
    //设置跳跃力道
    [Range(0, 100)]
    public float JumpForce = 100;
    //视角控制的速度
    public float mouseSpeed = 100f;

    //因为玩家状态可能叠加，这里用链表存储当前状态，如果链表长度为0默认状态为idel
    private List<PlayerState> nowStates = new List<PlayerState>();
    //方向输入值
    float hValue = 0;
    float vValue = 0;
    //重力组件
    Rigidbody rigidBody;
    //跳跃判定对象
    FootFlag ff;
    //角色零件
    Transform parts;
    //镜头参考
    public Transform cameraTemp;

    //*************************************************************************************

    // 一级初始化
    void Awake()
    {
        // couldjump = false;
        walkForce = runForce / 2;
        parts = transform.Find("Parts");
        cameraTemp = transform.Find("CameraTemp");
        rigidBody = transform.GetComponent<Rigidbody>();
        ff = transform.GetComponentInChildren<FootFlag>();


        //实现单例
        Instance = this;
    }

    // 一级刷新
    void FixedUpdate()
    {


        DoingByPlayerStates(nowStates);
        MouseCamera();
    }

    //二级刷新
    private void Update()
    {
        JudgeNowState();
    }






    //通过玩家的输入，计算当前的状态
    private void JudgeNowState()
    {


        //判断是否触发跳跃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            nowStates.Add(PlayerState.Jump);
            Debug.Log("触发空格");

        }

        //判断是否正在移动
        hValue = Input.GetAxis("Horizontal");
        vValue = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            //判断是否在移动的同事按住了shift,切换跑和走状态
            if (Input.GetKey(KeyCode.LeftShift))
            {
                nowStates.Add(PlayerState.Walk);
            }
            else
            {
                nowStates.Add(PlayerState.Run);

            }



        }


    }




    //通过计算出的状态做出行为
    private void DoingByPlayerStates(List<PlayerState> playerStates)
    {
        if (playerStates.Count != 0)
        {
            foreach (PlayerState pState in playerStates)
            {
                Invoke(pState.ToString(), 0);
                // Debug.Log(pState.ToString());
            }
            //完成所有状态行为，清空状态
            playerStates.Clear();
        }
        else
        {
            Invoke("Idel", 0);
        }
    }




    //玩家可能的状态枚举
    public enum PlayerState
    {
        Idel,
        Walk,
        Run,
        Jump
    }



    //镜头函数
    public virtual void MouseCamera()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        if (x != 0 || y != 0)
        {
            cameraTemp.RotateAround(transform.position, transform.up, mouseSpeed * x * Time.fixedDeltaTime);
            cameraTemp.RotateAround(transform.position, -cameraTemp.right, mouseSpeed * y * Time.fixedDeltaTime);
        }

    }



    //行为函数
    public virtual void Idel()
    {


    }
    public virtual void Walk()
    {

    }
    public virtual void Run()
    {

        //根据摄像机位置设置角色移动方向
        //step1:虚拟主摄像机位置与角色同一水平线，并获取这样的摄像机世界桌标
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 cameraVirtualPos = new Vector3(cameraPos.x, transform.position.y, cameraPos.z);
        //step2:算出虚拟摄像机位置朝向玩家的向量，并归一化，此为玩家向前施加力的方向向量。
        Vector3 forceForward = (transform.position - cameraVirtualPos).normalized;
        //step3:推导出向右施加力的方向向量
        Vector3 forceRight = Quaternion.AngleAxis(90, Vector3.up) * forceForward;



        //step4:看向要移动的方向，给角色施加移动的力道
        Vector3 forceTemp = forceForward * vValue * Time.deltaTime * runForce + forceRight * hValue * Time.deltaTime * runForce;

        parts.LookAt(transform.position + new Vector3(-forceTemp.z, forceTemp.y, forceTemp.x));


        //施加力移动
       
         rigidBody.velocity = new Vector3(forceTemp.x, rigidBody.velocity.y, forceTemp.z);

       
        //直接控制坐标移动
        //transform.position = transform.position+forceTemp/100;
    }



    public virtual void Jump()
    {

        if (ff.couldJump == true)
        {


            
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, JumpForce * Time.deltaTime, rigidBody.velocity.z) * JumpForce;
           
        }
        



    }
    //bool could2JumpForce = false;


    //private void OnCollisionStay(Collision collision)
    //{


    //    if (collision.transform.tag == "ground")
    //    {
    //        if (collision.transform.position.y>)
    //        {


    //        }
           
    //    }
        
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.transform.tag == "ground")
    //    {

    //            couldjump = false;


    //    }
    //}





}
