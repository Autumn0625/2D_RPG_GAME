using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    public int startCoinQuantity;
    public TextMeshProUGUI coinQuantity;

    public static int CurrentCoinQuantity;

    // Start is called before the first frame update
    void Start()
    {
        CurrentCoinQuantity = startCoinQuantity;
    }

    // Update is called once per frame
    void Update()
    {
        coinQuantity.text = CurrentCoinQuantity.ToString();
    }
}
