using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ToolManager
{
    private static string[] characterDisplayNames = { "����", "������", "�����", "������", "�ָ�", "�̼���" };
    private static readonly Dictionary<string, Dictionary<int, CharacterCardLevelInfoData>> characterConfigs = new Dictionary<string, Dictionary<int, CharacterCardLevelInfoData>>()
        {
            {
                "����", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(����) �׽�Ʈ 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(����) �׽�Ʈ 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "������", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(������) �׽�Ʈ 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(������) �׽�Ʈ 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "�����", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(�����) �׽�Ʈ 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(�����) �׽�Ʈ 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "������", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(������) �׽�Ʈ 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(������) �׽�Ʈ 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "�ָ�", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(�ָ�) �׽�Ʈ 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(�ָ�) �׽�Ʈ 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "�̼���", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(�̼���) �׽�Ʈ 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(�̼���) �׽�Ʈ 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            }
        };
    private static readonly int InitLevel = 2;
    private static readonly int maxLevel = 11;

    [MenuItem("�����/Generate characterCardLevelInfo_data")]
    private static void GenerateCharacterData()
    {
        //JSON ������ ����
        CharacterCardDataList list = new CharacterCardDataList
        {
            characterCardDatas = new List<CharacterCardData>()
        };

        foreach (string displayName in characterDisplayNames)
        {
            CharacterCardData character = new CharacterCardData
            {
                display_name = displayName,
                levels = new List<CharacterCardLevelInfoData>()
            };

            if (characterConfigs.TryGetValue(displayName, out var levelDataMap))
            {
                foreach (var levelDataEntry in levelDataMap)
                {
                    int level = levelDataEntry.Key;
                    CharacterCardLevelInfoData data = levelDataEntry.Value;

                    character.levels.Add(new CharacterCardLevelInfoData
                    {
                        level = level,
                        damage = data.damage,
                        attackSpeed = data.attackSpeed,
                        moveSpeed = data.moveSpeed,
                        skill = data.skill,
                        description = GenerateDescription(data.damage, data.attackSpeed, data.moveSpeed, data.skill)
                    });
                }
            }

            list.characterCardDatas.Add(character);
        }

        //Resources/Datas ���
        string path = Path.Combine(Application.dataPath, "Resources/Datas");
        string jsonPath = Path.Combine(path, "characterCardLevelInfo_data.json");

        //JSON ���� ����
        string jsonData = JsonUtility.ToJson(list, true);
        File.WriteAllText(jsonPath, jsonData);

        Debug.Log($"JSON data saved at: {jsonPath}");
        AssetDatabase.Refresh();
    }

    private static string GenerateDescription(int damage, float attackSpeed, float moveSpeed, string skill)
    {
        List<string> parts = new List<string>();

        if (damage > 0) parts.Add($"���ݷ��� {damage}��ŭ ����");
        if (attackSpeed > 0) parts.Add($"���ݼӵ��� {attackSpeed * 100}%��ŭ ����");
        if (moveSpeed > 0) parts.Add($"�̵��ӵ��� {moveSpeed * 100}%��ŭ ����");
        if (!string.IsNullOrEmpty(skill)) parts.Add($"{skill} ȹ��");

        return string.Join(", ", parts);
    }
}