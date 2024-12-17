using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterInfo : MonoBehaviour
{
    [SerializeField] Image imgCharacter;
    [SerializeField] Text txtLevel, txtName, txtDamage, txtAttackSpeed, txtAttackRange, txtMoveSpeed, txtSkillName, txtSkillDescription;

    private enum SelectSkill { One, Two, Three, Four, Five, Six, Seven }
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
        txtName.text = characterInfo.name;
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
                arrCb[i].Unlock();
                arrBtnSkill[i].interactable = false;
            }
            unlockSkillIndex++;
        }

        if (characterInfo.skillDatas.Count == 0 || characterInfo.skillDatas == null)
        {
            Debug.Log("��ų ������");
            txtSkillName.text = "������";
            txtSkillDescription.text = "������";
            return;
        }

        curSelectSkill = SelectSkill.One;
        arrCb[(int)curSelectSkill].SetSelect(true);
        txtSkillName.text = characterInfo.skillDatas[(int)curSelectSkill].skill_name;
        txtSkillDescription.text = characterInfo.skillDatas[(int)curSelectSkill].skill_description;

        for (int i = 0; i < arrBtnSkill.Length; i++)
        {
            int index = i;
            arrCb[index].SetSelect(index == (int)curSelectSkill);
            arrBtnSkill[index].onClick.AddListener(() => {
                curSelectSkill = (SelectSkill)index;
                for (int i = 0; i < arrCb.Length; i++) arrCb[i].SetSelect(i == (int)curSelectSkill);
                txtSkillName.text = characterInfo.skillDatas[(int)curSelectSkill].skill_name;
                txtSkillDescription.text = characterInfo.skillDatas[(int)curSelectSkill].skill_description;
            });
        }
    }
}
