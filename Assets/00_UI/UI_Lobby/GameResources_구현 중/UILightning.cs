using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILightning : UIGameResources
{
    protected override void Awake()
    {
        base.Awake();

        Init(Action, "LI");
    }

    private void Action()
    {
        tabMenuButtonController.OpenTabMenuShop();
    }
}