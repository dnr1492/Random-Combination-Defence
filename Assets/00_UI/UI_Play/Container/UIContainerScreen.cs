using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContainerScreen : MonoBehaviour
{
    [SerializeField] CharacterClickController characterClickController;
    [SerializeField] CharacterContainer characterContainer;
    [SerializeField] CharacterGenerator characterGenerator;
    [SerializeField] CameraController cameraController;

    [SerializeField] Button btnGet, btnGetAll, btnDelete, btnMovePlayScreen;

    private void Awake()
    {
        var selectedCharacters = characterClickController.GetSelectedCharacters();

        btnGet.onClick.AddListener(() => {
            var character = characterContainer.Get(selectedCharacters[0].name, ReturnDeckPosition);
            characterClickController.CancelObject(character.gameObject);
            DebugLogger.Log(character.name + "�� �����ҿ��� �����Խ��ϴ�.");
        });

        btnGetAll.onClick.AddListener(() => {
            var characters = characterContainer.GetAll(selectedCharacters[0].name, ReturnDeckPosition);
            characterClickController.CancelObjects();
            DebugLogger.Log(characters[0].name + "�� ��ü���� �����ҿ��� ��� �����Խ��ϴ�.");
        });

        btnDelete.onClick.AddListener(() => {
            var character = selectedCharacters[0].GetComponent<CharacterController>();
            characterClickController.CancelObject(character.gameObject);
            characterContainer.Delete(character.name);
            DebugLogger.Log(character.name + "��(��) �����ҿ��� �����ƽ��ϴ�.");
        });

        btnMovePlayScreen.onClick.AddListener(() => {
            cameraController.OffContainerCamera();
            UIScreenController.OnMainScreen();
        });
    }

    private void ReturnDeckPosition(GameObject character)
    {
        characterGenerator.Sort(character);
    }
}
