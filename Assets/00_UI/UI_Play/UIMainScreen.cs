using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainScreen : MonoBehaviour
{
    [SerializeField] CharacterClickController characterClickController;
    [SerializeField] CharacterContainer characterContainer;
    [SerializeField] CameraController cameraController;

    [SerializeField] Button btnAdd, btnMoveContainer;

    private void Awake()
    {
        var selectedCharacters = characterClickController.GetSelectedCharacters();
        btnAdd.onClick.AddListener(() => {    
            for (int i = 0; i < selectedCharacters.Count; i++) {
                var character = selectedCharacters[i].GetComponent<CharacterController>();
                characterContainer.Add(character);
                DebugLogger.Log(character.name + "��(��) �����ҷ� �̵��߽��ϴ�.");
            }
            characterClickController.CancelObjects();
        });

        btnMoveContainer.onClick.AddListener(() => {
            cameraController.OnContainerCamera();
            UIScreenController.OnContainerScreen();
        });
    }
}