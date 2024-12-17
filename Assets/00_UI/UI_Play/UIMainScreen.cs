using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainScreen : MonoBehaviour
{
    [SerializeField] PlayerClickController playerClickController;
    [SerializeField] PlayerContainer playerContainer;
    [SerializeField] PlayerGenerator playerGenerator;
    [SerializeField] CameraController cameraController;

    [SerializeField] Button btnAdd, btnMoveContainerScreen;

    private void Awake()
    {
        btnAdd.onClick.AddListener(() => {
            var players = playerClickController.GetSelectedPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i].GetComponent<PlayerController>();
                playerContainer.Add(player);
                Debug.Log(player.name + "이(가) 보관소로 이동했습니다.");
            }
            playerClickController.CancelObjects();
        });

        btnMoveContainerScreen.onClick.AddListener(() => {
            cameraController.OnContainerCamera();
            UIScreenController.OnContainerScreen();
        });
    }
}