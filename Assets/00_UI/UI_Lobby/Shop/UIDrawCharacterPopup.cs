using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDrawCharacterPopup : MonoBehaviour
{
    [SerializeField] GameObject uiDrawCharacterCardPrefab;
    [SerializeField] Transform contentTr;
    [SerializeField] Button btnClose;

    private List<GameObject> drawCharacterCardGos = new List<GameObject>();

    private void Awake()
    {
        gameObject.SetActive(false);

        btnClose.onClick.AddListener(() => {
            gameObject.SetActive(false);
            DestroyAll();
        });
    }

    public void Init(Dictionary<string, PlayFabManager.DrawCharacterData> dicDrawCharacterData)
    {
        gameObject.SetActive(true);

        foreach (var data in dicDrawCharacterData)
        {
            GameObject drawCharacterCardGo = Instantiate(uiDrawCharacterCardPrefab, contentTr);
            UIDrawCharacterCard uiDrawCharacterCard = drawCharacterCardGo.GetComponent<UIDrawCharacterCard>();
            var bgSprtie = SpriteManager.GetInstance().GetSprite(SpriteManager.SpriteType.Bg, data.Value.itemClass);
            var bgOutlineSprite = SpriteManager.GetInstance().GetSprite(SpriteManager.SpriteType.BgOutline, data.Value.itemClass);
            var imgCharacterSprite = SpriteManager.GetInstance().GetSprite(SpriteManager.SpriteType.ImgCharacter, data.Value.displayName);
            uiDrawCharacterCard.Init(data.Value.displayName, data.Value.cardCount, bgSprtie, bgOutlineSprite, imgCharacterSprite);
            drawCharacterCardGos.Add(drawCharacterCardGo);
            
            //Debug.Log(data.Value.displayName);  //이름
            //Debug.Log(data.Value.itemClass);  //티어
            //Debug.Log(data.Value.cardCount);  //개수
        }
    }

    private void DestroyAll()
    {
        foreach (GameObject obj in drawCharacterCardGos) if (obj != null) Destroy(obj);
        drawCharacterCardGos.Clear();
    }
}
