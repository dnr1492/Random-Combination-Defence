using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameResources : MonoBehaviour
{
    protected Button btn;
    protected Text txt;

    protected virtual void Awake()
    {
        btn = transform.Find("btn_bg/btn").GetComponent<Button>();
        txt = transform.Find("txt").GetComponent<Text>();
    }

    protected void Init(Action action, string virtualCurrencyName)
    {
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