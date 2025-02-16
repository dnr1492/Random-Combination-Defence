using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class ToolManager
{
    private static void LoadDataFromJSON<T>(T data, string fileName)
    {
        //Resources/Datas ���
        string path = Path.Combine(Application.dataPath, "Resources/Datas");
        string jsonPath = Path.Combine(path, fileName);

        //JSON ���� ����
        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(jsonPath, jsonData);

        Debug.Log($"JSON data saved at: {jsonPath}");
        AssetDatabase.Refresh();
    }

    #region CharacterCardLevelInfoData
    private static string[] characterDisplayNames = Enum.GetNames(typeof(CharacterDisplayName));
    private static readonly Dictionary<string, Dictionary<int, CharacterCardLevelInfoData>> characterConfigs = new Dictionary<string, Dictionary<int, CharacterCardLevelInfoData>>()
        {
            #region Tier: ����
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
                "�ü�", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(�ü�) �׽�Ʈ 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(�ü�) �׽�Ʈ 2" } },
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
            }
            #endregion
    };

    [MenuItem("�����/Generate characterCardLevelInfo_data")]
    private static void GenerateCharacterCardLevelInfoData()
    {
        //JSON ������ ����
        CharacterCardDataList list = new CharacterCardDataList
        {
            characterCardDatas = new List<CharacterCardData>()
        };

        //������ �߰�
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

        LoadDataFromJSON(list, "characterCardLevelInfo_data.json");
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
    #endregion

    #region CharacterCardLevelData
    private static readonly int maxLevel = 12;

    [MenuItem("�����/Generate characterCardLevel_data")]
    private static void GenerateCharacterCardLevelData()
    {
        //JSON ������ ����
        CharacterCardLevelDataList list = new CharacterCardLevelDataList
        {
            characterCardLevelDatas = new List<CharacterCardLevelData>()
        };

        //������ �߰�
        for (int i = 1; i < maxLevel; i++)
        {
            list.characterCardLevelDatas.Add(new CharacterCardLevelData
            {
                level = i,
                quantityTierCommon = 10 * i,
                quantityTierUncommon = 8 * i,
                quantityTierRare = 6 * i,
                quantityTierUniqe = 4 * i,
                quantityTierLegendary = 2 * i
            });
        }

        LoadDataFromJSON(list, "characterCardLevel_data.json");
    }
    #endregion

    #region CharacterData
    [MenuItem("�����/Generate character_data")]
    private static void GenerateCharacterData()
    {
        //JSON ������ ����
        CharacterDataList list = new CharacterDataList
        {
            characterDatas = new List<CharacterData>()
        };

        //������ �߰�
        #region Tier: ����
        list.characterDatas.Add(new CharacterData
        {
            displayName = "����",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 1,
            classType = "",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "����",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 1,
            classType = "",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "������",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 1,
            classType = "",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "�ü�",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 1,
            classType = "",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "������",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 1,
            classType = "",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        #endregion

        LoadDataFromJSON(list, "character_data.json");
    }
    #endregion

    #region CharacterRecipeData
    [MenuItem("�����/Generate characterRecipe_data")]
    private static void GenerateCharacterRecipeData()
    {
        //JSON ������ ����
        CharacterRecipeDataList list = new CharacterRecipeDataList
        {
            characterRecipeDatas = new List<CharacterRecipeData>()
        };

        //������ �߰�
        #region Tier: ����
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "����",
            recipeNameA = "����",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "������"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "����",
            recipeNameA = "����",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "������"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "������",
            recipeNameA = "������",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "������"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "�ü�",
            recipeNameA = "����",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "������"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "������",
            recipeNameA = "����",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "������"
        });
        #endregion

        LoadDataFromJSON(list, "characterRecipe_data.json");
    }
    #endregion

    #region CharacterSkillData
    [MenuItem("�����/Generate characterSkill_data")]
    private static void GenerateCharacterSkillData()
    {
        //JSON ������ ����
        CharacterSkillDataList list = new CharacterSkillDataList
        {
            characterSkillDatas = new List<CharacterSkillData>()
        };

        //������ �߰�
        list.characterSkillDatas.Add(new CharacterSkillData
        {
            skillName = "ȭ�� ������",
            characterDisplayName = "�ָ�",
            skillDescription = "ȭ���� ������ {0}�������� �����ϴ�.",
            skillImagePath = "image/skill/�ָ�/bow",
            skillDamage = "{0} * 5",
            skillTriggerChance = 10f,
            skillEffect = "",
            skillBasic = true
        });
        list.characterSkillDatas.Add(new CharacterSkillData
        {
            skillName = "��ȭ�� ������",
            characterDisplayName = "�ָ�",
            skillDescription = "��ȭ���� ������ {0}�������� �����ϴ�.",
            skillImagePath = "image/skill/�ָ�/fireBow",
            skillDamage = "{0} * 10",
            skillTriggerChance = 5f,
            skillEffect = "�߰������� 5�ʰ� ��ų �������� 200%��ŭ ���� ���ظ� �����ϴ�.",
            skillBasic = false
        });

        LoadDataFromJSON(list, "characterSkill_data.json");
    }
    #endregion

    #region PlayWaveData
    [MenuItem("�����/Generate playWave_data")]
    private static void GeneratePlayWaveData()
    {
        //JSON ������ ����
        PlayWaveDataList list = new PlayWaveDataList
        {
            playWaveDatas = new List<PlayWaveData>()
        };

        //������ �߰�
        List<PlayWaveData> playWaveDatas = new List<PlayWaveData> {
            new PlayWaveData { wave = 1, waveEnemyCount = 35, waveTimer = 20, enemyHp = 112, enemyDefense = 5.3f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 5 },
            new PlayWaveData { wave = 2, waveEnemyCount = 35, waveTimer = 40, enemyHp = 125, enemyDefense = 5.6f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 3, waveEnemyCount = 35, waveTimer = 40, enemyHp = 140, enemyDefense = 6.5f, enemySpeed = 2.531287151f, enemyType = EnemyType.�ӵ���, characterDrawCount = 2 },
            new PlayWaveData { wave = 4, waveEnemyCount = 35, waveTimer = 40, enemyHp = 157, enemyDefense = 6.3f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 5, waveEnemyCount = 35, waveTimer = 40, enemyHp = 176, enemyDefense = 6.7f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 6, waveEnemyCount = 35, waveTimer = 40, enemyHp = 212, enemyDefense = 11.6f, enemySpeed = 1.351703008f, enemyType = EnemyType.�����, characterDrawCount = 2 },
            new PlayWaveData { wave = 7, waveEnemyCount = 35, waveTimer = 40, enemyHp = 221, enemyDefense = 7.5f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 8, waveEnemyCount = 35, waveTimer = 40, enemyHp = 247, enemyDefense = 8.0f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 9, waveEnemyCount = 35, waveTimer = 40, enemyHp = 277, enemyDefense = 8.4f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 10, waveEnemyCount = 35, waveTimer = 40, enemyHp = 930, enemyDefense = 18.0f, enemySpeed = 1.5f, enemyType = EnemyType.����, characterDrawCount = 2 },
            new PlayWaveData { wave = 11, waveEnemyCount = 35, waveTimer = 40, enemyHp = 347, enemyDefense = 9.5f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 12, waveEnemyCount = 35, waveTimer = 40, enemyHp = 389, enemyDefense = 10.1f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 13, waveEnemyCount = 35, waveTimer = 40, enemyHp = 408, enemyDefense = 10.0f, enemySpeed = 2.46317351f, enemyType = EnemyType.�ӵ���, characterDrawCount = 2 },
            new PlayWaveData { wave = 14, waveEnemyCount = 35, waveTimer = 40, enemyHp = 488, enemyDefense = 11.3f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 15, waveEnemyCount = 35, waveTimer = 40, enemyHp = 547, enemyDefense = 12.0f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 16, waveEnemyCount = 35, waveTimer = 40, enemyHp = 1002, enemyDefense = 12.9f, enemySpeed = 1.462924598f, enemyType = EnemyType.ü����, characterDrawCount = 2 },
            new PlayWaveData { wave = 17, waveEnemyCount = 35, waveTimer = 40, enemyHp = 686, enemyDefense = 13.5f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 18, waveEnemyCount = 35, waveTimer = 40, enemyHp = 768, enemyDefense = 14.3f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 19, waveEnemyCount = 35, waveTimer = 40, enemyHp = 861, enemyDefense = 15.1f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 20, waveEnemyCount = 35, waveTimer = 40, enemyHp = 2892, enemyDefense = 32.0f, enemySpeed = 1.5f, enemyType = EnemyType.����, characterDrawCount = 2 },
            new PlayWaveData { wave = 21, waveEnemyCount = 35, waveTimer = 40, enemyHp = 1080, enemyDefense = 17.0f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 22, waveEnemyCount = 35, waveTimer = 40, enemyHp = 1210, enemyDefense = 18.0f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 23, waveEnemyCount = 35, waveTimer = 40, enemyHp = 1477, enemyDefense = 25.0f, enemySpeed = 1.442758823f, enemyType = EnemyType.�����, characterDrawCount = 2 },
            new PlayWaveData { wave = 24, waveEnemyCount = 35, waveTimer = 40, enemyHp = 1517, enemyDefense = 20.2f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 25, waveEnemyCount = 35, waveTimer = 40, enemyHp = 1700, enemyDefense = 21.5f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 26, waveEnemyCount = 35, waveTimer = 40, enemyHp = 1830, enemyDefense = 30.4f, enemySpeed = 1.524025433f, enemyType = EnemyType.�����, characterDrawCount = 2 },
            new PlayWaveData { wave = 27, waveEnemyCount = 35, waveTimer = 40, enemyHp = 2132, enemyDefense = 24.1f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 28, waveEnemyCount = 35, waveTimer = 40, enemyHp = 2388, enemyDefense = 25.6f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 29, waveEnemyCount = 35, waveTimer = 40, enemyHp = 2674, enemyDefense = 27.1f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 30, waveEnemyCount = 35, waveTimer = 40, enemyHp = 8985, enemyDefense = 57.4f, enemySpeed = 1.5f, enemyType = EnemyType.����, characterDrawCount = 2 },
            new PlayWaveData { wave = 31, waveEnemyCount = 35, waveTimer = 40, enemyHp = 3355, enemyDefense = 30.4f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 32, waveEnemyCount = 35, waveTimer = 40, enemyHp = 3758, enemyDefense = 32.3f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 33, waveEnemyCount = 35, waveTimer = 40, enemyHp = 4391, enemyDefense = 52.6f, enemySpeed = 1.607457305f, enemyType = EnemyType.�����, characterDrawCount = 2 },
            new PlayWaveData { wave = 34, waveEnemyCount = 35, waveTimer = 40, enemyHp = 4714, enemyDefense = 36.3f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 35, waveEnemyCount = 35, waveTimer = 40, enemyHp = 5279, enemyDefense = 38.4f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 36, waveEnemyCount = 35, waveTimer = 40, enemyHp = 6095, enemyDefense = 62.4f, enemySpeed = 1.396646324f, enemyType = EnemyType.�����, characterDrawCount = 2 },
            new PlayWaveData { wave = 37, waveEnemyCount = 35, waveTimer = 40, enemyHp = 6623, enemyDefense = 43.2f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 38, waveEnemyCount = 35, waveTimer = 40, enemyHp = 7417, enemyDefense = 45.8f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 39, waveEnemyCount = 35, waveTimer = 40, enemyHp = 8308, enemyDefense = 48.5f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 40, waveEnemyCount = 35, waveTimer = 40, enemyHp = 27915, enemyDefense = 102.8f, enemySpeed = 1.5f, enemyType = EnemyType.����, characterDrawCount = 2 },
            new PlayWaveData { wave = 41, waveEnemyCount = 35, waveTimer = 40, enemyHp = 10421, enemyDefense = 54.5f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 42, waveEnemyCount = 35, waveTimer = 40, enemyHp = 11672, enemyDefense = 57.8f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 43, waveEnemyCount = 35, waveTimer = 40, enemyHp = 17561, enemyDefense = 58.3f, enemySpeed = 1.464049377f, enemyType = EnemyType.ü����, characterDrawCount = 2 },
            new PlayWaveData { wave = 44, waveEnemyCount = 35, waveTimer = 40, enemyHp = 14641, enemyDefense = 64.9f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 45, waveEnemyCount = 35, waveTimer = 40, enemyHp = 16398, enemyDefense = 68.8f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 46, waveEnemyCount = 35, waveTimer = 40, enemyHp = 17114, enemyDefense = 68.0f, enemySpeed = 2.384900744f, enemyType = EnemyType.�ӵ���, characterDrawCount = 2 },
            new PlayWaveData { wave = 47, waveEnemyCount = 35, waveTimer = 40, enemyHp = 20570, enemyDefense = 77.3f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 48, waveEnemyCount = 35, waveTimer = 40, enemyHp = 23039, enemyDefense = 82.0f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 49, waveEnemyCount = 35, waveTimer = 40, enemyHp = 25803, enemyDefense = 86.9f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 50, waveEnemyCount = 35, waveTimer = 40, enemyHp = 86700, enemyDefense = 184.2f, enemySpeed = 1.5f, enemyType = EnemyType.����, characterDrawCount = 2 },
            new PlayWaveData { wave = 51, waveEnemyCount = 35, waveTimer = 40, enemyHp = 32368, enemyDefense = 97.6f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 52, waveEnemyCount = 35, waveTimer = 40, enemyHp = 36252, enemyDefense = 103.5f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 53, waveEnemyCount = 35, waveTimer = 40, enemyHp = 42916, enemyDefense = 100.0f, enemySpeed = 2.325251523f, enemyType = EnemyType.�ӵ���, characterDrawCount = 2 },
            new PlayWaveData { wave = 54, waveEnemyCount = 35, waveTimer = 40, enemyHp = 45475, enemyDefense = 116.3f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 55, waveEnemyCount = 35, waveTimer = 40, enemyHp = 50932, enemyDefense = 123.3f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 56, waveEnemyCount = 35, waveTimer = 40, enemyHp = 52715, enemyDefense = 183.1f, enemySpeed = 1.396295769f, enemyType = EnemyType.�����, characterDrawCount = 2 },
            new PlayWaveData { wave = 57, waveEnemyCount = 35, waveTimer = 40, enemyHp = 63889, enemyDefense = 138.5f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 58, waveEnemyCount = 35, waveTimer = 40, enemyHp = 71555, enemyDefense = 146.8f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 59, waveEnemyCount = 35, waveTimer = 40, enemyHp = 80142, enemyDefense = 155.6f, enemySpeed = 1.5f, enemyType = EnemyType.�Ϲ�, characterDrawCount = 2 },
            new PlayWaveData { wave = 60, waveEnemyCount = 35, waveTimer = 40, enemyHp = 269277, enemyDefense = 329.8f, enemySpeed = 1.5f, enemyType = EnemyType.����, characterDrawCount = 2 }
        };

        list.playWaveDatas = playWaveDatas;

        LoadDataFromJSON(list, "playWave_data.json");
    }
    #endregion
}
#endif