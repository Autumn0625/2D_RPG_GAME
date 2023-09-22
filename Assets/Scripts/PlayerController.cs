using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    public float climbSpeed;
    public float restoreTime;

    private BoxCollider2D myFeet;
    private Rigidbody2D rig;
    private Animator ani;
    private bool isGround;
    private bool canDoubleJump;
    private bool isOneWayPlatform;

    private bool isLadder;
    private bool isClimbing;

    private bool isJumping;
    private bool isDoubleJumping;
    private bool isFalling;
    private bool isDoubleFalling;

    private float playerGravity;

    public void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
        playerGravity = rig.gravityScale;
    }
    public void Update()
    {
        if (GameController.isGameAlive)
        {
            CheckAirStatus();
            Flip();
            Run();
            Jump();
            Climb();
            CheckGrounded();
            CheckLadder();
            SwitchAnimation();
            OneWayPlatformCheck();
        }
    }
    void CheckLadder()
    {
        isLadder = myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    }
    void CheckGrounded()
    {
        isGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) ||
                   myFeet.IsTouchingLayers(LayerMask.GetMask("MovingPlatform")) ||
                   myFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
        isOneWayPlatform = myFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
        Debug.Log("Check:" + isGround);
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

    public void Climb()
    {
        if (isLadder)
        {
            float moveY = Input.GetAxis("Vertical");
            Debug.Log(moveY);
            if (moveY > 0.5f || moveY < -0.5)
            {
                ani.SetBool("Climbing", true);
                rig.gravityScale = 0.0f;
                rig.velocity = new Vector2(rig.velocity.x, moveY * climbSpeed);
            }
            else
            {
                if(isJumping || isFalling || isDoubleFalling || isDoubleJumping)
                {
                    ani.SetBool("Climbing", false);
                }
                else
                {
                    ani.SetBool("Climbing", false);
                    rig.velocity = new Vector2(rig.velocity.x, 0.0f);
                }
            }
        }
        else
        {
            ani.SetBool("Climbing", false);
            rig.gravityScale = playerGravity;
        }


    }


    void OneWayPlatformCheck()
    {
        if (isGround && gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }

        float moveY = Input.GetAxis("Vertical");
        if (isOneWayPlatform && moveY < -0.1f)
        {
            gameObject.layer = LayerMask.NameToLayer("OneWayPlatform");
            Invoke("RestorePlayerLayer", restoreTime);
        }
    }

    void RestorePlayerLayer()
    {
        if (!isGround && gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    void CheckAirStatus()
    {
        isJumping = ani.GetBool("Jump");
        isFalling = ani.GetBool("Fall");
        isDoubleJumping = ani.GetBool("DoubleJump");
        isDoubleFalling = ani.GetBool("DoubleFall");
        isClimbing = ani.GetBool("Climbing");
    }
}
