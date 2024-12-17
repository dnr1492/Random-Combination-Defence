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

    // ======================= 씬이 로딩이 완료되기 전 클릭 또는 스페이스 연타시 중복으로 로딩되는 버그 수정하기 !!! ==========================
    // ======================= 씬이 로딩이 완료되기 전 클릭 또는 스페이스 연타시 중복으로 로딩되는 버그 수정하기 !!! ==========================
    // ======================= 씬이 로딩이 완료되기 전 클릭 또는 스페이스 연타시 중복으로 로딩되는 버그 수정하기 !!! ==========================
    [Header("씬 로드 - 동기")]
    [SerializeField] GameObject loading;
    [SerializeField] Image progressBar;
    [SerializeField] Text txtLoading;
    [SerializeField] Button btnLoading;

    private void Awake()
    {
        TempEditor();

        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) PlayFabSettings.TitleId = "71A69";

        btnRegist.onClick.AddListener(()=> {
            PlayFabManager.instance.Regist(inputEmail.text, inputPassword.text, inputUsername.text);
        });
        btnLogin.onClick.AddListener(()=> {
            PlayFabManager.instance.Login(inputEmail.text, inputPassword.text);
            StartCoroutine(LoadScene("LobbyScene"));
        });

        progressBar.fillAmount = 0;
        loading.SetActive(false);
    }

    private void TempEditor()
    {
        inputEmail.text = "dnr1492@gmail.com";
        inputPassword.text = "wnl881124";
    }

    #region 씬 로드 - 동기
    private IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitUntil(() => PlayFabManager.instance.CheckLoginSuccess());

        loading.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        float timer = 0f;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, asyncOperation.progress, timer);
                txtLoading.text = "Loding...";
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    txtLoading.text = "Press Touch to Start";
                    btnLoading.onClick.AddListener(() => asyncOperation.allowSceneActivation = true); 
                }
            }

            timer += Time.deltaTime;
            yield return Time.deltaTime;
        }
    }
    #endregion
}
