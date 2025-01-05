using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGold : UIGameResources
{
    protected override void Awake()
    {
        base.Awake();

        Init(Action, "GD");
    }

    private void Action()
    {
        tabMenuButtonController.OpenTabMenuShop();
    }
}
