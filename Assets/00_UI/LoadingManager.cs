using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    private static LoadingManager instance;
    private GameObject loginLoading, loading;
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

        if (instance.loginLoading == null)
        {
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

    #region 씬 로드 - 동기 / Additive / 데이터 로드
    public static IEnumerator LoadSceneAdditive(string loadSceneName, string unloadSceneName)
    {
        ShowLoginLoading();

        yield return new WaitUntil(() => PlayFabManager.instance.CheckLoginSuccess());

        //데이터 로드 작업
        IEnumerator[] loadTasks = new IEnumerator[] {
        LoadStep(() => DataManager.GetInstance().LoadCharacterCardLevelData()),
        LoadStep(() => DataManager.GetInstance().LoadCharacterCardLevelInfoData()),
        LoadStep(() => DataManager.GetInstance().LoadCharacterData()),
        LoadStep(() => DataManager.GetInstance().LoadCharacterSkillData()),
        LoadStep(() => DataManager.GetInstance().LoadCharacterRecipeData()),
        LoadStep(() => DataManager.GetInstance().LoadPlayWaveData()),
        LoadStep(() => DataManager.GetInstance().LoadPlayMapData()),
        LoadStep(() => DataManager.GetInstance().LoadPlayEnemyData()),
        LoadStep(() => SpriteManager.GetInstance().LoadSpriteAll()),
        LoadStep(() => PlayFabManager.instance.InitUserData("Characters")),
        LoadSceneAdditiveTask(loadSceneName, progress => {
                instance.progressBar.fillAmount = progress;
                instance.txtLoading.text = $"Loading Scene... {Mathf.RoundToInt(progress * 100)}%";
            })
        };

        float totalSteps = loadTasks.Length;
        float completeSteps = 0f;
        instance.progressBar.fillAmount = 0f;

        foreach (var task in loadTasks)
        {
            yield return task;
            completeSteps++;
            instance.progressBar.fillAmount = completeSteps / totalSteps;
            instance.txtLoading.text = $"Loading Data... {Mathf.RoundToInt((completeSteps / totalSteps) * 100)}%";
        }

        yield return new WaitUntil(() => instance.progressBar.fillAmount == 1);
        instance.progressBarBg.gameObject.SetActive(false);
        instance.txtLoading.text = "Press Touch to Start";
        instance.btnLoading.gameObject.SetActive(true);
        instance.btnLoading.onClick.AddListener(() => {
            HideLoginLoading();
            SceneManager.UnloadSceneAsync(unloadSceneName);
        });
    }

    private static IEnumerator LoadStep(Action loadAction)
    {
        yield return null;
        loadAction.Invoke();
    }

    private static IEnumerator LoadSceneAdditiveTask(string loadSceneName, Action<float> onProgress)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone) {
            if (asyncOperation.progress < 0.9f) {
                float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                onProgress?.Invoke(progress);
            }
            else {
                yield return new WaitForSeconds(2f);
                onProgress?.Invoke(1f);
                break;
            }
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;
    }
    #endregion

    #region 씬 로드 - 동기
    public static IEnumerator LoadScene(string loadSceneName, Action onComplete = null)
    {
        yield return new WaitUntil(() => PlayFabManager.instance.CheckLoginSuccess());

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(loadSceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone) {
            asyncOperation.allowSceneActivation = true;
            onComplete?.Invoke();
            yield return Time.deltaTime;
        }
    }
    #endregion
}