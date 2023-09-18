using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float InputX;
    
    private Rigidbody2D rig;
    private Animator ani;
    private CapsuleCollider2D cap;

    public float restoreTime;
    public Transform groundpoint;
    public Transform attackpoint;
    public float attackRange = 3f;

    public LayerMask groundMask, enemymask;
    
    private bool isFlip = false;
    private bool isGrounded= false;
    private bool canAttack = true;
    private bool isOneWayPlatform;

    public void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        cap = GetComponent<CapsuleCollider2D>();
    }
    public void Update()
    {
        rig.velocity = new Vector2(moveSpeed * InputX, rig.velocity.y);

        //判斷 使用者是否有輸入移動控制 有的話 讓腳色進行跑步動畫的播放
        ani.SetBool("isRun", Mathf.Abs(rig.velocity.x) > 0);
        ani.SetBool("isGrounded", isGrounded);
        ani.SetFloat("yVelocity", rig.velocity.y);

        OneWayPlatformCheck();

        if (!isFlip)
        {
            if(rig.velocity.x < 0)
            {
                isFlip = true;
                transform.Rotate(0.0f, 180f, 0.0f);
            }
        }
        else
        {
            if (rig.velocity.x > 0)
            {
                isFlip = false;
                transform.Rotate(0.0f, 180f, 0.0f);
            }
        }
        isOneWayPlatform = cap.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
        isGrounded = Physics2D.OverlapCircle(groundpoint.position, .2f,groundMask) || cap.IsTouchingLayers(LayerMask.GetMask("MovingPlatform")) || cap.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
        canAttack = isGrounded;
    }

    

    public void Move(InputAction.CallbackContext context)
    {
        InputX = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rig.velocity = new Vector2(rig.velocity.x, 6);
        }
    }
    public void Attack(InputAction.CallbackContext context)
    {
        //檢查玩家是否可以攻擊
        if (canAttack)
        {
            ani.SetBool("attack", true);
        }
    }
    private void CheckAttackHit()
    {
        //宣告 打到的目標物件 => 有可能是空的 如果是空的 => 沒打到東西 , 如果有 => 傳送指令給被攻擊者
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackpoint.position, attackRange, enemymask);

        foreach (Collider2D collider in detectedObjects)
        {
            Debug.Log(collider.gameObject.name);
            collider.gameObject.SendMessage("onDamage", 10.0f);
        }
    }
    public void EndAttack()
    {
        ani.SetBool("attack", false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundpoint.position, .2f);
        Gizmos.DrawWireSphere(attackpoint.position, attackRange);
    }
    
    void OneWayPlatformCheck()
    {
        //if(isGrounded) 
        //{ 
        //    if(isGrounded && gameObject.)
        //}
        float moveY = Input.GetAxis("Vertical");
        if (isOneWayPlatform && moveY <- 0.1)
        {
            gameObject.layer = LayerMask.NameToLayer("OneWayPlatform");
            Invoke("RestorePlayerLayer", restoreTime);
        }
    }

    void RestorePlayerLayer()
    {
        if(!isGrounded &&gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}
