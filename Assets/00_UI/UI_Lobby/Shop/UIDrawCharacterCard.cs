using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDrawCharacterCard : MonoBehaviour
{
    [SerializeField] Image bg, bgOutline, imgCharacter;
    [SerializeField] Text txtDisplayName, txtGainQuantity;

    public void Init(string displayName, int cardCount, Sprite bg, Sprite bgOutline, Sprite imgCharacter)
    {
        this.bg.sprite = bg;
        this.bgOutline.sprite = bgOutline;
        this.imgCharacter.sprite = imgCharacter;

        SpriteManager.GetInstance().SetImageRect(this.imgCharacter, new Vector2(0.5f, 0.5f), Vector2.zero, 1.25f);

        txtDisplayName.text = displayName;
        txtGainQuantity.text = cardCount.ToString();
    }
}
