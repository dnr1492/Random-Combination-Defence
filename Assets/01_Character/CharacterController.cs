using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CharacterController : MonoBehaviour
{
    private CharacterClickController characterClickController;
    private CameraController cameraController;
    private CharacterInfo curCharacterInfo;
    private GameObject selectingGo;
    private Tilemap mainTilemap;
    private Vector3 movePos;
    private bool isSelected = false;
    private bool isWaited = false;
    private bool isArrived = false;
    private bool isMoving = false;
    private float stoppingDistance;  //도착 판단 임계선

    private Animator animator;
    private const string IDLE = "Idle";
    private const string RUN = "Run";

    public void Init(CameraController cameraController, Tilemap mainTilemap)
    {
        this.cameraController = cameraController;
        this.mainTilemap = mainTilemap;
    }

    public void InitClick(CharacterClickController characterClickController, UICharacterInfo uiCharacterInfo, UICharacterRecipe uiCharacterRecipe, bool isSelected)
    {
        this.characterClickController = characterClickController;
        this.isSelected = isSelected;

        //캐릭터를 선택하자마자 움직이는 것을 방지
        if (gameObject.activeSelf) StartCoroutine(WaitOneFrame());  

        uiCharacterInfo.gameObject.SetActive(isSelected);
        uiCharacterRecipe.gameObject.SetActive(isSelected);

        if (isSelected) {
            uiCharacterInfo.Init(curCharacterInfo);
            uiCharacterRecipe.SetReferenceRecipe(characterClickController.GetSelectedCharacters()[0].name);
            Select(isSelected);
        }
        else Select(isSelected);
    }

    private void Awake()
    {
        curCharacterInfo = InfoManager.GetInstance().LoadCharacterInfo(gameObject.name);

        selectingGo = transform.Find("Selecting").gameObject;
        Select(isSelected);

        movePos = transform.position;
        //animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (cameraController.mainCam == null) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (isSelected && !isMoving && isWaited) SetMove();
    }

    private bool CheckTilemap()
    {
        Vector2 worldPosition = cameraController.mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = mainTilemap.WorldToCell(worldPosition);
        if (mainTilemap.HasTile(gridPosition)) return true;
        else return false;
    }

    private void SetMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!CheckTilemap()) return;

            isArrived = false;
            movePos = cameraController.mainCam.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(Move());
        }
    }

    private IEnumerator Move()
    {
        float timer = 0f;

        while (true)
        {
            if (Vector2.Distance(transform.position, movePos) > stoppingDistance)
            {
                DebugLogger.Log("이동 중");
                transform.position = Vector2.MoveTowards(transform.position, new Vector3(movePos.x, movePos.y, transform.position.z), curCharacterInfo.moveSpeed * Time.deltaTime);
                isMoving = true;
                timer += Time.deltaTime;
                //animator.SetBool(RUN, isMoving);
                
                if (timer >= 0.5f) {
                    DebugLogger.Log("일정 시간이 경과하여 stoppingDistance를 증가");
                    stoppingDistance += 0.1f;
                    timer = 0f;
                }
            }
            else
            {
                isArrived = true;
                isMoving = false;
                //animator.SetBool(IDLE, isMoving);
            }

            if (isArrived)
            {
                DebugLogger.Log("이동 완료 후 자동으로 선택 해제");
                characterClickController.CancelObject(gameObject);
                stoppingDistance = 0;
                yield break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// 한 프레임 대기
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitOneFrame()
    {
        isWaited = false;
        yield return Time.deltaTime;
        isWaited = true;
    }

    /// <summary>
    /// 이동 중인지 확인
    /// </summary>
    /// <returns></returns>
    public bool CheckMoving() => isMoving;

    /// <summary>
    /// 선택
    /// </summary>
    /// <param name="isActive"></param>
    private void Select(bool isActive) => selectingGo.SetActive(isActive);
}
