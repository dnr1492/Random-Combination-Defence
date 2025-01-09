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

    public void SaveCharacterInfo(string name, int level)
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

        //1. ��ų ������ �� �⺻(Basic) ��ų �з�
        if (dicSkillDatas.ContainsKey(dicCharacterDatas[name].skill_1_name)) {
            skillDatas.Add(dicSkillDatas[dicCharacterDatas[name].skill_1_name]);
            if (dicSkillDatas[dicCharacterDatas[name].skill_1_name].skill_basic) unlockSkills.Add(dicSkillDatas[dicCharacterDatas[name].skill_1_name].skill_basic);
        }
        if (dicSkillDatas.ContainsKey(dicCharacterDatas[name].skill_2_name)) {
            skillDatas.Add(dicSkillDatas[dicCharacterDatas[name].skill_2_name]);
            if (dicSkillDatas[dicCharacterDatas[name].skill_2_name].skill_basic) unlockSkills.Add(dicSkillDatas[dicCharacterDatas[name].skill_2_name].skill_basic);
        }
        if (dicSkillDatas.ContainsKey(dicCharacterDatas[name].skill_3_name)) {
            skillDatas.Add(dicSkillDatas[dicCharacterDatas[name].skill_3_name]);
            if (dicSkillDatas[dicCharacterDatas[name].skill_3_name].skill_basic) unlockSkills.Add(dicSkillDatas[dicCharacterDatas[name].skill_3_name].skill_basic);
        }
        
        //2. �ش� ĳ���� ī���� ���� ������ ����
        for (int i = 0; i < dicCharacterCardLevelInfoDatas.Count; i++)
        {
            string key = name + (i + 2);  //Ű ����) �ָ�2, �ָ�3 ...
            if (!dicCharacterCardLevelInfoDatas.ContainsKey(key)) continue;

            if (level < i + 2) continue;
            if (dicCharacterCardLevelInfoDatas[key].description.Contains("���ݷ�")) addDamage += dicCharacterCardLevelInfoDatas[key].increase;
            if (dicCharacterCardLevelInfoDatas[key].description.Contains("���ݼӵ�")) addAttackSpeed += dicCharacterCardLevelInfoDatas[key].increase;
            if (dicCharacterCardLevelInfoDatas[key].description.Contains("���ݻ�Ÿ�")) addAttackRange += dicCharacterCardLevelInfoDatas[key].increase;
            if (dicCharacterCardLevelInfoDatas[key].description.Contains("�̵��ӵ�")) addMoveSpeed += dicCharacterCardLevelInfoDatas[key].increase;
            if (dicCharacterCardLevelInfoDatas[key].description.Contains("��ų")) unlockSkills.Add(true);
        }

        //3. ĳ���� ���� ����
        SaveCharacterInfo(level, 
            name,
            dicCharacterDatas[name].damage + addDamage,
            dicCharacterDatas[name].attack_speed + addAttackSpeed,
            dicCharacterDatas[name].attack_range + addAttackRange,
            dicCharacterDatas[name].move_speed + addMoveSpeed,
            skillDatas,
            unlockSkills
        );
    }

    private void SaveCharacterInfo(int level, string name, float damage, float attackSpeed, float attackRange, float moveSpeed, List<CharacterSkillData> skillDatas, List<bool> unlockSkills)
    {
        string path = Application.persistentDataPath + "/" + name + "_info.json";

        CharacterInfo characterInfo = new CharacterInfo(level, name, damage, attackSpeed, attackRange, moveSpeed, skillDatas, unlockSkills);
        string characterJson = JsonConvert.SerializeObject(characterInfo);

        //UTF8�� ��ȣȭ
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(characterJson);
        string encodedJson = System.Convert.ToBase64String(bytes);

        File.WriteAllText(path, encodedJson);
        DebugLogger.Log(Application.persistentDataPath + "\n" + name + " ���� ����");
    }

    public CharacterInfo LoadCharacterInfo(string name)
    {
        int index = name.IndexOf("(Clone)");
        if (index > 0) name = name.Substring(0, index);

        string path = Application.persistentDataPath + "/" + name + "_info.json";

        string characterJson = File.ReadAllText(path);

        //UTF8�� ��ȣȭ
        byte[] bytes = System.Convert.FromBase64String(characterJson);
        string decodedJson = System.Text.Encoding.UTF8.GetString(bytes);

        CharacterInfo characterInfo = JsonConvert.DeserializeObject<CharacterInfo>(decodedJson);
        return characterInfo;
    }
}
