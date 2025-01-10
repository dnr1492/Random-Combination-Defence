using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class InfoManager : MonoBehaviour
{
    public static InfoManager instance = null;

    private InfoManager() { }

    public static InfoManager GetInstance()
    {
        if (instance == null) instance = new InfoManager();
        return instance;
    }

    public void SaveCharacterInfo(string displayName, int level)
    {
        var dicCharacterCardLevelInfoDatas = DataManager.GetInstance().GetCharacterCardLevelInfoData();
        var dicCharacterDatas = DataManager.GetInstance().GetCharacterData();
        var dicSkillDatas = DataManager.GetInstance().GetCharacterSkillData();

        float addDamage = 0;
        float addAttackSpeed = 0;
        float addAttackRange = 0;
        float addMoveSpeed = 0;
        List<bool> unlockSkills = new List<bool>();
        List<CharacterSkillData> skillDatas = new List<CharacterSkillData>();

        //1. 스킬 데이터 및 기본(Basic) 스킬 분류
        if (dicSkillDatas.ContainsKey(dicCharacterDatas[displayName].skill_1_name)) {
            skillDatas.Add(dicSkillDatas[dicCharacterDatas[displayName].skill_1_name]);
            if (dicSkillDatas[dicCharacterDatas[displayName].skill_1_name].skill_basic) unlockSkills.Add(dicSkillDatas[dicCharacterDatas[displayName].skill_1_name].skill_basic);
        }
        if (dicSkillDatas.ContainsKey(dicCharacterDatas[displayName].skill_2_name)) {
            skillDatas.Add(dicSkillDatas[dicCharacterDatas[displayName].skill_2_name]);
            if (dicSkillDatas[dicCharacterDatas[displayName].skill_2_name].skill_basic) unlockSkills.Add(dicSkillDatas[dicCharacterDatas[displayName].skill_2_name].skill_basic);
        }
        if (dicSkillDatas.ContainsKey(dicCharacterDatas[displayName].skill_3_name)) {
            skillDatas.Add(dicSkillDatas[dicCharacterDatas[displayName].skill_3_name]);
            if (dicSkillDatas[dicCharacterDatas[displayName].skill_3_name].skill_basic) unlockSkills.Add(dicSkillDatas[dicCharacterDatas[displayName].skill_3_name].skill_basic);
        }
        
        //2. 해당 캐릭터 카드의 레벨 데이터 정리
        for (int i = 0; i < dicCharacterCardLevelInfoDatas.Count; i++)
        {
            string key = displayName + (i + 2);  //키 예제) 주몽2, 주몽3 ...
            if (!dicCharacterCardLevelInfoDatas.ContainsKey(key)) continue;

            if (level < i + 2) continue;
            if (dicCharacterCardLevelInfoDatas[key].description.Contains("공격력")) addDamage += dicCharacterCardLevelInfoDatas[key].increase;
            if (dicCharacterCardLevelInfoDatas[key].description.Contains("공격속도")) addAttackSpeed += dicCharacterCardLevelInfoDatas[key].increase;
            if (dicCharacterCardLevelInfoDatas[key].description.Contains("공격사거리")) addAttackRange += dicCharacterCardLevelInfoDatas[key].increase;
            if (dicCharacterCardLevelInfoDatas[key].description.Contains("이동속도")) addMoveSpeed += dicCharacterCardLevelInfoDatas[key].increase;
            if (dicCharacterCardLevelInfoDatas[key].description.Contains("스킬")) unlockSkills.Add(true);
        }

        //3. 캐릭터 정보 저장
        SaveCharacterInfo(level, 
            displayName,
            dicCharacterDatas[displayName].damage + addDamage,
            dicCharacterDatas[displayName].attack_speed + addAttackSpeed,
            dicCharacterDatas[displayName].attack_range + addAttackRange,
            dicCharacterDatas[displayName].move_speed + addMoveSpeed,
            skillDatas,
            unlockSkills
        );
    }

    private void SaveCharacterInfo(int level, string displayName, float damage, float attackSpeed, float attackRange, float moveSpeed, List<CharacterSkillData> skillDatas, List<bool> unlockSkills)
    {
        string path = Application.persistentDataPath + "/" + displayName + "_info.json";

        CharacterInfo characterInfo = new CharacterInfo(level, displayName, damage, attackSpeed, attackRange, moveSpeed, skillDatas, unlockSkills);
        string characterJson = JsonConvert.SerializeObject(characterInfo);

        //UTF8로 암호화
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(characterJson);
        string encodedJson = System.Convert.ToBase64String(bytes);

        File.WriteAllText(path, encodedJson);
        DebugLogger.Log(Application.persistentDataPath + "\n" + displayName + " 정보 저장");
    }

    public CharacterInfo LoadCharacterInfo(string displayName)
    {
        int index = displayName.IndexOf("(Clone)");
        if (index > 0) displayName = displayName.Substring(0, index);

        string path = Application.persistentDataPath + "/" + displayName + "_info.json";

        string characterJson = File.ReadAllText(path);

        //UTF8로 복호화
        byte[] bytes = System.Convert.FromBase64String(characterJson);
        string decodedJson = System.Text.Encoding.UTF8.GetString(bytes);

        CharacterInfo characterInfo = JsonConvert.DeserializeObject<CharacterInfo>(decodedJson);
        return characterInfo;
    }
}
