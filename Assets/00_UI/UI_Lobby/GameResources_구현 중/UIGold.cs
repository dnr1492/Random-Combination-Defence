using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGold : UIGameResources
{
    protected override void Awake()
    {
        base.Awake();

        Init(bgSpr, imgSpr, btnSpr, Action, "GD");
    }

    private void Action()
    {
        Debug.Log("Gold Button Click Event");

        // ���������������������������������� //
        // ===== ���� �������� �̵� ���� ===== //
        // ���������������������������������� //
    }
}
