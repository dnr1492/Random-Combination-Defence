using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UISkillInfo : MonoBehaviour
{
    private Image img;
    private Button btn;
    private RectTransform btnRt;

    [SerializeField] GameObject uiSkillDataPopupGo;

    private void Awake()
    {
        img = GetComponentsInChildren<Image>().FirstOrDefault(img => img.gameObject != gameObject);
        btn = GetComponent<Button>();
        btnRt = btn.GetComponent<RectTransform>();
    }

    public void Set(CharacterSkillData skillData, string damage)
    {
        UISkillDataPopup uiSkillDataPopup = uiSkillDataPopupGo.GetComponent<UISkillDataPopup>();

        btn.onClick.AddListener(() => {
            string skillDamage = string.Format(skillData.skillDamage, damage);
            string skillDescription = string.Format(skillData.skillDescription, skillDamage) + " " + skillData.skillEffect;
            uiSkillDataPopup.Init(skillData.skillName, skillDescription, btn.transform.position, btnRt);
            uiSkillDataPopupGo.SetActive(true);
        });

        img.sprite = Resources.Load<Sprite>(skillData.skillImagePath);
    }
}