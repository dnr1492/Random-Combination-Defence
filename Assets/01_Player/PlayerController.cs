using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private PlayerClickController playerClickController;
    private CameraController cameraController;
    private CharacterInfo curCharacterInfo;
    private Tilemap targetTilemap;
    private Vector3 movePos;
    private bool isSelected = false;
    private bool isWaited = false;
    private bool isArrived = false;
    private bool isMoving = false;
    private float stoppingDistance;  //도착 판단 임계선

    private Animator animator;
    private const string IDLE = "Idle";
    private const string RUN = "Run";

    public void Init(CameraController cameraController, Tilemap targetTilemap)
    {
        this.cameraController = cameraController;
        this.targetTilemap = targetTilemap;
    }

    public void InitClick(PlayerClickController playerClickController, UICharacterInfo uiCharacterInfo, UICharacterRecipe uiCharacterRecipe, bool isSelected)
    {
        this.playerClickController = playerClickController;
        this.isSelected = isSelected;

        if (gameObject.activeSelf) StartCoroutine(WaitOneFrame());  //플레이어를 선택하자마자 움직이는 것을 방지

        uiCharacterInfo.gameObject.SetActive(isSelected);
        uiCharacterRecipe.gameObject.SetActive(isSelected);

        if (isSelected)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            uiCharacterInfo.Init(curCharacterInfo);
            uiCharacterRecipe.SetReferenceRecipe(playerClickController.GetSelectedPlayers()[0].name);
        }
        else gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void Awake()
    {
        curCharacterInfo = InfoManager.GetInstance().LoadCharacterInfo(gameObject.name);

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
        Vector3Int gridPosition = targetTilemap.WorldToCell(worldPosition);
        if (targetTilemap.HasTile(gridPosition)) return true;
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
                playerClickController.CancelObject(gameObject);
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
}
