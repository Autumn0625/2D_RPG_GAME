using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float hp;
    private Animator ani;
    public float speed;
    public float startWaitTime;
    public int damage;
    private float waitTime;
    public GameObject bloodEffect;//流血特效

    public Transform movePos;
    public Transform leftDownPos;
    public Transform rightUpPos;
    private PlayerHealth playerHealth;

    public void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        ani = GetComponent<Animator>();
        hp = 30.0f;
        Debug.Log("Hi i am enemy my hp is : "+hp);
        waitTime = startWaitTime;
    }
    public void Update()
    {
        
    }
    //被攻擊的Function
    public void onDamage(int damage)
    {
        hp = hp - damage;
        ani.SetTrigger("onDamage");
        Debug.Log("Now Enemy hp:" + hp);
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        GameController.camShake.Shake();

        if (hp <= 0)
        {
            Debug.Log("Enemy is dead");
            ani.SetBool("Death", true);
            Destroy(gameObject);
        }
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
