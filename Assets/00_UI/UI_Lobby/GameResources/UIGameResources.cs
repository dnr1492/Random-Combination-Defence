using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameResources : MonoBehaviour
{
    protected TabMenuButtonController tabMenuButtonController;
    protected Button btn;
    protected Text txt;

    protected virtual void Awake()
    {
        tabMenuButtonController = FindObjectOfType<TabMenuButtonController>();
        btn = transform.Find("btn_bg/btn").GetComponent<Button>();
        txt = transform.Find("txt").GetComponent<Text>();
    }

    protected void Init(Action action, string virtualCurrencyName)
    {
        btn.onClick.AddListener(() => { action(); });

        PlayFabManager.instance.DisplayGameResources(this, virtualCurrencyName);
    }

    public void DisplayGameResourceAmount(int amount)
    {
        txt.text = amount.ToString();
    }
}