using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbnailExample : MonoBehaviour
{
    public ThumbnailGenerator thumbnailGenerator; // ThumbnailGenerator ��ũ��Ʈ
    private string savePath = "TestThumbnail/Thumbnail.png"; // ���� ��� (Assets ���� ����)

    private void Start()
    {
        // ����� ����
        Sprite thumbnail = thumbnailGenerator.GenerateThumbnail();

        if (thumbnail != null)
        {
            // ����� ����
            string fullPath = Application.dataPath + "/" + savePath;
            thumbnailGenerator.SaveThumbnail(thumbnail, fullPath);
        }
    }
}