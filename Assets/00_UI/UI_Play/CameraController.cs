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
        // *** ī�޶��� ����� �����ϴ� �����̸�, ī�޶��� �������� �����ͻ󿡼� �������� ���� ��� *** //

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