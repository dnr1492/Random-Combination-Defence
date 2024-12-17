using UnityEngine;
using UnityEngine.UI;

public class TabMenuButtonController : ButtonController
{
    [SerializeField] TabMenuController tabMenuController;
    [SerializeField] GameObject[] inactiveGos, activeGos, menus;

    private void Start()
    {
        SetButtonEvent(Action);

        tabMenuController.OpenShop(inactiveGos, activeGos, menus, TabMenuController.eTabMenu.TabMenuLobby);
    }

    private void Action(object obj)
    {
        tabMenuController.OpenShop(inactiveGos, activeGos, menus,(TabMenuController.eTabMenu)obj);
    }

    public bool CheckActive(int index)
    {
        return menus[index].activeSelf;
    }
}
