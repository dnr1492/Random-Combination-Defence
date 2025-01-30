using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacter : MonoBehaviour
{
    private List<UICharacterCard> uiCharacterCards;
    private bool isInit = true;

    private void Awake()
    {
        uiCharacterCards = new List<UICharacterCard>();
        uiCharacterCards.AddRange(GetComponentsInChildren<UICharacterCard>());
        foreach (UICharacterCard card in uiCharacterCards) {
            card.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        Init();
    }

    public async void Init()
    {
        if (isInit) {
            isInit = false;
            return;
        }

        try {
            LoadingManager.ShowLoading();
            await PlayFabManager.instance.SetCharacterCardData(uiCharacterCards);
        }
        finally {
            LoadingManager.HideLoading();
        }
    }
}