using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJewel : UIGameResources
{
    protected override void Awake()
    {
        base.Awake();

        Init(Action, "JE");
    }

    private void Action()
    {
        tabMenuButtonController.OpenTabMenuShop();
    }
}
