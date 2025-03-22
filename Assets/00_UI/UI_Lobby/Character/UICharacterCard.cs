using UnityEngine;
using UnityEngine.UI;

public class UICharacterCard : MonoBehaviour
{
    [SerializeField] UICharacterCardDataPopup uiCharacterDataPopup;
    [SerializeField] Text txtDisplayName, txtLevel, txtQuantity;
    [SerializeField] Image bg, bgOutline, imgCharacter, imgQuantity;
    [SerializeField] Button btn;

    private void Awake()
    {
        btn.onClick.AddListener(() => {
            uiCharacterDataPopup.gameObject.SetActive(true);
            string[] levelStrs = txtLevel.text.Split(".");
            string[] quantityStrs = txtQuantity.text.Split("/");
            uiCharacterDataPopup.Open(levelStrs[1], txtDisplayName.text, int.Parse(quantityStrs[0]), int.Parse(quantityStrs[1]));
        });

        //비활성화를 표현
        imgCharacter.color = new Color(imgCharacter.color.r, imgCharacter.color.g, imgCharacter.color.b, 0.3f);  
        btn.interactable = false;
    }

    public void Set(string displayName, int level, int curQuantity, int requiredLevelUpQuantity,
        Sprite bg, Sprite bgOutline, Sprite imgCharacter)
    {
        txtDisplayName.text = displayName;
        txtLevel.text = "Lv. " + level.ToString();
        txtQuantity.text = curQuantity + "/" + requiredLevelUpQuantity;
        imgQuantity.fillAmount = (float)curQuantity / requiredLevelUpQuantity;

        this.bg.sprite = bg;
        this.bgOutline.sprite = bgOutline;
        this.imgCharacter.sprite = imgCharacter;

        SpriteManager.GetInstance().SetImageRect(this.imgCharacter, new Vector2(0.5f, 0.5f), new Vector2(0, 15f), 1.25f);

        //활성화를 표현
        this.imgCharacter.color = new Color(this.imgCharacter.color.r, this.imgCharacter.color.g, this.imgCharacter.color.b, 1);  
        btn.interactable = true;
    }
}