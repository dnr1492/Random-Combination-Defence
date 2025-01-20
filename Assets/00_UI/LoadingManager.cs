using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    private static LoadingManager instance;
    private static GameObject loading;

    private readonly float speed = 1f;

    [Header("�α��� �ε� ȭ��")]
    [SerializeField] GameObject initLoadingPrefab;
    private static Image progressBarBg, progressBar;
    private static Button btnLoading;
    private static Text txtLoading;

    [Header("��Ÿ �ε� ȭ��")]
    [SerializeField] GameObject loadingPrefab;
    private static Image loadingIcon;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            initLoadingPrefab.SetActive(false);
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
    /// �ε� ȭ�� ǥ��
    /// </summary>
    public static void ShowLoading(bool init = false)
    {
        if (instance == null) return;

        if (loading == null) {
            if (init) {
                loading = Instantiate(instance.initLoadingPrefab);
                loading.SetActive(false);
                if (progressBarBg == null) progressBarBg = loading.transform.Find("progressBarBg").GetComponent<Image>();
                if (progressBar == null) progressBar = loading.transform.Find("progressBarBg/progressBar").GetComponent<Image>();
                if (btnLoading == null) btnLoading = loading.transform.Find("btnLoading").GetComponent<Button>();
                if (txtLoading == null) txtLoading = loading.transform.Find("btnLoading/txtLoading").GetComponent<Text>();
                progressBar.fillAmount = 0;
                progressBarBg.gameObject.SetActive(false);
                btnLoading.gameObject.SetActive(false);
                txtLoading.text = string.Empty;
            }
            else {
                loading = Instantiate(instance.loadingPrefab);
                loading.SetActive(true);
                if (loadingIcon == null) loadingIcon = loading.transform.Find("loadingIcon").GetComponent<Image>();
                loadingIcon.fillAmount = 0;
            }
            DontDestroyOnLoad(loading);
        }
    }

    /// <summary>
    /// �ε� ȭ�� �����
    /// </summary>
    public static void HideLoading()
    {
        if (loading != null) {
            Destroy(loading);
            loading = null;
        }
    }

    public static IEnumerator LoadScene(string loadSceneName)
    {
        ShowLoading(true);

        // �ε� UI Ȱ��ȭ
        progressBarBg.gameObject.SetActive(true);
        loading.SetActive(true);

        // PlayFab �α��� ���� ���
        yield return new WaitUntil(() => PlayFabManager.instance.CheckLoginSuccess());

        // �� �ε� ����
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(loadSceneName);

        float timer = 0f;
        progressBar.fillAmount = 0f;

        // �� �ε� ����� ������Ʈ
        while (!asyncOperation.isDone)
        {
            // 0%~70% �ε� ����� �ݿ�
            if (asyncOperation.progress < 0.7f)
            {
                timer += Time.deltaTime * 3; // ���� �����
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, asyncOperation.progress, timer);
                txtLoading.text = "Loading...";
            }
            else
            {
                // 70%���� �� �ε� �Ϸ� ���� ���
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 0.7f, timer);
                txtLoading.text = "Scene Init...";
                break;
            }

            yield return null;
        }

        // �� �ε� �Ϸ�, ��Ʈ ������Ʈ �ʱ�ȭ
        var loadedScene = SceneManager.GetSceneByName(loadSceneName);
        GameObject targetGo = null;
        yield return new WaitUntil(() => loadedScene.IsValid());
        if (loadedScene.IsValid())
        {
            var rootGameObjects = loadedScene.GetRootGameObjects();

            float totalObjects = rootGameObjects.Length;
            float initializedObjects = 0f;

            foreach (var gameObject in rootGameObjects)
            {
                Debug.Log($"Initializing: {gameObject.name}");
                targetGo = gameObject;
                targetGo.SetActive(false);

                // �ʱ�ȭ �۾�
                yield return null; // ������ ��� (�񵿱� ó�� ����)
                initializedObjects++;

                // �ʱ�ȭ ����� �ݿ� (70%~100%)
                progressBar.fillAmount = 0.7f + (0.1f * (initializedObjects / totalObjects));
            }
        }
        else
        {
            Debug.LogError($"Scene '{loadSceneName}' failed to load!");
        }

        // �ε� �Ϸ� ����
        progressBar.fillAmount = 1f;
        progressBarBg.gameObject.SetActive(false);
        txtLoading.text = "Press Touch to Start";
        btnLoading.gameObject.SetActive(true);

        // ��ư Ŭ�� ���
        bool isButtonClicked = false;
        btnLoading.onClick.AddListener(() => isButtonClicked = true);
        yield return new WaitUntil(() => isButtonClicked);

        // �ε� UI ��Ȱ��ȭ
        loading.SetActive(false);
        targetGo.SetActive(true);
    }
}
