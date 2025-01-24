using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ThumbnailGenerator : MonoBehaviour
{
    public Camera renderCamera; // SPUM 캐릭터를 캡처할 카메라
    public GameObject[] spumGos; // SPUM 캐릭터 오브젝트
    public int thumbnailWidth = 128; // 썸네일 가로 크기
    public int thumbnailHeight = 128; // 썸네일 세로 크기
    private string savePath = "etc_ThumbnailScene"; // 저장 경로 (Assets 폴더 기준)
    private Sprite[] sprites;

    public IEnumerator Generate()
    {
        StartCoroutine(GenerateThumbnail());
        yield return new WaitUntil(() => sprites.Length == spumGos.Length);

        //썸네일 생성
        Sprite[] thumbnails = sprites;
        if (thumbnails != null)
        {
            for (int i = 0; i < thumbnails.Length; i++)
            {
                //썸네일 저장
                string fullPath = Application.dataPath + "/" + savePath + "/" + spumGos[i].name + ".png";
                SaveThumbnail(thumbnails[i], fullPath);
                Debug.Log(thumbnails[i].name);
                yield return new WaitForSeconds(1);
            }
        }
    }

    private void Active(int index)
    {
        for (int i = 0; i < spumGos.Length; i++) {
            spumGos[i].SetActive(false);
        }
        spumGos[index].SetActive(true);
    }

    /// <summary>
    /// SPUM 캐릭터를 렌더링하여 썸네일을 생성합니다.
    /// </summary>
    private IEnumerator GenerateThumbnail()
    {
        sprites = new Sprite[spumGos.Length];

        for (int i = 0; i < spumGos.Length; i++)
        {
            Active(i);

            if (renderCamera == null || spumGos[i] == null)
            {
                Debug.LogError("RenderCamera or SPUM Prefab is not set!");
                yield break;
            }

            // 1. RenderTexture 생성
            RenderTexture renderTexture = new RenderTexture(thumbnailWidth, thumbnailHeight, 16);
            renderCamera.targetTexture = renderTexture;

            // 2. 카메라 크기 조정
            AdjustCameraSizeForThumbnail(spumGos[i], renderCamera);

            // 3. SPUM 캐릭터 렌더링
            renderCamera.Render();

            // 4. RenderTexture를 Texture2D로 변환
            Texture2D texture = new Texture2D(thumbnailWidth, thumbnailHeight, TextureFormat.ARGB32, false);
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, thumbnailWidth, thumbnailHeight), 0, 0);
            texture.Apply();

            // 5. 필터링 모드 설정
            texture.filterMode = FilterMode.Point;
            texture.Apply(); // 필터링 적용 후 다시 Apply 호출

            // 6. Texture2D를 Sprite로 변환
            Sprite thumbnail = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // 7. RenderTexture 해제
            RenderTexture.active = null;
            renderCamera.targetTexture = null;
            Destroy(renderTexture);

            sprites[i] = thumbnail;

            yield return new WaitForSeconds(1);
        }
    }

    /// <summary>
    /// 생성한 썸네일을 로컬 파일로 저장합니다.
    /// </summary>
    private void SaveThumbnail(Sprite thumbnail, string filePath)
    {
        if (thumbnail == null)
        {
            Debug.LogError("Thumbnail is null. Cannot save.");
            return;
        }

        // Texture2D 가져오기
        Texture2D texture = thumbnail.texture;

        // Texture2D를 PNG로 변환
        byte[] pngData = texture.EncodeToPNG();
        if (pngData != null)
        {
            // 파일 저장
            System.IO.File.WriteAllBytes(filePath, pngData);
            Debug.Log($"Thumbnail saved to: {filePath}");
        }
        else
        {
            Debug.LogError("Failed to encode thumbnail to PNG.");
        }
    }

    /// <summary>
    /// 카메라 크기를 SPUM 캐릭터에 맞게 조정합니다.
    /// </summary>
    private void AdjustCameraSizeForThumbnail(GameObject targetObject, Camera renderCamera)
    {
        //CapsuleCollider2D collider2D = targetObject.GetComponent<CapsuleCollider2D>();
        //Bounds bounds = collider2D.bounds;

        //// 1. 객체 크기 계산
        //float objectHeight = bounds.size.y;
        //float objectWidth = bounds.size.x;

        //// 2. 패딩 비율 (1.5f = 50% 여유 공간 추가)
        //float paddingFactor = 1.5f;

        //// 3. 카메라 Orthographic Size 계산
        //float orthographicHeight = (objectHeight / 2f) * paddingFactor;
        //float orthographicWidth = (objectWidth / 2f) * paddingFactor;

        //// 4. 카메라의 세로 크기와 가로 크기를 비교
        //if (orthographicWidth / renderCamera.aspect > orthographicHeight)
        //{
        //    renderCamera.orthographicSize = orthographicWidth / renderCamera.aspect;
        //}
        //else
        //{
        //    renderCamera.orthographicSize = orthographicHeight;
        //}

        //// 5. 카메라 위치 설정
        //renderCamera.transform.position = new Vector3(bounds.center.x, bounds.center.y, renderCamera.transform.position.z);

        // ============================================================================================================================= //
        // ============================================================================================================================= //
        // ============================================================================================================================= //

        CapsuleCollider2D collider2D = targetObject.GetComponent<CapsuleCollider2D>();

        Bounds bounds = collider2D.bounds;
        Vector3 objectCenter = bounds.center;

        renderCamera.transform.position = new Vector3(objectCenter.x, objectCenter.y, renderCamera.transform.position.z);
        renderCamera.orthographic = true;
        renderCamera.orthographicSize = 3f;
    }
}
