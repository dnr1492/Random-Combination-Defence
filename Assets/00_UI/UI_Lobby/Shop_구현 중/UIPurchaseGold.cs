using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPurchaseGold : MonoBehaviour
{
    private Button btn;
    private Text txtPrice, txtAmount;
    private int price, amount;

    private readonly string addVirtualCurrencyName = "GD", subtractVirtualCurrencyName = "JE";

    [SerializeField] UIGold uiGold;
    [SerializeField] UIJewel uiJewel;
    
    private void Awake()
    {
        btn = transform.Find("btn").GetComponent<Button>();
        txtPrice = transform.Find("price").GetComponentInChildren<Text>();
        txtAmount = btn.GetComponentInChildren<Text>();
       
        Display();

        btn.onClick.AddListener(Purchase);
    }

    private void Display()
    {
        string[] strs = gameObject.name.Split("_");
        price = int.Parse(strs[1]);
        amount = int.Parse(strs[2]);

        txtPrice.text = price + " º¸¼®";
        txtAmount.text = amount.ToString();
    }

    private void Purchase()
    {
        PlayFabManager.instance.SubtractUserVirtualCurrency(uiJewel, subtractVirtualCurrencyName, price);
        PlayFabManager.instance.AddUserVirtualCurrency(uiGold, addVirtualCurrencyName, amount);
    }
}
