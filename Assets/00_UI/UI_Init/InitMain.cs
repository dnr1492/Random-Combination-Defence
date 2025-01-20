using PlayFab;
using UnityEngine;
using UnityEngine.UI;

public class InitMain : MonoBehaviour
{
    [Header("회원가입 및 로그인")]
    [SerializeField] InputField inputEmail, inputPassword, inputUsername;
    [SerializeField] Button btnRegist, btnLogin;

    private void Awake()
    {
        TempEditor();

        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) PlayFabSettings.TitleId = "71A69";

        btnRegist.onClick.AddListener(()=> {
            PlayFabManager.instance.Regist(inputEmail.text, inputPassword.text, inputUsername.text);
        });
        btnLogin.onClick.AddListener(()=> {
            PlayFabManager.instance.Login(inputEmail.text, inputPassword.text);
            StartCoroutine(LoadingManager.LoadSceneAdditive("LobbyScene", "InitScene"));
        });
    }

    private void TempEditor()
    {
        inputEmail.text = "dnr1492@gmail.com";
        inputPassword.text = "wnl881124";
    }
}
