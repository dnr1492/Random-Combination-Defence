using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] Text title;
    [SerializeField] Button btnQuitToLobby, btnRestartGame;

    private readonly string strGameOver = "���� ����";

    public void Init(UnityAction restartGameAction, UnityAction<int> setUIFastForwardAction)
    {
        title.text = strGameOver;
        Time.timeScale = 0;

        btnQuitToLobby.onClick.AddListener(() => {
            StartCoroutine(LoadingManager.LoadSceneAdditive("LobbyScene", "PlayScene"));
            setUIFastForwardAction((int)FastForward.X1);
        });

        btnRestartGame.onClick.AddListener(() => {
            // ===== ������ ��� ���� ===== //
            // ===== ������ ��� ���� ===== //
            // ===== ������ ��� ���� ===== //
            setUIFastForwardAction((int)FastForward.X1);
            restartGameAction();
            Destroy(gameObject);
        });
    }
}
