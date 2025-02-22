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

    private readonly string strGameOver = "게임 오버";

    public void Init(UnityAction restartGameAction, UnityAction<int> setUIFastForwardAction)
    {
        title.text = strGameOver;
        Time.timeScale = 0;

        btnQuitToLobby.onClick.AddListener(() => {
            StartCoroutine(LoadingManager.LoadSceneAdditive("LobbyScene", "PlayScene"));
            setUIFastForwardAction((int)FastForward.X1);
        });

        btnRestartGame.onClick.AddListener(() => {
            // ===== 광고보기 기능 구현 ===== //
            // ===== 광고보기 기능 구현 ===== //
            // ===== 광고보기 기능 구현 ===== //
            setUIFastForwardAction((int)FastForward.X1);
            restartGameAction();
            Destroy(gameObject);
        });
    }
}
