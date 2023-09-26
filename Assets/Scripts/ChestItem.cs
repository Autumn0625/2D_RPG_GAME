using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestItem : MonoBehaviour
{
    private bool isPlayerInChest;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y)) 
        {
            if (isPlayerInChest) 
            {
                if(CoinUI.CurrentCoinQuantity > 0)
                {
                    SoundManager.PlayThrowCoinClip();
                    ChestCoin.coinCurrent++;
                    CoinUI.CurrentCoinQuantity--;
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isPlayerInChest = true;
            Debug.Log("isPlayerInChest");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")
            && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            isPlayerInChest = false;
            Debug.Log("NotisPlayerInChest");
        }

    }
}
