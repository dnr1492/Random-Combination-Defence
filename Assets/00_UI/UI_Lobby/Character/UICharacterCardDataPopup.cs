using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterCardDataPopup : MonoBehaviour
{
    [SerializeField] UICharacter uiCharacter;
    [SerializeField] UIGold uiGold;
    [SerializeField] Image imgCharacter, imgQuantity;
    [SerializeField] Text txtQuantity, txtCurLevel, /*txtClassType,*/ txtDisplayName;
    [SerializeField] Text txtStatsDamage, txtStatsAttackSpeed, txtStatsAttackRange, txtStatsMoveSpeed;
    [SerializeField] Image[] imgBgLevel, imgBgDescription;
    [SerializeField] Text[] txtDescriptionLevel, txtDescriptions;
    [SerializeField] Button btnClose, btnExternalClose, btnUpgrade;
    [SerializeField] GameObject[] btnSkillInfos;
    [SerializeField] Text txtGoldPrice;

    private Color defaultColor;
    private Color gainColor;
    private int indexLevelOffset = 2;

    private void Awake()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < btnSkillInfos.Length; i++) btnSkillInfos[i].SetActive(false);

        defaultColor = ColorManager.HexToColor("9F9386");
        gainColor = ColorManager.HexToColor("EADED1");
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

    public void Open(string curLevel, string displayName, int curQuantity, int requiredLevelUpQuantity)
    {
        ResetData();

        var characterData = DataManager.GetInstance().GetCharacterData();
        int level = int.Parse(curLevel);

        imgCharacter.sprite = SpriteManager.GetInstance().GetSprite(SpriteType.ImgCharacter, displayName);
        imgQuantity.fillAmount = (float)curQuantity / requiredLevelUpQuantity;
        txtQuantity.text = curQuantity + "/" + requiredLevelUpQuantity;
        txtCurLevel.text = "Lv. " + level;
        //txtClassType.text = characterData[displayName].classType;
        txtDisplayName.text = displayName;
        txtGoldPrice.text = "2000";  //추후에 동적으로 증가율 설정해서 할당하기 ex) int.Parse(txtGoldPrice.text) * 2 또는 int.Parse(txtGoldPrice.text) * level

        SetSkillInfoButton(displayName);

        // ↓↓↓↓↓↓↓↓↓↓ addSkills을 활용하여 추후 획득한 스킬 추가 로직 구현하기 ↓↓↓↓↓↓↓↓↓↓ //
        //                                                                                                    //
        //                                                                                                    //
        // ↑↑↑↑↑↑↑↑↑↑ addSkills을 활용하여 추후 획득한 스킬 추가 로직 구현하기 ↑↑↑↑↑↑↑↑↑↑ //

        for (int i = 1; i < level; i++)
        {
            imgBgLevel[i - 1].color = gainColor;
            imgBgDescription[i - 1].color = gainColor;
        }

        var dicCharacterCardLevelInfoDatas = DataManager.GetInstance().GetCharacterCardLevelInfoData();
        CharacterCardData foundCharacterCardData = null;
        foreach (var dict in dicCharacterCardLevelInfoDatas) {
            if (dict.ContainsKey(displayName)) {
                foundCharacterCardData = dict[displayName];
                break;
            }
        }
        if (foundCharacterCardData != null) {
            foreach (var lv in foundCharacterCardData.levels) {
                txtDescriptionLevel[lv.level - indexLevelOffset].text = "Lv. " + lv.level.ToString();
                txtDescriptions[lv.level - indexLevelOffset].text = lv.description;
                DebugLogger.Log($"레벨 {lv.level}: {lv.description}, 공격력: {lv.damage}, 공격속도: {lv.attackSpeed}, 이동속도: {lv.moveSpeed}, 스킬: {lv.skill}");
            }
        }

        int levelUpRemainingUses = DataManager.GetInstance().GetCharacterCardLevelQuentityData(level, characterData[displayName].tierNum);
        if (IsMaxLevel(levelUpRemainingUses)) btnUpgrade.interactable = false;
        else btnUpgrade.interactable = true;
        btnUpgrade.onClick.RemoveAllListeners();
        btnUpgrade.onClick.AddListener(() => {
            string tempDisplayname = displayName;
            int tempLevelUpRemainingUses = levelUpRemainingUses;
            UpgradeAsync(tempDisplayname, tempLevelUpRemainingUses);
        });

        InfoManager.GetInstance().SaveCharacterInfo(displayName, level);

        var characterInfo = InfoManager.GetInstance().LoadCharacterInfo(displayName);
        txtStatsDamage.text = characterInfo.damage.ToString();
        txtStatsAttackSpeed.text = characterInfo.attackSpeed.ToString();
        txtStatsAttackRange.text = characterInfo.attackRange.ToString();
        txtStatsMoveSpeed.text = characterInfo.moveSpeed.ToString();
    }

    private void SetSkillInfoButton(string displayName)
    {
        Dictionary<string, CharacterData> dicCharacterDatas = DataManager.GetInstance().GetCharacterData();
        Dictionary<string, CharacterSkillData> dicSkillDatas = DataManager.GetInstance().GetCharacterSkillData();

        List<CharacterSkillData> skillDatas = new List<CharacterSkillData>();
        if (dicSkillDatas.ContainsKey(dicCharacterDatas[displayName].skill_1_name)) skillDatas.Add(dicSkillDatas[dicCharacterDatas[displayName].skill_1_name]);
        if (dicSkillDatas.ContainsKey(dicCharacterDatas[displayName].skill_2_name)) skillDatas.Add(dicSkillDatas[dicCharacterDatas[displayName].skill_2_name]);
        if (dicSkillDatas.ContainsKey(dicCharacterDatas[displayName].skill_3_name)) skillDatas.Add(dicSkillDatas[dicCharacterDatas[displayName].skill_3_name]);

        for (int i = 0; i < skillDatas.Count; i++) {
            btnSkillInfos[i].SetActive(true);
            btnSkillInfos[i].GetComponent<UISkillInfo>().Set(skillDatas[i], dicCharacterDatas[displayName].damage.ToString());
        }
    }

    private void ResetData()
    {
        for (int i = 0; i < btnSkillInfos.Length; i++) {
            btnSkillInfos[i].SetActive(false);
        }

        for (int i = 0; i < imgBgLevel.Length; i++) {
            imgBgLevel[i].color = defaultColor;
            imgBgDescription[i].color = defaultColor;
        }

        for (int i = 0; i < txtDescriptionLevel.Length; i++) {
            txtDescriptionLevel[i].text = string.Empty;
            txtDescriptions[i].text = string.Empty;
        }
    }

    private bool IsMaxLevel(int levelUpRemainingUses)
    {
        if (levelUpRemainingUses == 0) {
            DebugLogger.Log("유저의 캐릭터 카드가 최대 레벨입니다.");
            return true;
        }
        else {
            DebugLogger.Log("카드 레벨업 요구량 : " + levelUpRemainingUses);
            return false;
        }
    }

    private async void UpgradeAsync(string displayName, int levelUpRemainingUses)
    {
        try {
            LoadingManager.ShowLoading();
            await PlayFabManager.instance.UpgradeCharacterCardLevelToGoldAsync(this, uiCharacter, uiGold, displayName, int.Parse(txtGoldPrice.text), levelUpRemainingUses);
        }
        finally {
            LoadingManager.HideLoading();
        }
    }
}
