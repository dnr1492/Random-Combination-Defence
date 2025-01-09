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
        var selectedPlayers = playerClickController.GetSelectedPlayers();

        btnAdd.onClick.AddListener(() => {    
            for (int i = 0; i < selectedPlayers.Count; i++) {
                var player = selectedPlayers[i].GetComponent<PlayerController>();
                playerContainer.Add(player);
                DebugLogger.Log(player.name + "이(가) 보관소로 이동했습니다.");
            }
            playerClickController.CancelObjects();
        });

        btnMoveContainerScreen.onClick.AddListener(() => {
            cameraController.OnContainerCamera();
            UIScreenController.OnContainerScreen();
        });
    }
}