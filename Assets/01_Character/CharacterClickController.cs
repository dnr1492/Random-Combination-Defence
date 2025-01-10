using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterClickController : MonoBehaviour
{
    [SerializeField] UICharacterInfo uiCharacterInfo;
    [SerializeField] UICharacterRecipe uiCharacterRecipe;
    [SerializeField] CameraController cameraController;
    private Camera curCam;
    private List<GameObject> selectedCharacters = new List<GameObject>();  //������ ĳ���͵�
    private const string TAG_CHARACTER = "Character";

    private bool isOneClick = false;
    private float doubleClickSecond = 0.25f;
    private double timer = 0;
    private string selectedDisplayName;

    public List<GameObject> GetSelectedCharacters() => selectedCharacters;

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) DoubleClick();
    }

    private void DoubleClick()
    {
        if (cameraController.containerCam.gameObject.activeSelf) curCam = cameraController.containerCam;
        else curCam = cameraController.mainCam;

        if (!isOneClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DebugLogger.Log("One Click");
                GameObject clickObj = ClickObject();
                if (clickObj == null) return;
                else SelectSingle(clickObj);
            }
        }

        if (isOneClick && ((Time.time - timer) > doubleClickSecond))
        {
            DebugLogger.Log("Double Click - Time Over");
            isOneClick = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!isOneClick)
            {
                timer = Time.time;
                isOneClick = true;
            }
            else if (isOneClick && ((Time.time - timer) < doubleClickSecond))
            {
                isOneClick = false;

                GameObject clickObj = ClickObject();
                if (clickObj == null) return;

                if (selectedDisplayName != clickObj.name)
                {
                    DebugLogger.Log("One Click Object != Double Click Object");
                    SelectSingle(clickObj);
                    return;
                }
                else
                {
                    DebugLogger.Log("Double Click : " + clickObj.name);
                    SelectRatingAll(clickObj);
                }
            }
        }
    }

    /// <summary>
    /// ��ü Ŭ��
    /// </summary>
    /// <returns></returns>
    private GameObject ClickObject()
    {
        Vector3 worldPos = curCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, curCam.transform.forward, Mathf.Infinity);

        if (hit.collider == null || !hit.collider.CompareTag(TAG_CHARACTER)) return null;
        GameObject target = hit.collider.gameObject;
        return target;
    }

    /// <summary>
    /// ��ü ���� ����
    /// </summary>
    /// <param name="clickObj"></param>
    private void SelectSingle(GameObject clickObj)
    {
        CancelObjects();

        selectedDisplayName = clickObj.name;
        selectedCharacters.Add(clickObj);
        clickObj.GetComponent<CharacterController>().InitClick(this, uiCharacterInfo, uiCharacterRecipe, true);
        DebugLogger.Log("�̸��� " + selectedDisplayName + "�� ��ü�� ���� ����");
    }

    /// <summary>
    /// ��ü ��ü ����
    /// </summary>
    /// <param name="clickObj"></param>
    private void SelectRatingAll(GameObject clickObj)
    {
        CancelObjects();

        string displayName = clickObj.name;
        List<GameObject> characters = CharacterGenerator.ExistingCharacters;
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].name == displayName)
            {
                CharacterController characterController = characters[i].GetComponent<CharacterController>();
                if (characterController.CheckMoving()) continue;  //�̵� ���� ĳ���ʹ� ���� ��󿡼� ����
                selectedCharacters.Add(characters[i]);
                characterController.InitClick(this, uiCharacterInfo, uiCharacterRecipe, true);
                DebugLogger.Log("�̸��� " + displayName + "�� ��ü�� ��ü ����");
            }
        }
    }

    /// <summary>
    /// ��ü ���� ���� ����
    /// </summary>
    /// <param name="cancelObj"></param>
    public void CancelObject(GameObject cancelObj)
    {
        for (int i = 0; i < selectedCharacters.Count; i++)
        {
            if (selectedCharacters[i] == cancelObj)
            {
                selectedCharacters[i].GetComponent<CharacterController>().InitClick(this, uiCharacterInfo, uiCharacterRecipe, false);
                selectedCharacters.Remove(selectedCharacters[i]);
            }
        }
        selectedDisplayName = null;
    }

    /// <summary>
    /// ��ü ��ü ���� ����
    /// </summary>
    public void CancelObjects()
    {
        for (int i = 0; i < selectedCharacters.Count; i++) selectedCharacters[i].GetComponent<CharacterController>().InitClick(this, uiCharacterInfo, uiCharacterRecipe, false);
        selectedCharacters.RemoveRange(0, selectedCharacters.Count);
        selectedDisplayName = null;
    }
}