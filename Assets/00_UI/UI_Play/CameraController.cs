using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public Camera mainCam, containerCam;
    [SerializeField] PlayerClickController playerClickController;
    [SerializeField] Tilemap /*mainTilemap,*/ containerTilemap;

    private void Awake()
    {
        containerCam.gameObject.SetActive(false);
    }

    private void Start()
    {
        //AdjustCameraSize(mainTilemap, mainCam);
        AdjustCameraSize(containerTilemap, containerCam);
    }

    private void Update()
    {
        if (playerClickController.GetSelectedPlayers().Count == 0) UpdatePanningSetting();
    }

    private void AdjustCameraSize(Tilemap tilemap, Camera cam)
    {
        // *** 카메라의 사이즈를 조절하는 로직이며, 카메라의 포지션은 에디터상에서 수동으로 조절 요망 *** //

        //타일맵의 바운더리(경계)를 가져옵니다.
        BoundsInt tilemapBounds = tilemap.cellBounds;
        Vector3 tilemapSize = new Vector3(tilemapBounds.size.x, tilemapBounds.size.y, 0);

        //타일의 실제 크기를 고려하여 타일맵의 월드 크기를 계산합니다.
        Vector3 tilemapWorldSize = Vector3.Scale(tilemapSize, tilemap.cellSize);

        //해상도 비율 계산
        float screenAspect = (float)Screen.width / Screen.height;

        //타일맵의 가로세로 비율
        float tilemapAspect = tilemapWorldSize.x / tilemapWorldSize.y;

        if (screenAspect >= tilemapAspect)
        {
            //화면 비율이 타일맵 비율보다 클 때
            //카메라 높이에 맞춰서 크기를 조절
            cam.orthographicSize = tilemapWorldSize.y / 2;
        }
        else
        {
            //화면 비율이 타일맵 비율보다 작을 때
            //카메라 너비에 맞춰서 크기를 조절
            float differenceInSize = tilemapAspect / screenAspect;
            cam.orthographicSize = tilemapWorldSize.y / 2 * differenceInSize;
        }
    }

    public void OnContainerCamera()
    {
        containerCam.gameObject.SetActive(true);
    }

    public void OffContainerCamera()
    {
        containerCam.gameObject.SetActive(false);
    }

    private Vector2 lastPanPosition;  //마지막 터치 위치 저장
    private Vector2 limitMin = new Vector2(-34f, -9.5f);
    private Vector2 limitMax = new Vector2(34f, 9.5f);
    private float panSpeed = 0.009f;

    #region Panning
    private void UpdatePanningSetting()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            //터치 시작
            if (touch.phase == TouchPhase.Began) lastPanPosition = touch.position;

            //터치 중
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchPosition = touch.position;
                Vector2 panDelta = lastPanPosition - touchPosition;

                Panning(panDelta);

                lastPanPosition = touchPosition;
            }
        }
    }

    private void Panning(Vector2 panDelta)
    {
        //패닝 방향과 속도에 따라 카메라의 새 위치 계산
        Vector3 newPosition = mainCam.transform.position + new Vector3(panDelta.x * panSpeed, panDelta.y * panSpeed, 0f);

        //이동 범위를 제한 (x, y 좌표만)
        newPosition.x = Mathf.Clamp(newPosition.x, limitMin.x, limitMax.x);
        newPosition.y = Mathf.Clamp(newPosition.y, limitMin.y, limitMax.y);

        //새로운 위치로 카메라 이동
        mainCam.transform.position = newPosition;
    }
    #endregion
}