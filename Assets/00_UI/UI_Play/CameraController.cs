using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public Camera mainCam, containerCam;
    [SerializeField] CharacterClickController characterClickController;
    [SerializeField] Tilemap mainTilemap, containerTilemap;
    [SerializeField] RectTransform safeAreaRect;

    private readonly float camOffsetY = 3.5f;  //Ÿ�ϸ� ������ 1/3.5f��ŭ �Ʒ��� �̵�

    private void Awake()
    {
        containerCam.gameObject.SetActive(false);
    }

    private void Start()
    {
        AdjustCameraSizeAndPosition(mainTilemap, mainCam);
        AdjustCameraSizeAndPosition(containerTilemap, containerCam);
    }

    private void Update()
    {
        //if (characterClickController.GetSelectedCharacters().Count == 0) UpdatePanningSetting();
    }

    private void AdjustCameraSizeAndPosition(Tilemap tilemap, Camera cam)
    {
        #region ī�޶� ������ ����
        //Ÿ�ϸ��� �ٿ����(���)�� �����ɴϴ�.
        BoundsInt tilemapBounds = tilemap.cellBounds;
        Vector3 tilemapSize = new Vector3(tilemapBounds.size.x, tilemapBounds.size.y, 0);

        //Ÿ���� ���� ũ�⸦ ����Ͽ� Ÿ�ϸ��� ���� ũ�⸦ ����մϴ�.
        Vector3 tilemapWorldSize = Vector3.Scale(tilemapSize, tilemap.cellSize);

        //�ػ� ���� ���
        float screenAspect = (float)Screen.width / Screen.height;

        //Ÿ�ϸ��� ���μ��� ����
        float tilemapAspect = tilemapWorldSize.x / tilemapWorldSize.y;

        if (screenAspect >= tilemapAspect)
        {
            //ȭ�� ������ Ÿ�ϸ� �������� Ŭ ��
            //ī�޶� ���̿� ���缭 ũ�⸦ ����
            cam.orthographicSize = tilemapWorldSize.y / 2;
        }
        else
        {
            //ȭ�� ������ Ÿ�ϸ� �������� ���� ��
            //ī�޶� �ʺ� ���缭 ũ�⸦ ����
            float differenceInSize = tilemapAspect / screenAspect;
            cam.orthographicSize = tilemapWorldSize.y / 2 * differenceInSize;
        }
        #endregion

        #region ī�޶� ��ġ ����
        Vector2 safeAreaMin = safeAreaRect.anchorMin;
        float safeAreaOffsetRatio = safeAreaMin.y;
        float safeAreaOffsetY = cam.orthographicSize * 2 * safeAreaOffsetRatio;

        //Ÿ�ϸ� �߽�
        Vector3Int tilemapCenterInt = new Vector3Int(
            Mathf.RoundToInt(tilemap.cellBounds.center.x),
            Mathf.RoundToInt(tilemap.cellBounds.center.y),
            Mathf.RoundToInt(tilemap.cellBounds.center.z)
        );
        //Ÿ�ϸ� �߽��� ���� ��ǥ
        Vector3 tilemapCenter = tilemap.CellToWorld(tilemapCenterInt);

        //Ÿ�ϸ� ������ 1/6��ŭ �Ʒ��� �̵�
        float verticalOffset = -tilemapWorldSize.y / camOffsetY;

        cam.transform.position = new Vector3(
            tilemapCenter.x,
            tilemapCenter.y + verticalOffset - safeAreaOffsetY,  //Safe Area�� verticalOffset �ݿ�
            cam.transform.position.z
        );
        #endregion
    }

    public void OnContainerCamera()
    {
        containerCam.gameObject.SetActive(true);
    }

    public void OffContainerCamera()
    {
        containerCam.gameObject.SetActive(false);
    }

    private Vector2 lastPanPosition;  //������ ��ġ ��ġ ����
    private Vector2 limitMin = new Vector2(-34f, -9.5f);
    private Vector2 limitMax = new Vector2(34f, 9.5f);
    private float panSpeed = 0.009f;

    #region Panning
    private void UpdatePanningSetting()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            //��ġ ����
            if (touch.phase == TouchPhase.Began) lastPanPosition = touch.position;

            //��ġ ��
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
        //�д� ����� �ӵ��� ���� ī�޶��� �� ��ġ ���
        Vector3 newPosition = mainCam.transform.position + new Vector3(panDelta.x * panSpeed, panDelta.y * panSpeed, 0f);

        //�̵� ������ ���� (x, y ��ǥ��)
        newPosition.x = Mathf.Clamp(newPosition.x, limitMin.x, limitMax.x);
        newPosition.y = Mathf.Clamp(newPosition.y, limitMin.y, limitMax.y);

        //���ο� ��ġ�� ī�޶� �̵�
        mainCam.transform.position = newPosition;
    }
    #endregion
}