using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenController : MonoBehaviour
{
    private static UIMainScreen mainScreen;
    private static UIContainerScreen containerScreen;

    private void Awake()
    {
        mainScreen = gameObject.transform.Find("UIMainScreen").GetComponent<UIMainScreen>();
        containerScreen = gameObject.transform.Find("UIContainerScreen_UI리소스 적용 전").GetComponent<UIContainerScreen>();

        containerScreen.gameObject.SetActive(false);
    }

    public static void OnMainScreen(bool isActive = true)
    {
        mainScreen.gameObject.SetActive(isActive);
        containerScreen.gameObject.SetActive(!isActive);
    }

    public static void OnContainerScreen(bool isActive = true)
    {
        mainScreen.gameObject.SetActive(!isActive);
        containerScreen.gameObject.SetActive(isActive);
    }
}
