using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public Camera mainCam, containerCam;
    [SerializeField] CharacterClickController characterClickController;
    [SerializeField] Tilemap mainTilemap, containerTilemap;
    //[SerializeField] RectTransform safeAreaRect;

    private readonly float camSizeOffset = 1.1f;
    private readonly float verticalOffsetRatio = 0.0775f;  //0.0은 뷰포트의 최상단, 1.0은 최하단

    private void Awake()
    {
        containerCam.gameObject.SetActive(false);
    }

    private void Start()
    {
        AdjustCameraSizeAndPosition();
    }

    private void Update()
    {
        //if (characterClickController.GetSelectedCharacters().Count == 0) UpdatePanningSetting();
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

    // ================================================================================================================================================= //
    // ================================================================================================================================================= //
    // ================================================================================================================================================= //

    private Bounds CalculateTilemapBounds(Tilemap tilemap)
    {
        BoundsInt cellBounds = tilemap.cellBounds;
        Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        foreach (Vector3Int pos in cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                Vector3 worldPos = tilemap.CellToWorld(pos);
                min = Vector3.Min(min, worldPos);
                max = Vector3.Max(max, worldPos);
            }
        }

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min / camSizeOffset, max);
        return bounds;
    }

    private void AdjustCameraSizeAndPosition()
    {
        // 1. 타일맵의 Bounds 계산
        Bounds bounds = CalculateTilemapBounds(mainTilemap);

        // 2. 화면 비율과 타일맵 비율 계산
        float screenAspect = (float)Screen.width / Screen.height;
        float mapAspect = bounds.size.x / bounds.size.y;

        // 3. 카메라의 Orthographic Size 설정
        if (screenAspect >= mapAspect) mainCam.orthographicSize = bounds.size.y / 2;
        else mainCam.orthographicSize = bounds.size.x / screenAspect / 2;

        // 4. Safe Area Rect 가져오기
        Rect safeArea = Screen.safeArea;

        float safeAreaTopOffset = (Screen.height - (safeArea.y + safeArea.height)) / Screen.height;

        // 6. 타일맵의 상단 중앙 월드 좌표 계산
        Vector3 topCenterWorld = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);

        // 7. 카메라의 뷰포트에서 상단 중앙에 해당하는 월드 좌표 계산
        Vector3 viewportTopCenter = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 1.0f - verticalOffsetRatio - safeAreaTopOffset, mainCam.nearClipPlane));

        // 8. 카메라의 위치 조정
        Vector3 offset = topCenterWorld - viewportTopCenter;
        mainCam.transform.position += new Vector3(offset.x, offset.y, 0);
    }
}