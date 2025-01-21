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

    public void Init(UnityAction restartGameAction)
    {
        title.text = strGameOver;
        Time.timeScale = 0;

        btnQuitToLobby.onClick.AddListener(() => {
            StartCoroutine(LoadingManager.LoadSceneAdditive("LobbyScene", "PlayScene_Test", () => Time.timeScale = 1));
        });

        btnRestartGame.onClick.AddListener(() => {
            // ===== ������ ��� ���� ===== //
            // ===== ������ ��� ���� ===== //
            // ===== ������ ��� ���� ===== //
            Time.timeScale = 1;
            restartGameAction();
            Destroy(gameObject);
        });
    }
}
