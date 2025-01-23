using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThumbnailExample : MonoBehaviour
{
    [SerializeField] Button btnGeneratorThumbnail;
    public ThumbnailGenerator thumbnailGenerator;
    

    private void Awake()
    {
        btnGeneratorThumbnail.onClick.AddListener(() => {
            StartCoroutine(thumbnailGenerator.Generate());
        });
    }
}