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
    /// ���� �α���
    /// </summary>
    public void OnClickGoogleLogin()
    {
        //���� ����ڰ� �����Ǿ����� Ȯ��
        if (Social.localUser.authenticated) return;

        Social.localUser.Authenticate((bool success) =>
        {
            //�α��ο� ����
            if (success) txt.text = Social.localUser.userName + "�� ȯ���մϴ�" + "\n" + "����� ID�� " + Social.localUser.id + "�Դϴ�";
            //�α��ο� ����
            else txt.text = "�α��ο� �����߽��ϴ�";
        });
    }
}
