using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbnailExample : MonoBehaviour
{
    public ThumbnailGenerator thumbnailGenerator; // ThumbnailGenerator 스크립트
    private string savePath = "TestThumbnail/Thumbnail.png"; // 저장 경로 (Assets 폴더 기준)

    private void Start()
    {
        // 썸네일 생성
        Sprite thumbnail = thumbnailGenerator.GenerateThumbnail();

        if (thumbnail != null)
        {
            // 썸네일 저장
            string fullPath = Application.dataPath + "/" + savePath;
            thumbnailGenerator.SaveThumbnail(thumbnail, fullPath);
        }
    }
}