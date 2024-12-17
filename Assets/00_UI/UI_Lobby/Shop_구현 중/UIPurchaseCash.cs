using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPurchaseCash : MonoBehaviour
{
    private Button btn;
    private Text txtAmount, txtPrice;
    private int amount, price;
    private string virtualCurrencyName;

    [SerializeField] UILightning uiLightning;
    [SerializeField] UIGold uiGold;
    [SerializeField] UIJewel uiJewel;

    private void Awake()
    {
        btn = transform.Find("btn").GetComponent<Button>();
        txtAmount = btn.GetComponentInChildren<Text>();
        txtPrice = transform.Find("price").GetComponentInChildren<Text>();

        DisplayGameResource();

        btn.onClick.AddListener(() => {
            PurchaseGameResource(price);
        });
    }

    private void DisplayGameResource()
    {
        string[] strs = gameObject.name.Split("_");
        amount = int.Parse(strs[2]);
        virtualCurrencyName = strs[1];

        if (amount == 100) price = 9800;
        else if (amount == 1000) price = 98000;
        else if (amount == 10000) price = 998000;

        txtAmount.text = amount.ToString();
        txtPrice.text = price.ToString() + "KRW";
    }

    private void PurchaseGameResource(int price)
    {
        Debug.Log("가격 : " + price);

        // ===== 현금으로 구매 할 수 있도록 수정하기 ===== //
        // ===== 현금으로 구매 할 수 있도록 수정하기 ===== //
        // ===== 현금으로 구매 할 수 있도록 수정하기 ===== //
        if (virtualCurrencyName == "JE") PlayFabManager.instance.AddUserVirtualCurrency(uiJewel, virtualCurrencyName, amount);
    }
}