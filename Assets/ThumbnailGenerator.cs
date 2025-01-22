using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbnailGenerator : MonoBehaviour
{
    public Camera renderCamera; // SPUM ĳ���͸� ĸó�� ī�޶�
    public GameObject spumPrefab; // SPUM ĳ���� ������
    private readonly int thumbnailWidth = 64; // ����� ���� ũ��
    private readonly int thumbnailHeight = 64; // ����� ���� ũ��

    /// <summary>
    /// SPUM ĳ���͸� �������Ͽ� ������� �����մϴ�.
    /// </summary>
    public Sprite GenerateThumbnail()
    {
        if (renderCamera == null || spumPrefab == null)
        {
            Debug.LogError("RenderCamera or SPUM Prefab is not set!");
            return null;
        }

        // 1. RenderTexture ����
        RenderTexture renderTexture = new RenderTexture(thumbnailWidth, thumbnailHeight, 16);
        renderCamera.targetTexture = renderTexture;

        // 2. ī�޶� ũ�� ����
        AdjustCameraSizeForThumbnail(spumPrefab, renderCamera);

        // 3. SPUM ĳ���� ������
        renderCamera.Render();

        // 4. RenderTexture�� Texture2D�� ��ȯ
        Texture2D texture = new Texture2D(thumbnailWidth, thumbnailHeight, TextureFormat.ARGB32, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, thumbnailWidth, thumbnailHeight), 0, 0);
        texture.Apply();

        // 5. Texture2D�� Sprite�� ��ȯ
        Sprite thumbnail = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // 6. RenderTexture ����
        RenderTexture.active = null;
        renderCamera.targetTexture = null;
        Destroy(renderTexture);

        return thumbnail;
    }

    /// <summary>
    /// ������ ������� ���� ���Ϸ� �����մϴ�.
    /// </summary>
    public void SaveThumbnail(Sprite thumbnail, string filePath)
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
    public void AdjustCameraSizeForThumbnail(GameObject targetObject, Camera renderCamera)
    {
        // 1. Ÿ�� ������Ʈ�� Bounds ���
        Bounds bounds = CalculateBounds(targetObject);

        // 2. ī�޶��� Orthographic Size ��� (���� ���� ����)
        float objectHeight = bounds.size.y;
        renderCamera.orthographicSize = objectHeight / 2f;

        // 3. ���� ������ ��� (ī�޶� Aspect Ratio)
        float objectWidth = bounds.size.x;
        float cameraWidth = objectHeight * renderCamera.aspect;

        if (objectWidth > cameraWidth)
        {
            renderCamera.orthographicSize = objectWidth / (2f * renderCamera.aspect);
        }

        // ī�޶� ��ġ�� ������Ʈ �߽ɿ� ����
        renderCamera.transform.position = new Vector3(bounds.center.x, bounds.center.y, renderCamera.transform.position.z);
    }

    /// <summary>
    /// Ÿ�� ������Ʈ�� Bounds�� ����մϴ�.
    /// </summary>
    private Bounds CalculateBounds(GameObject targetObject)
    {
        Renderer[] renderers = targetObject.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(targetObject.transform.position, Vector3.zero);

        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }
}
