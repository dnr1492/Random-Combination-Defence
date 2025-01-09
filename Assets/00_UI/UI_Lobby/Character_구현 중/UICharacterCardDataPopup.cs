using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterCardDataPopup : MonoBehaviour
{
    [SerializeField] UICharacter uiCharacter;
    [SerializeField] UIGold uiGold;
    [SerializeField] Image bgCharacter, bgCharacterOutline, imgCharacter, imgQuantity;
    [SerializeField] Text txtQuantity, txtCurLevel, txtClass, txtName;
    [SerializeField] Text txtStatsDamage, txtStatsAttackSpeed, txtStatsAttackRange, txtStatsMoveSpeed;
    [SerializeField] Image[] imgBgLevel, imgBgDescription;
    [SerializeField] Text[] txtDescriptionLevel, txtDescriptions;
    [SerializeField] Button btnClose, btnExternalClose, btnUpgrade;
    [SerializeField] GameObject[] btnSkillInfos;
    [SerializeField] Text txtGoldPrice;

    private Color defaultColor;
    private Color gainColor;

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
        string tierName = null;
        foreach (PlayFabManager.CharacterTier tier in Enum.GetValues(typeof(PlayFabManager.CharacterTier))) {
            if ((int)tier == characterData[displayName].tier) tierName = tier.ToString();
        }
        int level = int.Parse(curLevel);

        bgCharacter.sprite = SpriteManager.GetInstance().GetSprite(SpriteManager.SpriteType.Bg, tierName);
        bgCharacterOutline.sprite = SpriteManager.GetInstance().GetSprite(SpriteManager.SpriteType.BgOutline, tierName);
        imgCharacter.sprite = SpriteManager.GetInstance().GetSprite(SpriteManager.SpriteType.ImgCharacter, displayName);
        imgQuantity.fillAmount = (float)curQuantity / requiredLevelUpQuantity;
        txtQuantity.text = curQuantity + "/" + requiredLevelUpQuantity;
        txtCurLevel.text = "Lv. " + level;
        //txtClass.text = ���Ŀ� Class(����)�� �����ϸ� �Ҵ��ϱ� (***ItemClass(Tier)���� �ٸ� ������)
        txtName.text = displayName;
        txtGoldPrice.text = "2000";  //���Ŀ� �������� ������ �����ؼ� �Ҵ��ϱ� ex) int.Parse(txtGoldPrice.text) * 2 �Ǵ� int.Parse(txtGoldPrice.text) * level

        SetSkillInfoButton(displayName);

        // ����������� addSkills�� Ȱ���Ͽ� ���� ȹ���� ��ų �߰� ���� �����ϱ� ����������� //
        //                                                                                                    //
        //                                                                                                    //
        //                                                                                                    //
        // ����������� addSkills�� Ȱ���Ͽ� ���� ȹ���� ��ų �߰� ���� �����ϱ� ����������� //

        for (int i = 1; i < level; i++)
        {
            imgBgLevel[i - 1].color = gainColor;
            imgBgDescription[i - 1].color = gainColor;
        }

        Dictionary<string, CharacterCardLevelInfoData> dicCharacterCardLevelInfoDatas = DataManager.GetInstance().GetCharacterCardLevelInfoData();
        for (int i = 0; i < dicCharacterCardLevelInfoDatas.Count; i++)
        {
            string key = displayName + (i + 2);  //Ű ����) 2���� ���� - �ָ�2, �ָ�3 ...
            if (!dicCharacterCardLevelInfoDatas.ContainsKey(key)) continue;

            txtDescriptionLevel[i].text = "Lv. " + (i + 2);
            txtDescriptions[i].text = string.Format(dicCharacterCardLevelInfoDatas[key].description, dicCharacterCardLevelInfoDatas[key].increase);
        }

        int levelUpRemainingUses = DataManager.GetInstance().GetCharacterCardLevelQuentityData(level, characterData[displayName].tier);
        if (IsMaxLevel(levelUpRemainingUses)) btnUpgrade.interactable = false;
        else btnUpgrade.interactable = true;
        btnUpgrade.onClick.RemoveAllListeners();
        btnUpgrade.onClick.AddListener(async () => {
            await PlayFabManager.instance.UpgradeCharacterCardLevelToGoldAsync(this, uiCharacter, uiGold, displayName, int.Parse(txtGoldPrice.text), levelUpRemainingUses);
        });

        InfoManager.GetInstance().SaveCharacterInfo(displayName, level);

        var characterInfo = InfoManager.GetInstance().LoadCharacterInfo(displayName);
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

        for (int i = 0; i < skillDatas.Count; i++) {
            btnSkillInfos[i].SetActive(true);
            btnSkillInfos[i].GetComponent<UISkillInfo>().Set(skillDatas[i], dicCharacterDatas[name].damage.ToString());
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
            Debug.Log("������ ĳ���� ī�尡 �ִ� �����Դϴ�.");
            return true;
        }
        else {
            //Debug.Log("������ �䱸�� : " + levelUpRemainingUses);
            return false;
        }
    }
}
