using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CharacterController : MonoBehaviour
{
    private CharacterClickController characterClickController;
    private CameraController cameraController;
    private AnimatorController animationController;
    private CharacterInfo curCharacterInfo;
    private GameObject selectingGo;
    private RectTransform uiAttackRangeRt;
    private Tilemap mainTilemap;
    private Vector3 movePos;
    private bool isSelected = false;
    private bool isWaited = false;
    private bool isArrived = false;
    private bool isMoving = false;
    private float stoppingDistance;  //도착 판단 임계선

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
            //전체 선택이든 단일 선택이든 한 종류의 캐릭터를 선택하므로 [0]번째 인덱스에 접근
            uiCharacterRecipe.SetReferenceRecipe(characterClickController.GetSelectedCharacters()[0].name);
            Select(isSelected);
            ShowAttackRangeUI(isSelected);
        }
        else
        {
            Select(isSelected);
            ShowAttackRangeUI(isSelected);
        }
    }

    private void Awake()
    {
        animationController = new AnimatorController(GetComponentInChildren<Animator>());

        curCharacterInfo = InfoManager.GetInstance().LoadCharacterInfo(gameObject.name);

        selectingGo = transform.Find("Selecting").gameObject;
        Select(isSelected);

        uiAttackRangeRt = transform.Find("AttackRange").GetComponent<RectTransform>();
        ShowAttackRangeUI(isSelected);

        movePos = transform.position;
    }

    private void Update()
    {
        if (cameraController.mainCam == null) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (isSelected && !isMoving && isWaited) SetMove();

        if (isSelected) {
            //부모의 Scale을 -로 변경할 경우 자식 AttackRange의 Width가 -로 변경되는 것을 방지
            uiAttackRangeRt.sizeDelta = new Vector2(
                Mathf.Abs(uiAttackRangeRt.sizeDelta.x),
                uiAttackRangeRt.sizeDelta.y
            );
        }
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

                SetDirection(movePos);

                transform.position = Vector2.MoveTowards(transform.position, new Vector3(movePos.x, movePos.y, transform.position.z), curCharacterInfo.moveSpeed * Time.deltaTime);
                isMoving = true;
                timer += Time.deltaTime;
                animationController.ChangeState(AnimatorState.Move);

                if (timer >= 0.5f) {
                    DebugLogger.Log("일정 시간이 경과하여 stoppingDistance를 증가");
                    stoppingDistance += 0.1f;
                    timer = 0f;
                }
            }
            else
            {
                DebugLogger.Log("이동 완료");
                isArrived = true;
                isMoving = false;
            }

            if (isArrived)
            {
                DebugLogger.Log("이동 완료 후 자동으로 선택 해제");
                characterClickController.CancelObject(gameObject);
                stoppingDistance = 0;
                animationController.ChangeState(AnimatorState.Idle);
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

    /// <summary>
    /// 공격 범위를 UI로 표시
    /// </summary>
    /// <param name="isActive"></param>
    private void ShowAttackRangeUI(bool isActive)
    {
        uiAttackRangeRt.gameObject.SetActive(isActive);
        uiAttackRangeRt.sizeDelta = new Vector2(
            curCharacterInfo.attackRange * 2 / transform.lossyScale.x, 
            curCharacterInfo.attackRange * 2 / transform.lossyScale.y);
    }

    /// <summary>
    /// 방향 설정
    /// </summary>
    /// <param name="targetPos"></param>
    public void SetDirection(Vector3 targetPos)
    {
        bool isLeft = false;
        //오른쪽
        if (targetPos.x > transform.position.x) isLeft = false;
        //왼쪽
        else if (targetPos.x < transform.position.x) isLeft = true;

        Vector3 newScale = transform.localScale;
        newScale.x = isLeft ? Mathf.Abs(newScale.x) : -Mathf.Abs(newScale.x);
        transform.localScale = newScale;
    }
}