using PlayFab.GroupsModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    private static LoadingManager instance;
    private GameObject loginLoading, loading;
    private readonly WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(1f);
    private readonly float speed = 1f;

    [Header("로그인 로딩 화면")]
    [SerializeField] GameObject loginLoadingPrefab;
    private Image progressBarBg, progressBar;
    private Button btnLoading;
    private Text txtLoading;

    [Header("기타 로딩 화면")]
    [SerializeField] GameObject loadingPrefab;
    private Image loadingIcon;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            loginLoadingPrefab.SetActive(false);
            loadingPrefab.SetActive(false);
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
    public static void ShowLoginLoading()
    {
        if (instance == null) return;

        if (instance.loginLoading == null) {
            instance.loginLoading = Instantiate(instance.loginLoadingPrefab);
            instance.loginLoading.SetActive(true);
            if (instance.progressBarBg == null) instance.progressBarBg = instance.loginLoading.transform.Find("progressBarBg").GetComponent<Image>();
            if (instance.progressBar == null) instance.progressBar = instance.loginLoading.transform.Find("progressBarBg/progressBar").GetComponent<Image>();
            if (instance.btnLoading == null) instance.btnLoading = instance.loginLoading.transform.Find("btnLoading").GetComponent<Button>();
            if (instance.txtLoading == null) instance.txtLoading = instance.loginLoading.transform.Find("btnLoading/txtLoading").GetComponent<Text>();
            instance.progressBar.fillAmount = 0;
            instance.progressBarBg.gameObject.SetActive(true);
            instance.txtLoading.text = "Loding...";
            DontDestroyOnLoad(instance.loginLoading);
        }
    }

    /// <summary>
    /// 로딩 화면 표시
    /// </summary>
    public static void ShowLoading()
    {
        if (instance == null) return;

        if (instance.loading == null) {
            instance.loading = Instantiate(instance.loadingPrefab);
            instance.loading.SetActive(true);
            if (instance.loadingIcon == null) instance.loadingIcon = instance.loading.transform.Find("loadingIcon").GetComponent<Image>();
            instance.loadingIcon.fillAmount = 0;
            DontDestroyOnLoad(instance.loading);
        }
    }

    /// <summary>
    /// 로딩 화면 숨기기
    /// </summary>
    public static void HideLoginLoading()
    {
        if (instance.loginLoading != null) {
            Destroy(instance.loginLoading);
            instance.loginLoading = null;
        }
    }

    /// <summary>
    /// 로딩 화면 숨기기
    /// </summary>
    public static void HideLoading()
    {
        if (instance.loading != null) {
            Destroy(instance.loading);
            instance.loading = null;
        }
    }

    #region (비동기) 씬 로드 - Additive / 데이터, 씬 로딩
    public static IEnumerator LoadSceneAdditive(string loadSceneName, string unloadSceneName, Action onComplete = null)
    {
        ShowLoginLoading();

        yield return new WaitUntil(() => PlayFabManager.instance.CheckLoginSuccess());

        #region 데이터 로딩 작업
        IEnumerator[] loadTasks = new IEnumerator[] {
        LoadDataStep(() => DataManager.GetInstance().LoadCharacterCardLevelData()),
        LoadDataStep(() => DataManager.GetInstance().LoadCharacterCardLevelInfoData()),
        LoadDataStep(() => DataManager.GetInstance().LoadCharacterData()),
        LoadDataStep(() => DataManager.GetInstance().LoadCharacterSkillData()),
        LoadDataStep(() => DataManager.GetInstance().LoadCharacterRecipeData()),
        LoadDataStep(() => DataManager.GetInstance().LoadPlayWaveData()),
        LoadDataStep(() => SpriteManager.GetInstance().LoadSpriteAll()),
        LoadDataStep(() => PlayFabManager.instance.InitUserData()),
        };

        float totalSteps = loadTasks.Length;
        float completeSteps = 0f;

        foreach (var task in loadTasks)
        {
            yield return task;
            completeSteps++;
            instance.progressBar.fillAmount = completeSteps / totalSteps;
            instance.txtLoading.text = $"Loading Data... {Mathf.RoundToInt((completeSteps / totalSteps) * 100)}%";
        }
        #endregion

        yield return new WaitUntil(() => instance.progressBar.fillAmount == 1);
        yield return instance.waitForSecondsRealtime;

        #region 씬 로딩 작업
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;

        instance.progressBar.fillAmount = 0f;
        instance.txtLoading.text = $"Loading Scene... {Mathf.RoundToInt(instance.progressBar.fillAmount * 100)}%";

        while (true) {
            if (asyncOperation.progress < 0.9f) {
                float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                instance.progressBar.fillAmount = progress;
                instance.txtLoading.text = $"Loading Scene... {Mathf.RoundToInt(progress * 100)}%";
            }
            else {
                instance.progressBar.fillAmount = 1;
                instance.txtLoading.text = $"Loading Scene... {Mathf.RoundToInt(1 * 100)}%";
                asyncOperation.allowSceneActivation = true;
                break;
            }
            yield return null;
        }
        #endregion

        yield return new WaitUntil(() => asyncOperation.allowSceneActivation);
        instance.txtLoading.text = $"Loading Final...";
        yield return instance.waitForSecondsRealtime;

        instance.txtLoading.text = "Press Touch to Start";
        instance.progressBarBg.gameObject.SetActive(false);
        instance.btnLoading.gameObject.SetActive(true);
        instance.btnLoading.onClick.AddListener(() => {
            SceneManager.UnloadSceneAsync(unloadSceneName);
            HideLoginLoading();
            onComplete?.Invoke();
        });
    }

    private static IEnumerator LoadDataStep(Action loadAction)
    {
        yield return null;
        loadAction.Invoke();
    }
    #endregion

    #region (비동기) 씬 로드
    public static IEnumerator LoadScene(string loadSceneName, Action onComplete = null)
    {
        yield return new WaitUntil(() => PlayFabManager.instance.CheckLoginSuccess());

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(loadSceneName);
        asyncOperation.allowSceneActivation = false;

        while (true) {
            if (!asyncOperation.isDone) {
                yield return null;
            }
            asyncOperation.allowSceneActivation = true;
            onComplete?.Invoke();
            break;
        }
    }
    #endregion
}