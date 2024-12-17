using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILightning : UIGameResources
{
    protected override void Awake()
    {
        base.Awake();

        Init(bgSpr, imgSpr, btnSpr, Action, "LI");
    }

    private void Action()
    {
        Debug.Log("Lightning Button Click Event");

        // ���������������������������������� //
        // ===== ���� �������� �̵� ���� ===== //
        // ���������������������������������� //
    }
}