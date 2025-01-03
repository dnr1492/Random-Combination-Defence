using UnityEngine;
using UnityEngine.UI;

public class TabMenuButtonController : ButtonController
{
    [SerializeField] TabMenuController tabMenuController;
    [SerializeField] GameObject[] activeGos, inactiveGos, menus;

    private void Start()
    {
        SetButtonEvent(Action);

        tabMenuController.OpenShop(activeGos, inactiveGos, menus, TabMenuController.eTabMenu.TabMenuLobby);
    }

    private void Action(object obj)
    {
        tabMenuController.OpenShop(activeGos, inactiveGos, menus, (TabMenuController.eTabMenu)obj);
    }

    public bool CheckActive(int index)
    {
        return menus[index].activeSelf;
    }
}
