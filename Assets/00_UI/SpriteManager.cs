using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager
{
    private static SpriteManager instance = null;

    private SpriteManager() { }

    public static SpriteManager GetInstance()
    {
        if (instance == null) instance = new SpriteManager();
        return instance;
    }

    private Sprite[] allSprites;
    private Dictionary<string, Sprite> bgSprites = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> bgOutlineSprites = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> imgCharacterSprites = new Dictionary<string, Sprite>();

    private readonly string bgSpriteName = "bg_characterCard_";
    private readonly string bgOutlineSpriteName = "bg_characterCard_outline_";

    public void LoadSpriteAll()
    {
        allSprites = Resources.LoadAll<Sprite>("image/character");
        foreach (Sprite sprite in allSprites)
        {
            if (sprite.name.StartsWith(bgSpriteName) && !sprite.name.Contains("outline"))
            {
                string key = sprite.name.Replace(bgSpriteName, "");
                bgSprites[key] = sprite;
            }
            else if (sprite.name.StartsWith(bgOutlineSpriteName))
            {
                string key = sprite.name.Replace(bgOutlineSpriteName, "");
                bgOutlineSprites[key] = sprite;
            }
            else
            {
                string key = sprite.name;
                imgCharacterSprites[key] = sprite;
            }
        }
    }

    public Sprite GetSprite(SpriteType spriteType, string str)
    {
        if (spriteType == SpriteType.Bg) return bgSprites[str];  //TierName, ItemClass�� ã��
        else if (spriteType == SpriteType.BgOutline) return bgOutlineSprites[str];  //TierName, ItemClass�� ã��
        else if (spriteType == SpriteType.ImgCharacter) return imgCharacterSprites[str];  //DisplayName���� ã��
        else return null;
    }

    public void SetImageRect(Image image, Vector2 anchor, Vector2 anchoredPosition, float sizeDeltaOffset)
    {
        Sprite sprite = image.sprite;
        RectTransform rt = image.rectTransform;
        //Anchor�� ~���� ����
        rt.anchorMin = rt.anchorMax = anchor;
        //������ Sprite Pivot �״�� ����
        Vector2 normalizedPivot = sprite.pivot / sprite.rect.size;
        rt.pivot = normalizedPivot;
        image.SetNativeSize();
        //��ġ �� ũ�� ������
        rt.anchoredPosition = anchoredPosition;
        rt.sizeDelta *= sizeDeltaOffset;
    }
}
