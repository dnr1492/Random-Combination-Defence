using UnityEngine;
using UnityEngine.UI;

public class TabMenuButtonController : ButtonController
{
    [SerializeField] TabMenuController tabMenuController;
    [SerializeField] GameObject[] activeGos, inactiveGos, menus;

    private void Start()
    {
        SetButtonEvent(Action);

        tabMenuController.OpenTabMenu(activeGos, inactiveGos, menus, TabMenu.TabMenuLobby);
    }

    private void Action(object obj)
    {
        tabMenuController.OpenTabMenu(activeGos, inactiveGos, menus, (TabMenu)obj);
    }

    public void OpenTabMenuShop()
    {
        tabMenuController.OpenTabMenu(activeGos, inactiveGos, menus, TabMenu.TabMenuShop);
    }

    public bool CheckActive(int index)
    {
        return menus[index].activeSelf;
    }
}
