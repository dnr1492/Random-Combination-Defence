using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager
{
    private static SpriteManager instance = null;

    private SpriteManager() { }

    public static SpriteManager GetInstance()
    {
        if (instance == null) instance = new SpriteManager();
        return instance;
    }

    public enum SpriteType { None, Bg, BgOutline, ImgCharacter }

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
        if (spriteType == SpriteType.Bg) return bgSprites[str];  //ItemClass, TierName으로 찾기
        else if (spriteType == SpriteType.BgOutline) return bgOutlineSprites[str];  //ItemClass, TierName으로 찾기
        else if (spriteType == SpriteType.ImgCharacter) return imgCharacterSprites[str];  //DisplayName으로 찾기
        else return null;
    }
}
