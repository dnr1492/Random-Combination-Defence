using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDrawCharacterCard : MonoBehaviour
{
    [SerializeField] Image bg, bgOutline, imgCharacter;
    [SerializeField] Text txtName, txtGainQuantity;

    public void Init(string displayName, int cardCount, Sprite bg, Sprite bgOutline, Sprite imgCharacter)
    {
        this.bg.sprite = bg;
        this.bgOutline.sprite = bgOutline;
        this.imgCharacter.sprite = imgCharacter;

        txtName.text = displayName;
        txtGainQuantity.text = cardCount.ToString();
    }
}
