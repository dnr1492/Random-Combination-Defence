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

        // ============== 초기 로비 화면 로드 시 느리므로 최적화 필요 ================ // 
        // ============== 초기 로비 화면 로드 시 느리므로 최적화 필요 ================ // 
        // ============== 초기 로비 화면 로드 시 느리므로 최적화 필요 ================ // 
        PlayFabManager.instance.DisplayGameResources(this, virtualCurrencyName);
    }

    public void DisplayGameResourceAmount(int amount)
    {
        txt.text = amount.ToString();
    }
}