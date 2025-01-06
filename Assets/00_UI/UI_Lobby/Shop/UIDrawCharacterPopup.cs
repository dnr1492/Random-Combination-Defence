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

    private Sprite[] allSprites;
    private Dictionary<string, Sprite> bgSprites = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> bgOutlineSprites = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> imgCharacterSprites = new Dictionary<string, Sprite>();
    private List<GameObject> drawCharacterCardGos = new List<GameObject>();

    private readonly string bgSpriteName = "bg_characterCard_";
    private readonly string bgOutlineSpriteName = "bg_characterCard_outline_";
    private readonly string imgCharacterSpriteName = "temp(�̸�)_";

    private void Awake()
    {
        gameObject.SetActive(false);

        btnClose.onClick.AddListener(() => {
            gameObject.SetActive(false);
            DestroyAll();
        });

        allSprites = Resources.LoadAll<Sprite>("image/character");
        foreach (Sprite sprite in allSprites)
        {
            if (sprite.name.StartsWith(bgSpriteName) && !sprite.name.Contains("outline")) {
                string key = sprite.name.Replace(bgSpriteName, "");
                bgSprites[key] = sprite;
            }
            else if (sprite.name.StartsWith(bgOutlineSpriteName)) {
                string key = sprite.name.Replace(bgOutlineSpriteName, "");
                bgOutlineSprites[key] = sprite;
            }
            else {
                // ===== ���� ĳ���� �̸��� �������� ���� ���� ��� ===== //
                // ===== ���� ĳ���� �̸��� �������� ���� ���� ��� ===== //
                // ===== ���� ĳ���� �̸��� �������� ���� ���� ��� ===== //
                string key = sprite.name.Replace(imgCharacterSpriteName, "");
                imgCharacterSprites[key] = sprite;
            }
        }
    }

    public void Init(Dictionary<string, PlayFabManager.DrawCharacterData> dicDrawCharacterData)
    {
        gameObject.SetActive(true);

        foreach (var data in dicDrawCharacterData)
        {
            GameObject drawCharacterCardGo = Instantiate(uiDrawCharacterCardPrefab, contentTr);
            UIDrawCharacterCard uiDrawCharacterCard = drawCharacterCardGo.GetComponent<UIDrawCharacterCard>();
            uiDrawCharacterCard.Init(data.Value.displayName, data.Value.cardCount, bgSprites[data.Value.itemClass], bgOutlineSprites[data.Value.itemClass], imgCharacterSprites[data.Value.displayName]);
            drawCharacterCardGos.Add(drawCharacterCardGo);

            //Debug.Log(data.Value.displayName);  //�̸�
            //Debug.Log(data.Value.itemClass);  //���
            //Debug.Log(data.Value.cardCount);  //����
        }
    }

    private void DestroyAll()
    {
        foreach (GameObject obj in drawCharacterCardGos) if (obj != null) Destroy(obj);
        drawCharacterCardGos.Clear();
    }
}
