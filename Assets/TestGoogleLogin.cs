using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class TestGoogleLogin : MonoBehaviour
{
    [SerializeField] Button btnLogin;
    [SerializeField] Text txt;

    private void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        btnLogin.onClick.AddListener(OnClickGoogleLogin);
    }

    /// <summary>
    /// 구글 로그인
    /// </summary>
    public void OnClickGoogleLogin()
    {
        //현재 사용자가 인증되었는지 확인
        if (Social.localUser.authenticated) return;

        Social.localUser.Authenticate((bool success) =>
        {
            //로그인에 성공
            if (success) txt.text = Social.localUser.userName + "님 환영합니다" + "\n" + "당신의 ID는 " + Social.localUser.id + "입니다";
            //로그인에 실패
            else txt.text = "로그인에 실패했습니다";
        });
    }
}
