using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPurchaseVirtualCurrency : MonoBehaviour
{
    private Button btn;
    private Text txtDraw, txtPrice;
    private int amount, drawCount;
    private string virtualCurrencyName;
    private string catalogVersion;

    [SerializeField] UIDrawCharacterPopup uiDrawCharacterPopup;
    [SerializeField] UILightning uiLightning;
    [SerializeField] UIGold uiGold;
    [SerializeField] UIJewel uiJewel;

    private void Awake()
    {
        btn = transform.Find("btn").GetComponentInChildren<Button>();
        txtDraw = transform.Find("btn/txt_draw").GetComponentInChildren<Text>();
        txtPrice = transform.Find("price/bg_price/txt_price").GetComponentInChildren<Text>();

        Display();

        btn.onClick.AddListener(Purchase);
    }

    private void Display()
    {
        string[] strs = gameObject.name.Split("_");
        virtualCurrencyName = strs[1];
        amount = int.Parse(strs[2]);
        catalogVersion = strs[0];
        drawCount = int.Parse(strs[3]);

        txtDraw.text = drawCount + "È¸ »Ì±â";
        txtPrice.text = amount + " º¸¼®";
    }

    private void Purchase()
    {
        if (virtualCurrencyName == "JE") PlayFabManager.instance.OnClickDrawCharactersAsync(uiJewel, this, virtualCurrencyName, amount, catalogVersion, drawCount);
    }

    public void DisplayDrawCharacters(Dictionary<string, PlayFabManager.DrawCharacterData> dicDrawCharacterData)
    {
        uiDrawCharacterPopup.Init(dicDrawCharacterData);
    }
}