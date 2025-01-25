using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThumbnailExample : MonoBehaviour
{
    public ThumbnailGenerator thumbnailGenerator;
    

    private void Start()
    {
        StartCoroutine(thumbnailGenerator.Generate());
    }
}