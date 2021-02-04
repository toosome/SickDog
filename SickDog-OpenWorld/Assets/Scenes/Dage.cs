using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dage : MonoBehaviour
{


    public float moveSpeed=20;
    public float jumpSpeed = 20;

    private State dageState = State.OnIdel;



    private Rigidbody rigidBody;

    //************
    public static Dage Instance;


    // Start is called before the first frame update
    void Awake()
    {

        rigidBody = transform.GetComponent<Rigidbody>();


        //*******************
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {


        StateManager();


        Debug.Log(dageState);

        switch (dageState)
        {
            case State.OnWalk:
                Walk();
                break;
            case State.OnJump:
                Jump();
                break;
            case State.OnIdel:
                Idel();
                break;
            case State.OnRun:
                Run();
                break;
            default:
                break;
        }



    }





    void StateManager()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        

        if (x != 0 || y != 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                dageState = State.OnWalk;
                //Debug.Log("OnWalk");
            }
            else
            {
                dageState = State.OnRun;
               // Debug.Log("OnRun");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {

            dageState = State.OnJump;
           // Debug.Log("Space");
        }
        //else
        //{
        //    dageState = State.OnIdel;
        //}

    }


    void Run()
    {

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Debug.Log(x + "...." + y);

        rigidBody.velocity = new Vector3(y*moveSpeed, rigidBody.velocity.y, -x*moveSpeed) ;

    }


    void Jump()
    {
     
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 10, rigidBody.velocity.z) * jumpSpeed;      
        dageState = State.OnIdel;

    }




    void Walk()
    {
       

    }

    void Idel()
    {


    }




    enum State
    {
        OnWalk,
        OnJump,
        OnIdel,
        OnRun
    }

}
