using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPurchaseGem : MonoBehaviour
{
    private Button btn;
    private Text txtPrice, txtAmount;
    private int price, amount;

    private readonly string addVirtualCurrencyName = "JE";

    [SerializeField] UIJewel uiJewel;

    private void Awake()
    {
        btn = transform.Find("btn").GetComponent<Button>();
        txtPrice = transform.Find("price").GetComponentInChildren<Text>();
        txtAmount = btn.GetComponentInChildren<Text>();

        Display();

        btn.onClick.AddListener(() => {
            Purchase(price);
        });
    }

    private void Display()
    {
        string[] strs = gameObject.name.Split("_");
        amount = int.Parse(strs[1]);

        if (amount == 100) price = 9800;
        else if (amount == 1000) price = 98000;
        else if (amount == 10000) price = 998000;

        txtPrice.text = price.ToString() + "KRW";
        txtAmount.text = amount.ToString();
    }

    private void Purchase(int price)
    {
        Debug.Log("가격 : " + price);

        // ===== 현금으로 구매 할 수 있도록 수정하기 ===== //
        // ===== 현금으로 구매 할 수 있도록 수정하기 ===== //
        // ===== 현금으로 구매 할 수 있도록 수정하기 ===== //
        PlayFabManager.instance.AddUserVirtualCurrency(uiJewel, addVirtualCurrencyName, amount);
    }
}