using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestCoin : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public static int coinCurrent;
    public static int coinMax;

    private Image chestBar;

    // Start is called before the first frame update
    void Start()
    {
        chestBar = GetComponent<Image>();
        coinCurrent = 0;
        coinMax = 99;
    }

    // Update is called once per frame
    void Update()
    {
        chestBar.fillAmount = (float)coinCurrent / (float)coinMax;
        coinText.text = coinCurrent.ToString() + "/" + coinMax.ToString();
    }
}
