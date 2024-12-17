using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameResources : MonoBehaviour
{
    protected Image bg, img, btnImg;
    protected Button btn;
    protected Text txt;

    public Sprite bgSpr, imgSpr, btnSpr;

    protected virtual void Awake()
    {
        bg = transform.Find("bg").GetComponent<Image>();
        img = transform.Find("img").GetComponent<Image>();
        btnImg = transform.Find("btn").GetComponent<Image>();
        btn = transform.Find("btn").GetComponent<Button>();
        txt = transform.Find("txt").GetComponent<Text>();
    }

    protected void Init(Sprite bgSpr, Sprite imgSpr, Sprite btnSpr, Action action, string virtualCurrencyName)
    {
        bg.sprite = bgSpr;
        img.sprite = imgSpr;
        btnImg.sprite = btnSpr;
        btn.onClick.AddListener(() => { action(); });

        // ============== �ʱ� �κ� ȭ�� �ε� �� �����Ƿ� ����ȭ �ʿ� ================ // 
        // ============== �ʱ� �κ� ȭ�� �ε� �� �����Ƿ� ����ȭ �ʿ� ================ // 
        // ============== �ʱ� �κ� ȭ�� �ε� �� �����Ƿ� ����ȭ �ʿ� ================ // 
        PlayFabManager.instance.DisplayGameResources(this, virtualCurrencyName);
    }

    public void DisplayGameResourceAmount(int amount)
    {
        txt.text = amount.ToString();
    }
}