using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    [SerializeField] Slider sliderHp;

    private RectTransform hpRt;
    private Transform parantTr;

    private readonly float offsetY = 0.2f;
    private readonly float baseScale = 3f;
    private readonly float baseHeight = 0.05f;
   
    public void SetHp(float curHp, float maxHp)
    {
        gameObject.SetActive(true);
        sliderHp.value = curHp / maxHp;
    }

    private void Awake()
    {
        gameObject.SetActive(false);

        parantTr = transform.parent;
        hpRt = GetComponent<RectTransform>();

        SetHpPosition();
        SetHpWidth();
        SetHpHeight();
    }

    private void SetHpPosition()
    {
        Renderer renderer = parantTr.GetComponent<Renderer>();
        Bounds bounds = renderer.bounds;
        Vector3 topPos = new Vector3(bounds.center.x, bounds.center.y + renderer.bounds.extents.y);
        topPos.y += offsetY;
        Vector3 pos = topPos;
        hpRt.transform.position = pos;
    }

    private void SetHpWidth()
    {
        Renderer renderer = parantTr.GetComponent<Renderer>();
        float spriteWidth = renderer.bounds.size.x / parantTr.localScale.x;
        hpRt.sizeDelta = new Vector2(spriteWidth, hpRt.sizeDelta.y);
        hpRt.localScale = Vector3.one;
    }

    private void SetHpHeight()
    {
        float currentScale = parantTr.localScale.y;
        float adjustedHeight = baseHeight * (baseScale / currentScale);
        float width = hpRt.sizeDelta.x;
        hpRt.sizeDelta = new Vector2(width, adjustedHeight);
    }
}
