using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterCardDataPopup : MonoBehaviour
{
    [SerializeField] UICharacter uiCharacter;
    [SerializeField] Image bgCharacter, bgCharacterOutline, imgCharacter, imgQuantity;
    [SerializeField] Text txtQuantity, txtCurLevel, txtName;
    [SerializeField] Text txtStatsDamage, txtStatsAttackSpeed, txtStatsAttackRange, txtStatsMoveSpeed;
    [SerializeField] Image[] imgBgLevel, imgBgDescription;
    [SerializeField] Text[] txtDescriptionLevel, txtDescriptions;
    [SerializeField] Button btnClose, btnExternalClose, btnUpgrade;
    [SerializeField] GameObject[] btnSkillInfos;

    private Color defaultColor = Color.white;
    private Color gainColor = new Color(0.8f, 0.6f, 0.15f);

    private void Awake()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < btnSkillInfos.Length; i++) btnSkillInfos[i].SetActive(false);
    }

    private void Update()
    {
        btnClose.onClick.AddListener(() => {
            gameObject.SetActive(false);
        });

        btnExternalClose.onClick.AddListener(() => {
            gameObject.SetActive(false);
        });
    }

    public void Open(string curLevel, string name, int curQuantity, int requiredLevelUpQuantity)
    {
        ResetData();

        Dictionary<string, CharacterCardLevelInfoData> dicCharacterCardLevelInfoDatas = DataManager.GetInstance().GetCharacterCardLevelInfoData();
        int level = int.Parse(curLevel);

        var characterData = DataManager.GetInstance().GetCharacterData();
        string itemClass = null;
        foreach (PlayFabManager.CharacterTier tier in Enum.GetValues(typeof(PlayFabManager.CharacterTier))) {
            if ((int)tier == characterData[name].tier) itemClass = tier.ToString();
        }

        bgCharacter.sprite = SpriteManager.GetInstance().GetSprite(SpriteManager.SpriteType.Bg, itemClass);
        bgCharacterOutline.sprite = SpriteManager.GetInstance().GetSprite(SpriteManager.SpriteType.BgOutline, itemClass);
        imgCharacter.sprite = SpriteManager.GetInstance().GetSprite(SpriteManager.SpriteType.ImgCharacter, name);
        imgQuantity.fillAmount = (float)curQuantity / requiredLevelUpQuantity;
        txtQuantity.text = curQuantity + "/" + requiredLevelUpQuantity;
        txtCurLevel.text = "Lv. " + level;
        txtName.text = name;

        SetSkillInfoButton(name);

        // ↓↓↓↓↓↓↓↓↓↓ addSkills을 활용하여 추후 획득한 스킬 추가 로직 구현하기 ↓↓↓↓↓↓↓↓↓↓ //
        //                                                                                                    //
        //                                                                                                    //
        //                                                                                                    //
        // ↑↑↑↑↑↑↑↑↑↑ addSkills을 활용하여 추후 획득한 스킬 추가 로직 구현하기 ↑↑↑↑↑↑↑↑↑↑ //

        for (int i = 1; i < level; i++)
        {
            imgBgLevel[i - 1].color = gainColor;
            imgBgDescription[i - 1].color = gainColor;
        }

        for (int i = 0; i < dicCharacterCardLevelInfoDatas.Count; i++)
        {
            string key = name + (i + 2);  //키 예제) 2부터 시작 - 주몽2, 주몽3 ...
            if (!dicCharacterCardLevelInfoDatas.ContainsKey(key)) continue;

            txtDescriptionLevel[i].text = "Lv. " + (i + 2);
            txtDescriptions[i].text = string.Format(dicCharacterCardLevelInfoDatas[key].description, dicCharacterCardLevelInfoDatas[key].increase);
        }

        btnUpgrade.onClick.RemoveAllListeners();
        btnUpgrade.onClick.AddListener(() => {
            if (level > dicCharacterCardLevelInfoDatas.Count) return;
            PlayFabManager.instance.UpgradeCharacterCard(name, this, uiCharacter);
        });

        InfoManager.GetInstance().SaveCharacterInfo(name, level);

        var characterInfo = InfoManager.GetInstance().LoadCharacterInfo(name);
        txtStatsDamage.text = characterInfo.damage.ToString();
        txtStatsAttackSpeed.text = characterInfo.attackSpeed.ToString();
        txtStatsAttackRange.text = characterInfo.attackRange.ToString();
        txtStatsMoveSpeed.text = characterInfo.moveSpeed.ToString();
    }

    private void SetSkillInfoButton(string name)
    {
        Dictionary<string, CharacterData> dicCharacterDatas = DataManager.GetInstance().GetCharacterData();
        Dictionary<string, CharacterSkillData> dicSkillDatas = DataManager.GetInstance().GetCharacterSkillData();

        List<CharacterSkillData> skillDatas = new List<CharacterSkillData>();
        if (dicSkillDatas.ContainsKey(dicCharacterDatas[name].skill_1_name)) skillDatas.Add(dicSkillDatas[dicCharacterDatas[name].skill_1_name]);
        if (dicSkillDatas.ContainsKey(dicCharacterDatas[name].skill_2_name)) skillDatas.Add(dicSkillDatas[dicCharacterDatas[name].skill_2_name]);
        if (dicSkillDatas.ContainsKey(dicCharacterDatas[name].skill_3_name)) skillDatas.Add(dicSkillDatas[dicCharacterDatas[name].skill_3_name]);

        for (int i = 0; i < skillDatas.Count; i++)
        {
            btnSkillInfos[i].SetActive(true);
            btnSkillInfos[i].GetComponent<UISkillInfo>().Set(skillDatas[i], dicCharacterDatas[name].damage.ToString());
        }
    }

    private void ResetData()
    {
        for (int i = 0; i < btnSkillInfos.Length; i++)
        {
            btnSkillInfos[i].SetActive(false);
        }

        for (int i = 0; i < imgBgLevel.Length; i++)
        {
            imgBgLevel[i].color = defaultColor;
            imgBgDescription[i].color = defaultColor;
        }

        for (int i = 0; i < txtDescriptionLevel.Length; i++)
        {
            txtDescriptionLevel[i].text = string.Empty;
            txtDescriptions[i].text = string.Empty;
        }
    }
}
