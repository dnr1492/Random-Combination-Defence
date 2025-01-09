using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContainerScreen : MonoBehaviour
{
    [SerializeField] PlayerClickController playerClickController;
    [SerializeField] PlayerContainer playerContainer;
    [SerializeField] PlayerGenerator playerGenerator;
    [SerializeField] CameraController cameraController;

    [SerializeField] Button btnGet, btnGetAll, btnDelete, btnMovePlayScreen;

    private void Awake()
    {
        var selectedPlayers = playerClickController.GetSelectedPlayers();

        btnGet.onClick.AddListener(() => {
            var player = playerContainer.Get(selectedPlayers[0].name, ReturnDeckPosition);
            playerClickController.CancelObject(player.gameObject);
            DebugLogger.Log(player.name + "를 보관소에서 가져왔습니다.");
        });

        btnGetAll.onClick.AddListener(() => {
            var players = playerContainer.GetAll(selectedPlayers[0].name, ReturnDeckPosition);
            playerClickController.CancelObjects();
            DebugLogger.Log(players[0].name + "의 개체군을 보관소에서 모두 가져왔습니다.");
        });

        btnDelete.onClick.AddListener(() => {
            var player = selectedPlayers[0].GetComponent<PlayerController>();
            playerClickController.CancelObject(player.gameObject);
            playerContainer.Delete(player.name);
            DebugLogger.Log(player.name + "이(가) 보관소에서 삭제됐습니다.");
        });

        btnMovePlayScreen.onClick.AddListener(() => {
            cameraController.OffContainerCamera();
            UIScreenController.OnMainScreen();
        });
    }

    private void ReturnDeckPosition(GameObject player)
    {
        playerGenerator.Sort(player);
    }
}
