using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacter : MonoBehaviour
{
    private List<UICharacterCard> uiCharacterCards;

    private void Awake()
    {
        uiCharacterCards = new List<UICharacterCard>();
        uiCharacterCards.AddRange(GetComponentsInChildren<UICharacterCard>());
        foreach (UICharacterCard card in uiCharacterCards) {
            card.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Delay());
    }

    public void DisplayCharacters(Dictionary<string, PlayFabManager.StructCharacterCardData> dicAllCharacterDatas)
    {
        string displayName;
        int level;
        int quantity;
        int tierNum;
        string tierName = null;

        Sprite bgSprtie;
        Sprite bgOutlineSprite;
        Sprite imgCharacterSprite;

        int index = 0;
        foreach (var data in dicAllCharacterDatas)
        {
            if (index < uiCharacterCards.Count)
            {
                displayName = data.Key;
                level = data.Value.Level;
                quantity = data.Value.quantity;
                tierNum = data.Value.TierNum;

                foreach (PlayFabManager.CharacterTier tier in Enum.GetValues(typeof(PlayFabManager.CharacterTier))) {
                    if ((int)tier == tierNum) tierName = tier.ToString();
                }

                bgSprtie = SpriteManager.GetInstance().GetSprite(SpriteManager.SpriteType.Bg, tierName);
                bgOutlineSprite = SpriteManager.GetInstance().GetSprite(SpriteManager.SpriteType.BgOutline, tierName);
                imgCharacterSprite = SpriteManager.GetInstance().GetSprite(SpriteManager.SpriteType.ImgCharacter, displayName);

                uiCharacterCards[index].gameObject.SetActive(true);
                uiCharacterCards[index].Set(
                    displayName,
                    level,
                    quantity,
                    DataManager.GetInstance().GetCharacterCardLevelQuentityData(level, tierNum),
                    bgSprtie,
                    bgOutlineSprite,
                    imgCharacterSprite
                );
                index++;
            }
        }
    }

    private IEnumerator Delay()
    {
        yield return Time.deltaTime;
        PlayFabManager.instance.DisplayCharacterCard(this);
    }
}