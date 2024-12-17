using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDrawCharacterPopup : MonoBehaviour
{
    [SerializeField] GameObject[] arrUIDrawCharacterCard;
    [SerializeField] Image[] arrImgCharacter;
    [SerializeField] Text[] arrTxtName, arrTxtGainQuantity;
    [SerializeField] Button btnClose;

    private void Awake()
    {
        gameObject.SetActive(false);

        btnClose.onClick.AddListener(() => {
            gameObject.SetActive(false);
        });

        arrImgCharacter = new Image[arrUIDrawCharacterCard.Length];
        arrTxtName = new Text[arrUIDrawCharacterCard.Length];
        arrTxtGainQuantity = new Text[arrUIDrawCharacterCard.Length];

        for (int i = 0; i < arrUIDrawCharacterCard.Length; i++)
        {
            arrImgCharacter[i] = arrUIDrawCharacterCard[i].transform.GetChild(0).GetComponent<Image>();
            arrTxtName[i] = arrUIDrawCharacterCard[i].transform.GetChild(1).GetComponent<Text>();
            arrTxtGainQuantity[i] = arrUIDrawCharacterCard[i].transform.GetChild(2).GetComponentInChildren<Text>();
            arrUIDrawCharacterCard[i].SetActive(false);
        }
    }

    public void Init(Dictionary<string, PlayFabManager.DrawCharacterData> dicDrawCharacterData)
    {
        gameObject.SetActive(true);

        for (int i = 0; i < arrUIDrawCharacterCard.Length; i++) arrUIDrawCharacterCard[i].SetActive(false);

        int index = 0;
        foreach (var data in dicDrawCharacterData)
        {
            arrUIDrawCharacterCard[index].SetActive(true);
            arrTxtName[index].text = data.Value.displayName;
            arrTxtGainQuantity[index].text = data.Value.cardCount.ToString();
            index++;
        }
    }
}
