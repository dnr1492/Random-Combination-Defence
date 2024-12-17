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
        btnGet.onClick.AddListener(() => {
            var players = playerClickController.GetSelectedPlayers();
            var player = playerContainer.Get(players[0].name, ReturnDeckPosition);
            playerClickController.CancelObject(player.gameObject);
            Debug.Log(player.name + "�� �����ҿ��� �����Խ��ϴ�.");
        });

        btnGetAll.onClick.AddListener(() => {
            var players = playerClickController.GetSelectedPlayers();
            var tempPlayers = playerContainer.GetAll(players[0].name, ReturnDeckPosition);
            playerClickController.CancelObjects();
            Debug.Log(tempPlayers[0].name + "�� ��ü���� �����ҿ��� ��� �����Խ��ϴ�.");
        });

        btnDelete.onClick.AddListener(() => {
            var players = playerClickController.GetSelectedPlayers();
            var player = players[0].GetComponent<PlayerController>();
            playerClickController.CancelObject(player.gameObject);
            playerContainer.Delete(player.name);
            Debug.Log(player.name + "��(��) �����ҿ��� �����ƽ��ϴ�.");
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
