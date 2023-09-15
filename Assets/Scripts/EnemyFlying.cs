using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    private int hp;
    private Animator ani;
    public float speed;
    public float startWaitTime;
    private float waitTime;
    public GameObject bloodEffect;//流血特效
    public int damage;

    public Transform movePos;
    public Transform leftDownPos;
    public Transform rightUpPos;
    private PlayerHealth playerHealth;

    public void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        ani = GetComponent<Animator>();
        hp = 30;
        Debug.Log("Hi i am EnemyFlying my hp is : " + hp);
        waitTime = startWaitTime;
        movePos.position = GetRandomPos();
    }
    public void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, movePos.position, speed * Time.deltaTime);

        if(Vector2.Distance(transform.position, movePos.position) < 0.1f)
        {
            if(waitTime <= 0)
            {
                movePos.position = GetRandomPos();
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
    //被攻擊的Function
    public void onDamage(int damage)
    {
        hp = hp - damage;
        ani.SetTrigger("onDamage");
        Debug.Log("Now EnemyFlying hp:" + hp);
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        GameController.camShake.Shake();

        if (hp <= 0)
        {
            Debug.Log("EnemyFlying is dead");
            ani.SetBool("Death", true);
            Destroy(gameObject);
        }
    }
    Vector2 GetRandomPos()
    {
        Vector2 rndPos = new Vector2(Random.Range(leftDownPos.position.x, rightUpPos.position.x), Random.Range(leftDownPos.position.y, rightUpPos.position.y));
        return rndPos;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            if (playerHealth != null)
            {
                playerHealth.DamagePlayer(damage);
                Debug.Log("Hurt!");
            }
        }
    }
}
