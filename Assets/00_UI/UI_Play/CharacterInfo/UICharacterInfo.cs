using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterInfo : MonoBehaviour
{
    [SerializeField] Image imgCharacter;
    [SerializeField] Text txtLevel, txtDisplayName, txtDamage, txtAttackSpeed, txtAttackRange, txtMoveSpeed, txtSkillName, txtSkillDescription;

    private SelectSkill curSelectSkill;
    [SerializeField] Image[] arrImgSkill;
    [SerializeField] Button[] arrBtnSkill;
    private CustomBackground[] arrCb;

    private void Awake()
    {
        gameObject.SetActive(false);

        arrCb = new CustomBackground[arrBtnSkill.Length];
        for (int i = 0; i < arrBtnSkill.Length; i++) arrCb[i] = arrBtnSkill[i].GetComponent<CustomBackground>();
    }

    public void Init(CharacterInfo characterInfo)
    {
        gameObject.SetActive(true);

        txtLevel.text = "Lv. " + characterInfo.level.ToString();
        txtDisplayName.text = characterInfo.displayName;
        txtDamage.text = characterInfo.damage.ToString();
        txtAttackSpeed.text = characterInfo.attackSpeed.ToString();
        txtAttackRange.text = characterInfo.attackRange.ToString();
        txtMoveSpeed.text = characterInfo.moveSpeed.ToString();

        for (int i = 0; i < arrBtnSkill.Length; i++) arrBtnSkill[i].gameObject.SetActive(false);

        int unlockSkillIndex = 0;
        for (int i = 0; i < characterInfo.skillDatas.Count; i++)
        {
            arrBtnSkill[i].gameObject.SetActive(true);
            if (unlockSkillIndex >= characterInfo.unlockSkills.Count)
            {
                arrCb[i].UnlockSkill();
                arrBtnSkill[i].interactable = false;
            }
            unlockSkillIndex++;
        }

        if (characterInfo.skillDatas.Count == 0 || characterInfo.skillDatas == null)
        {
            DebugLogger.Log("스킬이 존재하지 않습니다.");
            txtSkillName.text = "부존재";
            txtSkillDescription.text = "부존재";
            return;
        }

        curSelectSkill = SelectSkill.One;
        arrCb[(int)curSelectSkill].SetSelect(true);
        txtSkillName.text = characterInfo.skillDatas[(int)curSelectSkill].skillName;
        txtSkillDescription.text = characterInfo.skillDatas[(int)curSelectSkill].skillDescription;

        for (int i = 0; i < arrBtnSkill.Length; i++)
        {
            int index = i;
            arrCb[index].SetSelect(index == (int)curSelectSkill);
            arrBtnSkill[index].onClick.AddListener(() => {
                curSelectSkill = (SelectSkill)index;
                for (int i = 0; i < arrCb.Length; i++) arrCb[i].SetSelect(i == (int)curSelectSkill);
                txtSkillName.text = characterInfo.skillDatas[(int)curSelectSkill].skillName;
                txtSkillDescription.text = characterInfo.skillDatas[(int)curSelectSkill].skillDescription;
            });
        }
    }
}
