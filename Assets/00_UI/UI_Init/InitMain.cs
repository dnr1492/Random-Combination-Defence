using System.Collections;
using System.Collections.Generic;
using PlayFab;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitMain : MonoBehaviour
{
    [Header("회원가입 및 로그인")]
    [SerializeField] InputField inputEmail, inputPassword, inputUsername;
    [SerializeField] Button btnRegist, btnLogin;

    //[Header("씬 로드 - 동기")]
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

    //#region 씬 로드 - 동기
    //private IEnumerator LoadScene(string loadSceneName)
    //{
    //    // ===== InitScene이 파괴되서 접근이 불가능 해지므로 해당 함수 및 변수들을 LoadingManager로 옮겨서 사용하기 ===== //
    //    // ===== InitScene이 파괴되서 접근이 불가능 해지므로 해당 함수 및 변수들을 LoadingManager로 옮겨서 사용하기 ===== //
    //    // ===== InitScene이 파괴되서 접근이 불가능 해지므로 해당 함수 및 변수들을 LoadingManager로 옮겨서 사용하기 ===== //

    //    // 로딩 UI 활성화
    //    progressBarBg.gameObject.SetActive(true);
    //    loading.SetActive(true);

    //    // PlayFab 로그인 성공 대기
    //    yield return new WaitUntil(() => PlayFabManager.instance.CheckLoginSuccess());

    //    // 씬 로드 시작
    //    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(loadSceneName);

    //    float timer = 0f;
    //    progressBar.fillAmount = 0f;

    //    // 씬 로드 진행률 업데이트
    //    while (!asyncOperation.isDone)
    //    {
    //        // 0%~70% 로딩 진행률 반영
    //        if (asyncOperation.progress < 0.7f)
    //        {
    //            timer += Time.deltaTime * 3; // 빠른 진행률
    //            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, asyncOperation.progress, timer);
    //            txtLoading.text = "Loading...";
    //        }
    //        else
    //        {
    //            // 70%에서 씬 로드 완료 상태 대기
    //            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 0.7f, timer);
    //            txtLoading.text = "Scene Init...";
    //            break;
    //        }

    //        yield return null;
    //    }

    //    // 씬 로드 완료, 루트 오브젝트 초기화
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

    //            // 초기화 작업
    //            yield return null; // 프레임 대기 (비동기 처리 가능)
    //            initializedObjects++;

    //            // 초기화 진행률 반영 (70%~100%)
    //            progressBar.fillAmount = 0.7f + (0.1f * (initializedObjects / totalObjects));
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError($"Scene '{loadSceneName}' failed to load!");
    //    }

    //    // 로딩 완료 상태
    //    progressBar.fillAmount = 1f;
    //    progressBarBg.gameObject.SetActive(false);
    //    txtLoading.text = "Press Touch to Start";
    //    btnLoading.gameObject.SetActive(true);

    //    // 버튼 클릭 대기
    //    bool isButtonClicked = false;
    //    btnLoading.onClick.AddListener(() => isButtonClicked = true);
    //    yield return new WaitUntil(() => isButtonClicked);

    //    // 로딩 UI 비활성화
    //    loading.SetActive(false);
    //    targetGo.SetActive(true);
    //}
    //#endregion
}
