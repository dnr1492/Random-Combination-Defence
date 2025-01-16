using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    private static LoadingManager instance;
    private static GameObject loading;
    private static Image loadingIcon;

    [SerializeField] GameObject loadingPrefab;

    private readonly float speed = 1f;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (loadingIcon == null) return;
        loadingIcon.fillAmount += speed * Time.deltaTime;
        if (loadingIcon.fillAmount >= 1f) loadingIcon.fillAmount = 0f;
    }

    /// <summary>
    /// 로딩 화면 표시
    /// </summary>
    public static void ShowLoading()
    {
        if (instance == null) return;

        if (loading == null) {
            loading = Instantiate(instance.loadingPrefab);
            loading.SetActive(true);
            if (loadingIcon == null) loadingIcon = loading.transform.Find("SafeArea/loadingIcon").GetComponent<Image>();
            loadingIcon.fillAmount = 0;
            DontDestroyOnLoad(loading);
        }
    }

    /// <summary>
    /// 로딩 화면 숨기기
    /// </summary>
    public static void HideLoading()
    {
        if (loading != null) {
            Destroy(loading);
            loading = null;
        }
    }
}
