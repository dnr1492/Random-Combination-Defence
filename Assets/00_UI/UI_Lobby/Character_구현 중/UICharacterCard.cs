using UnityEngine;
using UnityEngine.UI;

public class UICharacterCard : MonoBehaviour
{
    [SerializeField] Text txtName, txtLevel, txtQuantity;
    [SerializeField] Image imgCharacter, imgQuantity;
    [SerializeField] Button btn;
    [SerializeField] GameObject uiCharacterDataPopupPrefab;

    private void Awake()
    {
        btn.onClick.AddListener(() => {
            uiCharacterDataPopupPrefab.SetActive(true);
            string[] levelStrs = txtLevel.text.Split(".");
            string[] quantityStrs = txtQuantity.text.Split("/");
            uiCharacterDataPopupPrefab.GetComponent<UICharacterCardDataPopup>().Open(levelStrs[1], txtName.text, int.Parse(quantityStrs[0]), int.Parse(quantityStrs[1]));
        });

        //비활성화를 표현
        imgCharacter.color = new Color(imgCharacter.color.r, imgCharacter.color.g, imgCharacter.color.b, 0.3f);  
        btn.interactable = false;
    }

    public void Set(int level, int curQuantity, int requiredLevelUpQuantity)
    {
        txtLevel.text = "Lv. " + level.ToString();
        txtQuantity.text = curQuantity + "/" + requiredLevelUpQuantity;
        imgQuantity.fillAmount = (float)curQuantity / requiredLevelUpQuantity;

        //활성화를 표현
        imgCharacter.color = new Color(imgCharacter.color.r, imgCharacter.color.g, imgCharacter.color.b, 1);  
        btn.interactable = true;
    }

    public string GetName()
    {
        return txtName.text;
    }
}