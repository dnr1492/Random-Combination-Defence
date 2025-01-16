using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPurchaseDrawCharacters : MonoBehaviour
{
    private Button btn;
    private Text txtPrice, txtDraw;
    private int price, drawCount;
    private string catalogVersion;

    private readonly string addVirtualCurrencyName = "JE";
 
    [SerializeField] UIDrawCharacterPopup uiDrawCharacterPopup;
    [SerializeField] UIJewel uiJewel;

    private void Awake()
    {
        btn = transform.Find("btn").GetComponentInChildren<Button>();
        txtPrice = transform.Find("price").GetComponentInChildren<Text>();
        txtDraw = btn.GetComponentInChildren<Text>();

        Display();

        btn.onClick.AddListener(PurchaseAsync);
    }

    private void Display()
    {
        string[] strs = gameObject.name.Split("_");
        catalogVersion = strs[0];
        price = int.Parse(strs[1]);
        drawCount = int.Parse(strs[2]);

        txtPrice.text = price + " º¸¼®";
        txtDraw.text = drawCount + "È¸ »Ì±â";
    }

    private async void PurchaseAsync()
    {
        try {
            LoadingManager.ShowLoading();
            await PlayFabManager.instance.DrawCharactersAsync(uiJewel, this, addVirtualCurrencyName, price, catalogVersion, drawCount);
        }
        finally {
            LoadingManager.HideLoading();
        }
    }

    public void DisplayDrawCharacters(Dictionary<string, PlayFabManager.DrawCharacterData> dicDrawCharacterData)
    {
        uiDrawCharacterPopup.Init(dicDrawCharacterData);
    }
}