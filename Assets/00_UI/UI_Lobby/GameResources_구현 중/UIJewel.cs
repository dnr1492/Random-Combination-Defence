using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJewel : UIGameResources
{
    protected override void Awake()
    {
        base.Awake();

        Init(bgSpr, imgSpr, btnSpr, Action, "JE");
    }

    private void Action()
    {
        Debug.Log("Jewel Button Click Event");

        // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ //
        // ===== 상점 페이지로 이동 구현 ===== //
        // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ //
    }
}
