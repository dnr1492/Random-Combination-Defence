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
            DebugLogger.Log(player.name + "�� �����ҿ��� �����Խ��ϴ�.");
        });

        btnGetAll.onClick.AddListener(() => {
            var players = playerContainer.GetAll(selectedPlayers[0].name, ReturnDeckPosition);
            playerClickController.CancelObjects();
            DebugLogger.Log(players[0].name + "�� ��ü���� �����ҿ��� ��� �����Խ��ϴ�.");
        });

        btnDelete.onClick.AddListener(() => {
            var player = selectedPlayers[0].GetComponent<PlayerController>();
            playerClickController.CancelObject(player.gameObject);
            playerContainer.Delete(player.name);
            DebugLogger.Log(player.name + "��(��) �����ҿ��� �����ƽ��ϴ�.");
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
