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

    [Header("씬 로드 - 동기")]
    [SerializeField] GameObject loading;
    [SerializeField] Image progressBarBg, progressBar;
    [SerializeField] Button btnLoading;
    [SerializeField] Text txtLoading;

    private void Awake()
    {
        TempEditor();

        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) PlayFabSettings.TitleId = "71A69";

        btnRegist.onClick.AddListener(()=> {
            PlayFabManager.instance.Regist(inputEmail.text, inputPassword.text, inputUsername.text);
        });
        btnLogin.onClick.AddListener(()=> {
            StartCoroutine(LoadScene("LobbyScene"));
            PlayFabManager.instance.Login(inputEmail.text, inputPassword.text);
        });

        progressBar.fillAmount = 0;
        progressBarBg.gameObject.SetActive(false);
        loading.SetActive(false);
        btnLoading.gameObject.SetActive(false);
    }

    private void TempEditor()
    {
        inputEmail.text = "dnr1492@gmail.com";
        inputPassword.text = "wnl881124";
    }

    #region 씬 로드 - 동기
    private IEnumerator LoadScene(string sceneName)
    {
        progressBarBg.gameObject.SetActive(true);
        loading.SetActive(true);

        yield return new WaitUntil(() => PlayFabManager.instance.CheckLoginSuccess());

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        //float timer = 0f;
        float timer = Time.deltaTime * 3;  //60 FPS

        while (!asyncOperation.isDone) {
            if (asyncOperation.progress < 0.9f) {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, asyncOperation.progress, timer);
                txtLoading.text = "Loding...";
            }
            else {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (/*progressBar.fillAmount == 1.0f*/ progressBar.fillAmount >= 0.99f) {
                    progressBarBg.gameObject.SetActive(false);
                    txtLoading.text = "Press Touch to Start";
                    btnLoading.gameObject.SetActive(true);
                    btnLoading.onClick.AddListener(() => asyncOperation.allowSceneActivation = true);
                    break;
                }
            }

            //timer += Time.deltaTime;
            yield return Time.deltaTime;
        }
    }
    #endregion
}
