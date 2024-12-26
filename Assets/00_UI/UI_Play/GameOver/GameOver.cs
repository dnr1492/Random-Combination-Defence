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
    private readonly string strGameOver = "게임 오버";

    private void Awake()
    {
        //title.text = strGameOver;

        //btnQuitToLobby.onClick.AddListener(() => {
        //    Time.timeScale = 1;
        //    // ===== 데이터 매니저에서 버그 뜸 ===== //
        //    SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
        //});

        //btnRestartGame.onClick.AddListener(() => {
        //    // ===== 광고보기 ===== //
        //    // ===== 몬스터 초기화 후 해당 웨이브 다시 시작 ===== //
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
            // ===== 데이터 매니저에서 버그 뜸 ===== //
            SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
        });

        btnRestartGame.onClick.AddListener(() => {
            // ===== 광고보기 ===== //
            // ===== 몬스터 초기화 후 해당 웨이브 다시 시작 ===== //
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
        // ===== foreach문이 제대로 안 돔 ===== //
        foreach (var enemy in enemys) {
            enemys.Remove(enemy);
            Destroy(enemy);
        }
        uiPlay.SetUI_EnemyCount(enemys.Count);
    }
}
