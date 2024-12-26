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

    private UIPlay uiPlay;
    private readonly string strGameOver = "���� ����";

    private void Awake()
    {
        //title.text = strGameOver;

        //btnQuitToLobby.onClick.AddListener(() => {
        //    Time.timeScale = 1;
        //    // ===== ������ �Ŵ������� ���� �� ===== //
        //    SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
        //});

        //btnRestartGame.onClick.AddListener(() => {
        //    // ===== ������ ===== //
        //    // ===== ���� �ʱ�ȭ �� �ش� ���̺� �ٽ� ���� ===== //
        //    Time.timeScale = 1;
        //    RestartGame();
        //    Destroy(gameObject);
        //});

        //Time.timeScale = 0;
    }

    public void Init(UIPlay uiPlay)
    {
        title.text = strGameOver;

        btnQuitToLobby.onClick.AddListener(() => {
            Time.timeScale = 1;
            // ===== ������ �Ŵ������� ���� �� ===== //
            SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
        });

        btnRestartGame.onClick.AddListener(() => {
            // ===== ������ ===== //
            // ===== ���� �ʱ�ȭ �� �ش� ���̺� �ٽ� ���� ===== //
            Time.timeScale = 1;
            RestartGame();
            Destroy(gameObject);
        });

        Time.timeScale = 0;

        this.uiPlay = uiPlay;
    }

    private void RestartGame()
    {
        var enemys = EnemyGenerator.ExistingEnemys;
        // ===== foreach���� ����� �� �� ===== //
        foreach (var enemy in enemys) {
            enemys.Remove(enemy);
            Destroy(enemy);
        }
        uiPlay.SetUI_EnemyCount(enemys.Count);
    }
}
