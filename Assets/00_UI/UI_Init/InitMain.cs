using System.Collections;
using System.Collections.Generic;
using PlayFab;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitMain : MonoBehaviour
{
    [Header("ȸ������ �� �α���")]
    [SerializeField] InputField inputEmail, inputPassword, inputUsername;
    [SerializeField] Button btnRegist, btnLogin;

    //[Header("�� �ε� - ����")]
    //[SerializeField] GameObject loading;
    //[SerializeField] Image progressBarBg, progressBar;
    //[SerializeField] Button btnLoading;
    //[SerializeField] Text txtLoading;

    private void Awake()
    {
        TempEditor();

        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) PlayFabSettings.TitleId = "71A69";

        btnRegist.onClick.AddListener(()=> {
            PlayFabManager.instance.Regist(inputEmail.text, inputPassword.text, inputUsername.text);
        });
        btnLogin.onClick.AddListener(()=> {
            StartCoroutine(LoadingManager.LoadScene("LobbyScene"));
            //StartCoroutine(LoadScene("LobbyScene"));
            PlayFabManager.instance.Login(inputEmail.text, inputPassword.text);
        });

        //progressBar.fillAmount = 0;
        //progressBarBg.gameObject.SetActive(false);
        //loading.SetActive(false);
        //btnLoading.gameObject.SetActive(false);
    }

    private void TempEditor()
    {
        inputEmail.text = "dnr1492@gmail.com";
        inputPassword.text = "wnl881124";
    }

    //#region �� �ε� - ����
    //private IEnumerator LoadScene(string loadSceneName)
    //{
    //    // ===== InitScene�� �ı��Ǽ� ������ �Ұ��� �����Ƿ� �ش� �Լ� �� �������� LoadingManager�� �Űܼ� ����ϱ� ===== //
    //    // ===== InitScene�� �ı��Ǽ� ������ �Ұ��� �����Ƿ� �ش� �Լ� �� �������� LoadingManager�� �Űܼ� ����ϱ� ===== //
    //    // ===== InitScene�� �ı��Ǽ� ������ �Ұ��� �����Ƿ� �ش� �Լ� �� �������� LoadingManager�� �Űܼ� ����ϱ� ===== //

    //    // �ε� UI Ȱ��ȭ
    //    progressBarBg.gameObject.SetActive(true);
    //    loading.SetActive(true);

    //    // PlayFab �α��� ���� ���
    //    yield return new WaitUntil(() => PlayFabManager.instance.CheckLoginSuccess());

    //    // �� �ε� ����
    //    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(loadSceneName);

    //    float timer = 0f;
    //    progressBar.fillAmount = 0f;

    //    // �� �ε� ����� ������Ʈ
    //    while (!asyncOperation.isDone)
    //    {
    //        // 0%~70% �ε� ����� �ݿ�
    //        if (asyncOperation.progress < 0.7f)
    //        {
    //            timer += Time.deltaTime * 3; // ���� �����
    //            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, asyncOperation.progress, timer);
    //            txtLoading.text = "Loading...";
    //        }
    //        else
    //        {
    //            // 70%���� �� �ε� �Ϸ� ���� ���
    //            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 0.7f, timer);
    //            txtLoading.text = "Scene Init...";
    //            break;
    //        }

    //        yield return null;
    //    }

    //    // �� �ε� �Ϸ�, ��Ʈ ������Ʈ �ʱ�ȭ
    //    var loadedScene = SceneManager.GetSceneByName(loadSceneName);
    //    GameObject targetGo = null;
    //    yield return new WaitUntil(() => loadedScene.IsValid());
    //    if (loadedScene.IsValid())
    //    {
    //        var rootGameObjects = loadedScene.GetRootGameObjects();

    //        float totalObjects = rootGameObjects.Length;
    //        float initializedObjects = 0f;

    //        foreach (var gameObject in rootGameObjects)
    //        {
    //            Debug.Log($"Initializing: {gameObject.name}");
    //            targetGo = gameObject;
    //            targetGo.SetActive(false);

    //            // �ʱ�ȭ �۾�
    //            yield return null; // ������ ��� (�񵿱� ó�� ����)
    //            initializedObjects++;

    //            // �ʱ�ȭ ����� �ݿ� (70%~100%)
    //            progressBar.fillAmount = 0.7f + (0.1f * (initializedObjects / totalObjects));
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError($"Scene '{loadSceneName}' failed to load!");
    //    }

    //    // �ε� �Ϸ� ����
    //    progressBar.fillAmount = 1f;
    //    progressBarBg.gameObject.SetActive(false);
    //    txtLoading.text = "Press Touch to Start";
    //    btnLoading.gameObject.SetActive(true);

    //    // ��ư Ŭ�� ���
    //    bool isButtonClicked = false;
    //    btnLoading.onClick.AddListener(() => isButtonClicked = true);
    //    yield return new WaitUntil(() => isButtonClicked);

    //    // �ε� UI ��Ȱ��ȭ
    //    loading.SetActive(false);
    //    targetGo.SetActive(true);
    //}
    //#endregion
}
