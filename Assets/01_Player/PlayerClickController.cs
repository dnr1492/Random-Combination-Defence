using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerClickController : MonoBehaviour
{
    [SerializeField] UICharacterInfo uiCharacterInfo;
    [SerializeField] UICharacterRecipe uiCharacterRecipe;
    [SerializeField] CameraController cameraController;
    private Camera curCam;
    private List<GameObject> selectedPlayers = new List<GameObject>();  //선택한 플레이어들
    private const string TAG_PLAYER = "Player";

    private bool isOneClick = false;
    private float doubleClickSecond = 0.25f;
    private double timer = 0;
    private string selectedName;

    public List<GameObject> GetSelectedPlayers() => selectedPlayers;

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
                Debug.Log("One Click");
                GameObject clickObj = ClickObject();
                if (clickObj == null) return;
                else SelectSingle(clickObj);
            }
        }

        if (isOneClick && ((Time.time - timer) > doubleClickSecond))
        {
            Debug.Log("Double Click - Time Over");
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

                if (selectedName != clickObj.name)
                {
                    Debug.Log("One Click Object != Double Click Object");
                    SelectSingle(clickObj);
                    return;
                }
                else
                {
                    Debug.Log("Double Click : " + clickObj.name);
                    SelectRatingAll(clickObj);
                }
            }
        }
    }

    /// <summary>
    /// 개체 클릭
    /// </summary>
    /// <returns></returns>
    private GameObject ClickObject()
    {
        Vector3 worldPos = curCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, curCam.transform.forward, Mathf.Infinity);

        if (hit.collider == null || !hit.collider.CompareTag(TAG_PLAYER)) return null;
        GameObject target = hit.collider.gameObject;
        return target;
    }

    /// <summary>
    /// 개체 단일 선택
    /// </summary>
    /// <param name="clickObj"></param>
    private void SelectSingle(GameObject clickObj)
    {
        CancelObjects();

        selectedName = clickObj.name;
        selectedPlayers.Add(clickObj);
        clickObj.GetComponent<PlayerController>().InitClick(this, uiCharacterInfo, uiCharacterRecipe, true);
        Debug.Log("이름이 " + selectedName + "인 개체군 단일 선택");
    }

    /// <summary>
    /// 개체 전체 선택
    /// </summary>
    /// <param name="clickObj"></param>
    private void SelectRatingAll(GameObject clickObj)
    {
        CancelObjects();

        string name = clickObj.name;
        List<GameObject> players = PlayerGenerator.ExistingPlayers;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].name == name)
            {
                PlayerController playerController = players[i].GetComponent<PlayerController>();
                if (playerController.CheckMoving()) continue;  //이동 중인 플레이어는 선택 대상에서 제외
                selectedPlayers.Add(players[i]);
                playerController.InitClick(this, uiCharacterInfo, uiCharacterRecipe, true);
                Debug.Log("이름이 " + name + "인 개체군 전체 선택");
            }
        }
    }

    /// <summary>
    /// 개체 단일 선택 해제
    /// </summary>
    /// <param name="cancelObj"></param>
    public void CancelObject(GameObject cancelObj)
    {
        for (int i = 0; i < selectedPlayers.Count; i++)
        {
            if (selectedPlayers[i] == cancelObj)
            {
                selectedPlayers[i].GetComponent<PlayerController>().InitClick(this, uiCharacterInfo, uiCharacterRecipe, false);
                selectedPlayers.Remove(selectedPlayers[i]);
            }
        }
        selectedName = null;
    }

    /// <summary>
    /// 개체 전체 선택 해제
    /// </summary>
    public void CancelObjects()
    {
        for (int i = 0; i < selectedPlayers.Count; i++) selectedPlayers[i].GetComponent<PlayerController>().InitClick(this, uiCharacterInfo, uiCharacterRecipe, false);
        selectedPlayers.RemoveRange(0, selectedPlayers.Count);
        selectedName = null;
    }
}