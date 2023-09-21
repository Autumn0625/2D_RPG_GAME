using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    //public float climbSpeed;

    private BoxCollider2D myfeet;
    private Rigidbody2D rig;
    private Animator ani;
    private CapsuleCollider2D cap;
    private bool isGround;
    private bool canDoubleJump;
    public float restoreTime;
    //public LayerMask groundMask, enemymask;
    
    //private bool isFlip = false;
    
    //private bool canAttack = true;
    //private bool isOneWayPlatform;

    //private bool isLadder;
    //private bool isClimbing;

    //private float playerGravity;

    public void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        myfeet = GetComponent<BoxCollider2D>();
    //    cap = GetComponent<CapsuleCollider2D>();
    //    playerGravity = rig.gravityScale;
    }
    public void Update()
    {
        Flip();
        Run();
        Jump();
        //Attack();
        CheckGrounded();
        SwitchAnimation();
        //CheckAirStatus();
        //OneWayPlatformCheck();


        //isOneWayPlatform = cap.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
        //isGrounded = Physics2D.OverlapCircle(groundpoint.position, .2f,groundMask) || cap.IsTouchingLayers(LayerMask.GetMask("MovingPlatform")) || cap.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
        //canAttack = isGrounded;
        //CheckLadder();
    }
    //void CheckLadder()
    //{
    //    isLadder = cap.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    //}
    public void CheckGrounded()
    {
        isGround = myfeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
        Debug.Log(isGround);
    }
    public void Flip()
    {
        bool playerHasXAxis = Mathf.Abs(rig.velocity.x) > Mathf.Epsilon;
        if (playerHasXAxis)
        {
            if(rig.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (rig.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    public void Run()
    {
        float moveDir = Input.GetAxis("Horizontal");
        Vector2 playerVel = new Vector2(moveDir * runSpeed, rig.velocity.y);
        rig.velocity = playerVel;
        bool playerHasXaxisSpeed = Mathf.Abs(rig.velocity.x) > Mathf.Epsilon;
        ani.SetBool("Run", playerHasXaxisSpeed);
    }
    public void Jump()
    {
        if(Input.GetButtonDown("Jump")) 
        {
            if(isGround)
            {
                ani.SetBool("Jump", true);
                Vector2 jumpVel = new Vector2(0.0f, jumpSpeed);
                rig.velocity = Vector2.up * jumpVel;
                canDoubleJump = true;
            }
            else
            {
                if(canDoubleJump)
                {
                    ani.SetBool("DoubleJump", true);
                    Vector2 doubleJumpvel = new Vector2(0.0f, doubleJumpSpeed);
                    rig.velocity = Vector2.up * doubleJumpvel;
                    canDoubleJump = false;
                }
            }
        }
    }
    //public void Attack()
    //{
    //    if (Input.GetButtonDown("Attack"))
    //    {
    //        ani.SetTrigger("Attack");
    //    }
    //}
    public void SwitchAnimation()
    {
        ani.SetBool("Idle", false);
        if (ani.GetBool("Jump"))
        {
            if(rig.velocity.y < 0.0f)
            {
                ani.SetBool("Jump", false);
                ani.SetBool("Fall", true);
            }
        }
        else if (isGround)
        {
            ani.SetBool("Fall", false);
            ani.SetBool("Idle", true);
        }

        if (ani.GetBool("DoubleJump"))
        {
            if (rig.velocity.y < 0.0f)
            {
                ani.SetBool("DoubleJump", false);
                ani.SetBool("DoubleFall", true);
            }
        }
        else if (isGround)
        {
            ani.SetBool("DoubleFall", false);
            ani.SetBool("Idle", true);
        }
    }

    //public void Climb()
    //{
    //    if (isLadder)
    //    {
    //        float moveY = Input.GetAxis("Vertical");
    //        Debug.Log(moveY);
    //        if (moveY > 0.5f || moveY < -0.5)
    //        {
    //            ani.SetBool("Climbing", true);
    //            rig.gravityScale = 0.0f;
    //            rig.velocity = new Vector2(rig.velocity.x, moveY * climbSpeed);
    //        }
    //        else
    //        {
    //            ani.SetBool("Climbing", false);
    //            rig.velocity = new Vector2(rig.velocity.x, 0.0f);
    //        }
    //    }
    //    else
    //    {
    //        ani.SetBool("Climbing", false);
    //        rig.gravityScale = playerGravity;
    //    }

        
    //}
    
    
    //void OneWayPlatformCheck()
    //{
    //    if (isGrounded && gameObject.layer != LayerMask.NameToLayer("player"))
    //    {
    //        gameObject.layer = LayerMask.NameToLayer("player");
    //    }
    //    float moveY = Input.GetAxis("Vertical");
    //    if (isOneWayPlatform && moveY <- 0.1f)
    //    {
    //        gameObject.layer = LayerMask.NameToLayer("OneWayPlatform");
    //        Invoke("RestorePlayerLayer", restoreTime);
    //    }
    //}

    //void RestorePlayerLayer()
    //{
    //    if(!isGrounded && gameObject.layer != LayerMask.NameToLayer("player"))//Player§ï¦¨player
    //    {
    //        gameObject.layer = LayerMask.NameToLayer("player");
    //    }
    //}

    //void CheckAirStatus()
    //{
    //    isClimbing = ani.GetBool("Climbing");
    //}
}
