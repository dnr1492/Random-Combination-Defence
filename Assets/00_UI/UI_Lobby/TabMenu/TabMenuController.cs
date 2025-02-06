using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabMenuController : MonoBehaviour
{
    #region ÅÇ ¸Þ´º ¿­±â
    /// <summary>
    /// ÅÇ ¸Þ´º ¿­±â
    /// </summary>
    /// <param name="tabMenuActiveGos"></param>
    /// <param name="tabMenuInactiveGos"></param>
    /// <param name="menus"></param>
    /// <param name="tabMenu"></param>
    public void OpenTabMenu(GameObject[] tabMenuActiveGos, GameObject[] tabMenuInactiveGos, GameObject[] menus, TabMenu tabMenu)
    {
        for (int i = 0; i < tabMenuActiveGos.Length; i++) tabMenuActiveGos[i].SetActive(false);
        tabMenuActiveGos[(int)tabMenu].SetActive(true);

        for (int i = 0; i < tabMenuInactiveGos.Length; i++) tabMenuInactiveGos[i].SetActive(true);
        tabMenuInactiveGos[(int)tabMenu].SetActive(false);

        for (int i = 0; i < menus.Length; i++) menus[i].SetActive(false);
        menus[(int)tabMenu].SetActive(true);
    }
    #endregion
}