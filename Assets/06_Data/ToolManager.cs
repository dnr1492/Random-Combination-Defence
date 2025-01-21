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
        //Resources/Datas 경로
        string path = Path.Combine(Application.dataPath, "Resources/Datas");
        string jsonPath = Path.Combine(path, fileName);

        //JSON 파일 저장
        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(jsonPath, jsonData);

        Debug.Log($"JSON data saved at: {jsonPath}");
        AssetDatabase.Refresh();
    }

    #region CharacterCardLevelInfoData
    private static string[] characterDisplayNames = Enum.GetNames(typeof(PlayFabManager.CharacterDisplayName));
    private static readonly Dictionary<string, Dictionary<int, CharacterCardLevelInfoData>> characterConfigs = new Dictionary<string, Dictionary<int, CharacterCardLevelInfoData>>()
        {
            #region Tier: 흔한
            {
            "검병", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(검병) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(검병) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "창병", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(창병) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(창병) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "궁병", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(궁병) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(궁병) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "보급병", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(보급병) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(보급병) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "광전사", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(광전사) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(광전사) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "군사", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(군사) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(군사) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "책사", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(책사) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(책사) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            #endregion
            {
                "안흔한", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(안흔한) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(안흔한) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "희귀한", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(희귀한) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(희귀한) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "유일한", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(유일한) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(유일한) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "주몽", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(주몽) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(주몽) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            },
            {
                "이순신", new Dictionary<int, CharacterCardLevelInfoData>
                {
                    { 2, new CharacterCardLevelInfoData { level = 2, moveSpeed = 0.2f } },
                    { 3, new CharacterCardLevelInfoData { level = 3, skill = "(이순신) 테스트 1" } },
                    { 4, new CharacterCardLevelInfoData { level = 4, damage = 10 } },
                    { 5, new CharacterCardLevelInfoData { level = 5, attackSpeed = 0.1f } },
                    { 6, new CharacterCardLevelInfoData { level = 6, damage = 10 } },
                    { 7, new CharacterCardLevelInfoData { level = 7, damage = 10 } },
                    { 8, new CharacterCardLevelInfoData { level = 8, moveSpeed = 0.2f } },
                    { 9, new CharacterCardLevelInfoData { level = 9, attackSpeed = 0.1f } },
                    { 10, new CharacterCardLevelInfoData { level = 10, skill = "(이순신) 테스트 2" } },
                    { 11, new CharacterCardLevelInfoData { level = 11, damage = 20 } }
                }
            }
        };

    [MenuItem("정재욱/Generate characterCardLevelInfo_data")]
    private static void GenerateCharacterCardLevelInfoData()
    {
        //JSON 데이터 생성
        CharacterCardDataList list = new CharacterCardDataList
        {
            characterCardDatas = new List<CharacterCardData>()
        };

        //데이터 추가
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

        if (damage > 0) parts.Add($"공격력이 {damage}만큼 증가");
        if (attackSpeed > 0) parts.Add($"공격속도가 {attackSpeed * 100}%만큼 증가");
        if (moveSpeed > 0) parts.Add($"이동속도가 {moveSpeed * 100}%만큼 증가");
        if (!string.IsNullOrEmpty(skill)) parts.Add($"{skill} 획득");

        return string.Join(", ", parts);
    }
    #endregion

    #region CharacterCardLevelData
    private static readonly int maxLevel = 12;

    [MenuItem("정재욱/Generate characterCardLevel_data")]
    private static void GenerateCharacterCardLevelData()
    {
        //JSON 데이터 생성
        CharacterCardLevelDataList list = new CharacterCardLevelDataList
        {
            characterCardLevelDatas = new List<CharacterCardLevelData>()
        };

        //데이터 추가
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
    [MenuItem("정재욱/Generate character_data")]
    private static void GenerateCharacterData()
    {
        //JSON 데이터 생성
        CharacterDataList list = new CharacterDataList
        {
            characterDatas = new List<CharacterData>()
        };

        //데이터 추가
        list.characterDatas.Add(new CharacterData
        {
            displayName = "주몽",
            damage = 30f,
            attackSpeed = 2f,
            attackRange = 5f,
            moveSpeed = 5f,
            tierNum = 5,
            classType = "궁병",
            skill_1_name = "화살 날리기",
            skill_2_name = "불화살 날리기",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "이순신",
            damage = 20f,
            attackSpeed = 2.5f,
            attackRange = 7f,
            moveSpeed = 5f,
            tierNum = 5,
            classType = "궁병",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "유일한",
            damage = 15f,
            attackSpeed = 1.5f,
            attackRange = 4f,
            moveSpeed = 4f,
            tierNum = 4,
            classType = "책사",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "희귀한",
            damage = 10f,
            attackSpeed = 1.25f,
            attackRange = 3f,
            moveSpeed = 3f,
            tierNum = 3,
            classType = "창병",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "안흔한",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 2,
            classType = "군사",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        #region Tier: 흔한
        list.characterDatas.Add(new CharacterData
        {
            displayName = "검병",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 2,
            classType = "검병",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "창병",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 2,
            classType = "창병",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "궁병",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 2,
            classType = "궁병",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "보급병",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 2,
            classType = "보급병",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "광전사",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 2,
            classType = "광전사",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "군사",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 2,
            classType = "군사",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        list.characterDatas.Add(new CharacterData
        {
            displayName = "책사",
            damage = 7f,
            attackSpeed = 1f,
            attackRange = 2f,
            moveSpeed = 2f,
            tierNum = 2,
            classType = "책사",
            skill_1_name = "",
            skill_2_name = "",
            skill_3_name = ""
        });
        #endregion

        LoadDataFromJSON(list, "character_data.json");
    }
    #endregion

    #region CharacterRecipeData
    [MenuItem("정재욱/Generate characterRecipe_data")]
    private static void GenerateCharacterRecipeData()
    {
        //JSON 데이터 생성
        CharacterRecipeDataList list = new CharacterRecipeDataList
        {
            characterRecipeDatas = new List<CharacterRecipeData>()
        };

        //데이터 추가
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "유일한",
            recipeNameA = "유일한",
            recipeNameB = "희귀한",
            recipeNameC = "안흔한",
            resultName = "주몽"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "유일한",
            recipeNameA = "유일한",
            recipeNameB = "희귀한",
            recipeNameC = "안흔한",
            resultName = "이순신"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "희귀한",
            recipeNameA = "희귀한",
            recipeNameB = "희귀한",
            recipeNameC = "",
            resultName = "유일한"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "희귀한",
            recipeNameA = "희귀한",
            recipeNameB = "안흔한",
            recipeNameC = "흔한",
            resultName = "유일한"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "안흔한",
            recipeNameA = "안흔한",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "희귀한"
        });
        #region Tier: 흔한
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "검병",
            recipeNameA = "검병",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "안흔한"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "창병",
            recipeNameA = "창병",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "안흔한"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "궁병",
            recipeNameA = "궁병",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "안흔한"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "보급병",
            recipeNameA = "보급병",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "안흔한"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "광전사",
            recipeNameA = "광전사",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "안흔한"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "군사",
            recipeNameA = "군사",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "안흔한"
        });
        list.characterRecipeDatas.Add(new CharacterRecipeData
        {
            selectName = "책사",
            recipeNameA = "책사",
            recipeNameB = "",
            recipeNameC = "",
            resultName = "안흔한"
        });
        #endregion

        LoadDataFromJSON(list, "characterRecipe_data.json");
    }
    #endregion

    #region CharacterSkillData
    [MenuItem("정재욱/Generate characterSkill_data")]
    private static void GenerateCharacterSkillData()
    {
        //JSON 데이터 생성
        CharacterSkillDataList list = new CharacterSkillDataList
        {
            characterSkillDatas = new List<CharacterSkillData>()
        };

        //데이터 추가
        list.characterSkillDatas.Add(new CharacterSkillData
        {
            skillName = "화살 날리기",
            characterDisplayName = "주몽",
            skillDescription = "화살을 날려서 {0}데미지를 입힙니다.",
            skillImagePath = "image/skill/주몽/bow",
            skillDamage = "{0} * 5",
            skillTriggerChance = 10f,
            skillEffect = "",
            skillBasic = true
        });
        list.characterSkillDatas.Add(new CharacterSkillData
        {
            skillName = "불화살 날리기",
            characterDisplayName = "주몽",
            skillDescription = "불화살을 날려서 {0}데미지를 입힙니다.",
            skillImagePath = "image/skill/주몽/fireBow",
            skillDamage = "{0} * 10",
            skillTriggerChance = 5f,
            skillEffect = "추가적으로 5초간 스킬 데미지의 200%만큼 지속 피해를 입힙니다.",
            skillBasic = false
        });

        LoadDataFromJSON(list, "characterSkill_data.json");
    }
    #endregion

    #region PlayWaveData
    [MenuItem("정재욱/Generate playWave_data")]
    private static void GeneratePlayWaveData()
    {
        //JSON 데이터 생성
        PlayWaveDataList list = new PlayWaveDataList
        {
            playWaveDatas = new List<PlayWaveData>()
        };

        //데이터 추가
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
    [MenuItem("정재욱/Generate playMap_data")]
    private static void GeneratePlayMapData()
    {
        //JSON 데이터 생성
        PlayMapDataList list = new PlayMapDataList
        {
            playMapDatas = new List<PlayMapData>()
        };

        //데이터 추가
        list.playMapDatas.Add(new PlayMapData { mapId = 10000, maximumPopulation = 15, maximumEnemyCount = 40 });

        LoadDataFromJSON(list, "playMap_data.json");
    }
    #endregion

    #region PlayEnemyData
    [MenuItem("정재욱/Generate playEnemy_data")]
    private static void GeneratePlayEnemyData()
    {
        //JSON 데이터 생성
        PlayEnemyDataList list = new PlayEnemyDataList
        {
            playEnemyDatas = new List<PlayEnemyData>()
        };

        //데이터 추가
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