using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ThumbnailGenerator : MonoBehaviour
{
    public Camera renderCamera; // SPUM ĳ���͸� ĸó�� ī�޶�
    public GameObject[] spumGos; // SPUM ĳ���� ������Ʈ
    public int thumbnailWidth = 128; // ����� ���� ũ��
    public int thumbnailHeight = 128; // ����� ���� ũ��
    private string savePath = "etc_ThumbnailScene"; // ���� ��� (Assets ���� ����)
    private Sprite[] sprites;

    public IEnumerator Generate()
    {
        StartCoroutine(GenerateThumbnail());
        yield return new WaitUntil(() => sprites.Length == spumGos.Length);

        //����� ����
        Sprite[] thumbnails = sprites;
        if (thumbnails != null)
        {
            for (int i = 0; i < thumbnails.Length; i++)
            {
                //����� ����
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
    /// SPUM ĳ���͸� �������Ͽ� ������� �����մϴ�.
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

            // 1. RenderTexture ����
            RenderTexture renderTexture = new RenderTexture(thumbnailWidth, thumbnailHeight, 16);
            renderCamera.targetTexture = renderTexture;

            // 2. ī�޶� ũ�� ����
            AdjustCameraSizeForThumbnail(spumGos[i], renderCamera);

            // 3. SPUM ĳ���� ������
            renderCamera.Render();

            // 4. RenderTexture�� Texture2D�� ��ȯ
            Texture2D texture = new Texture2D(thumbnailWidth, thumbnailHeight, TextureFormat.ARGB32, false);
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, thumbnailWidth, thumbnailHeight), 0, 0);
            texture.Apply();

            // 5. ���͸� ��� ����
            texture.filterMode = FilterMode.Point;
            texture.Apply(); // ���͸� ���� �� �ٽ� Apply ȣ��

            // 6. Texture2D�� Sprite�� ��ȯ
            Sprite thumbnail = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // 7. RenderTexture ����
            RenderTexture.active = null;
            renderCamera.targetTexture = null;
            Destroy(renderTexture);

            sprites[i] = thumbnail;

            yield return new WaitForSeconds(1);
        }
    }

    /// <summary>
    /// ������ ������� ���� ���Ϸ� �����մϴ�.
    /// </summary>
    private void SaveThumbnail(Sprite thumbnail, string filePath)
    {
        if (thumbnail == null)
        {
            Debug.LogError("Thumbnail is null. Cannot save.");
            return;
        }

        // Texture2D ��������
        Texture2D texture = thumbnail.texture;

        // Texture2D�� PNG�� ��ȯ
        byte[] pngData = texture.EncodeToPNG();
        if (pngData != null)
        {
            // ���� ����
            System.IO.File.WriteAllBytes(filePath, pngData);
            Debug.Log($"Thumbnail saved to: {filePath}");
        }
        else
        {
            Debug.LogError("Failed to encode thumbnail to PNG.");
        }
    }

    /// <summary>
    /// ī�޶� ũ�⸦ SPUM ĳ���Ϳ� �°� �����մϴ�.
    /// </summary>
    private void AdjustCameraSizeForThumbnail(GameObject targetObject, Camera renderCamera)
    {
        //CapsuleCollider2D collider2D = targetObject.GetComponent<CapsuleCollider2D>();
        //Bounds bounds = collider2D.bounds;

        //// 1. ��ü ũ�� ���
        //float objectHeight = bounds.size.y;
        //float objectWidth = bounds.size.x;

        //// 2. �е� ���� (1.5f = 50% ���� ���� �߰�)
        //float paddingFactor = 1.5f;

        //// 3. ī�޶� Orthographic Size ���
        //float orthographicHeight = (objectHeight / 2f) * paddingFactor;
        //float orthographicWidth = (objectWidth / 2f) * paddingFactor;

        //// 4. ī�޶��� ���� ũ��� ���� ũ�⸦ ��
        //if (orthographicWidth / renderCamera.aspect > orthographicHeight)
        //{
        //    renderCamera.orthographicSize = orthographicWidth / renderCamera.aspect;
        //}
        //else
        //{
        //    renderCamera.orthographicSize = orthographicHeight;
        //}

        //// 5. ī�޶� ��ġ ����
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
