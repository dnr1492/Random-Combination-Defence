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
    private static string[] characterDisplayNames = Enum.GetNames(typeof(PlayFabManager.CharacterDisplayName));
    private static readonly Dictionary<string, Dictionary<int, CharacterCardLevelInfoData>> characterConfigs = new Dictionary<string, Dictionary<int, CharacterCardLevelInfoData>>()
        {
            #region Tier: ����
            {
            "�˺�", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(�˺�) �׽�Ʈ 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(�˺�) �׽�Ʈ 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "â��", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(â��) �׽�Ʈ 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(â��) �׽�Ʈ 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "�ú�", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(�ú�) �׽�Ʈ 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(�ú�) �׽�Ʈ 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "���޺�", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(���޺�) �׽�Ʈ 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(���޺�) �׽�Ʈ 2" } },
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
                "å��", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(å��) �׽�Ʈ 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(å��) �׽�Ʈ 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            #endregion
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
        list.characterDatas.Add(new CharacterData
        {
            displayName = "�ָ�",
            damage = 30f,
            attackSpeed = 2f,
            attackRange = 5f,
            moveSpeed = 5f,
            tierNum = 5,
            classType = "�ú�",
            skill_1_name = "ȭ�� ������",
            skill_2_name = "��ȭ�� ������",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "�̼���",
            damage = 20f,
            attackSpeed = 2.5f,
            attackRange = 7f,
            moveSpeed = 5f,
            tierNum = 5,
            classType = "�ú�",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "������",
            damage = 15f,
            attackSpeed = 1.5f,
            attackRange = 4f,
            moveSpeed = 4f,
            tierNum = 4,
            classType = "å��",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "�����",
            damage = 10f,
            attackSpeed = 1.25f,
            attackRange = 3f,
            moveSpeed = 3f,
            tierNum = 3,
            classType = "â��",
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
            tierNum = 2,
            classType = "����",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        #region Tier: ����
        list.characterDatas.Add(new CharacterData
        {
            displayName = "�˺�",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 2,
            classType = "�˺�",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "â��",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 2,
            classType = "â��",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "�ú�",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 2,
            classType = "�ú�",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "���޺�",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 2,
            classType = "���޺�",
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
            tierNum = 2,
            classType = "������",
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
            tierNum = 2,
            classType = "����",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "å��",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 2,
            classType = "å��",
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
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "������",
            recipeNameA = "������",
            recipeNameB = "�����",
            recipeNameC = "������",
            resultName = "�ָ�"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "������",
            recipeNameA = "������",
            recipeNameB = "�����",
            recipeNameC = "������",
            resultName = "�̼���"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "�����",
            recipeNameA = "�����",
            recipeNameB = "�����",
            recipeNameC = "",
            resultName = "������"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "�����",
            recipeNameA = "�����",
            recipeNameB = "������",
            recipeNameC = "����",
            resultName = "������"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "������",
            recipeNameA = "������",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "�����"
        });
        #region Tier: ����
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "�˺�",
            recipeNameA = "�˺�",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "������"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "â��",
            recipeNameA = "â��",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "������"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "�ú�",
            recipeNameA = "�ú�",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "������"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "���޺�",
            recipeNameA = "���޺�",
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
            selectName = "����",
            recipeNameA = "����",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "������"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "å��",
            recipeNameA = "å��",
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
        list.playWaveDatas.Add(new PlayWaveData { waveId = 0, waveEnemyName = "Test_1", waveEnemyCount = 15, waveTimer = 15f });
        list.playWaveDatas.Add(new PlayWaveData { waveId = 1, waveEnemyName = "Test_2", waveEnemyCount = 15, waveTimer = 30f });
        list.playWaveDatas.Add(new PlayWaveData { waveId = 2, waveEnemyName = "Test_3", waveEnemyCount = 15, waveTimer = 30f });
        list.playWaveDatas.Add(new PlayWaveData { waveId = 3, waveEnemyName = "Test_4", waveEnemyCount = 15, waveTimer = 30f });
        list.playWaveDatas.Add(new PlayWaveData { waveId = 4, waveEnemyName = "Test_5", waveEnemyCount = 15, waveTimer = 30f });
        list.playWaveDatas.Add(new PlayWaveData { waveId = 5, waveEnemyName = "Test_Boss_1", waveEnemyCount = 1, waveTimer = 60f });
        list.playWaveDatas.Add(new PlayWaveData { waveId = 6, waveEnemyName = "Test_6", waveEnemyCount = 15, waveTimer = 30f });
        list.playWaveDatas.Add(new PlayWaveData { waveId = 7, waveEnemyName = "Test_7", waveEnemyCount = 15, waveTimer = 30f });
        list.playWaveDatas.Add(new PlayWaveData { waveId = 8, waveEnemyName = "Test_8", waveEnemyCount = 15, waveTimer = 30f });
        list.playWaveDatas.Add(new PlayWaveData { waveId = 9, waveEnemyName = "Test_9", waveEnemyCount = 15, waveTimer = 30f });
        list.playWaveDatas.Add(new PlayWaveData { waveId = 10, waveEnemyName = "Test_10", waveEnemyCount = 15, waveTimer = 30f });
        list.playWaveDatas.Add(new PlayWaveData { waveId = 11, waveEnemyName = "Test_Boss_2", waveEnemyCount = 1, waveTimer = 60f });

        LoadDataFromJSON(list, "playWave_data.json");
    }
    #endregion

    #region PlayMapData
    [MenuItem("�����/Generate playMap_data")]
    private static void GeneratePlayMapData()
    {
        //JSON ������ ����
        PlayMapDataList list = new PlayMapDataList
        {
            playMapDatas = new List<PlayMapData>()
        };

        //������ �߰�
        list.playMapDatas.Add(new PlayMapData { mapId = 10000, maximumPopulation = 15, maximumEnemyCount = 40 });

        LoadDataFromJSON(list, "playMap_data.json");
    }
    #endregion

    #region PlayEnemyData
    [MenuItem("�����/Generate playEnemy_data")]
    private static void GeneratePlayEnemyData()
    {
        //JSON ������ ����
        PlayEnemyDataList list = new PlayEnemyDataList
        {
            playEnemyDatas = new List<PlayEnemyData>()
        };

        //������ �߰�
        list.playEnemyDatas.Add(new PlayEnemyData { enemyName = "Test_1", enemySpeed = 5f, enemyHp = 10, dropGold = 1, dropDarkGold = 0 });
        list.playEnemyDatas.Add(new PlayEnemyData { enemyName = "Test_2", enemySpeed = 5.5f, enemyHp = 20, dropGold = 1, dropDarkGold = 0 });
        list.playEnemyDatas.Add(new PlayEnemyData { enemyName = "Test_3", enemySpeed = 6f, enemyHp = 30, dropGold = 1, dropDarkGold = 0 });
        list.playEnemyDatas.Add(new PlayEnemyData { enemyName = "Test_Boss_1", enemySpeed = 3f, enemyHp = 100, dropGold = 5, dropDarkGold = 1 });
        list.playEnemyDatas.Add(new PlayEnemyData { enemyName = "Test_4", enemySpeed = 6.5f, enemyHp = 40, dropGold = 1, dropDarkGold = 0 });
        list.playEnemyDatas.Add(new PlayEnemyData { enemyName = "Test_5", enemySpeed = 5f, enemyHp = 50, dropGold = 1, dropDarkGold = 0 });
        list.playEnemyDatas.Add(new PlayEnemyData { enemyName = "Test_6", enemySpeed = 5.5f, enemyHp = 60, dropGold = 1, dropDarkGold = 0 });
        list.playEnemyDatas.Add(new PlayEnemyData { enemyName = "Test_7", enemySpeed = 6f, enemyHp = 70, dropGold = 1, dropDarkGold = 0 });
        list.playEnemyDatas.Add(new PlayEnemyData { enemyName = "Test_8", enemySpeed = 6.5f, enemyHp = 80, dropGold = 1, dropDarkGold = 0 });
        list.playEnemyDatas.Add(new PlayEnemyData { enemyName = "Test_9", enemySpeed = 5f, enemyHp = 90, dropGold = 1, dropDarkGold = 0 });
        list.playEnemyDatas.Add(new PlayEnemyData { enemyName = "Test_10", enemySpeed = 5.5f, enemyHp = 100, dropGold = 1, dropDarkGold = 0 });
        list.playEnemyDatas.Add(new PlayEnemyData { enemyName = "Test_Boss_2", enemySpeed = 1f, enemyHp = 200, dropGold = 5, dropDarkGold = 1 });

        LoadDataFromJSON(list, "playEnemy_data.json");
    }
    #endregion
}
#endif