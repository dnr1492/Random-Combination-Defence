using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPurchaseVirtualCurrency : MonoBehaviour
{
    private Button btn;
    private Text txt;
    private int amount, drawCount;
    private string virtualCurrencyName;
    private string catalogVersion;

    [SerializeField] UIDrawCharacterPopup uiDrawCharacterPopup;
    [SerializeField] UILightning uiLightning;
    [SerializeField] UIGold uiGold;
    [SerializeField] UIJewel uiJewel;

    private void Awake()
    {
        btn = transform.Find("btn").GetComponent<Button>();
        txt = transform.Find("txt").GetComponent<Text>();

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

        txt.text = drawCount + "È¸ »Ì±â";
    }

    private void Purchase()
    {
        if (virtualCurrencyName == "JE") PlayFabManager.instance.DrawCharacters(uiJewel, this, virtualCurrencyName, amount, catalogVersion, drawCount);
    }

    public void DisplayDrawCharacters(Dictionary<string, PlayFabManager.DrawCharacterData> dicDrawCharacterData)
    {
        uiDrawCharacterPopup.Init(dicDrawCharacterData);
    }
}