using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    private static DataManager instance = null;

    private DataManager() { }

    public static DataManager GetInstance()
    {
        if (instance == null) instance = new DataManager();
        return instance;
    }

    private Dictionary<int, CharacterCardLevelData> dicCharacterCardLevelDatas;
    private List<Dictionary<string, CharacterCardData>> characterCardLevelInfoDatas;
    private Dictionary<string, CharacterData> dicCharacterDatas;
    private Dictionary<string, CharacterSkillData> dicCharacterSkillDatas;
    private Dictionary<string, List<CharacterRecipeData>> dicCharacterRecipeDatas;
    private Dictionary<int, PlayWaveData> dicPlayWaveDatas;
    private Dictionary<int, PlayMapData> dicPlayMapDatas;
    private Dictionary<string, PlayEnemyData> dicPlayEnemyDatas;

    public void LoadCharacterCardLevelData()
    {
        dicCharacterCardLevelDatas = new Dictionary<int, CharacterCardLevelData>();
        string json = Resources.Load<TextAsset>("Datas/characterCardLevel_data").text;
        CharacterCardLevelDataList characterCardLevelDataList = JsonConvert.DeserializeObject<CharacterCardLevelDataList>(json);
        foreach (var data in characterCardLevelDataList.characterCardLevelDatas)
        {
            if (!dicCharacterCardLevelDatas.ContainsKey(data.level)) {
                dicCharacterCardLevelDatas.Add(data.level, data);
            }
        }
    }

    public void LoadCharacterCardLevelInfoData()
    {
        characterCardLevelInfoDatas = new List<Dictionary<string, CharacterCardData>>();
        string json = Resources.Load<TextAsset>("Datas/characterCardLevelInfo_data").text;
        CharacterCardDataList characterCardDataList = JsonConvert.DeserializeObject<CharacterCardDataList>(json);
        Dictionary<string, CharacterCardData> dicCharacterCardData = new Dictionary<string, CharacterCardData>();
        foreach (var character in characterCardDataList.characterCardDatas)
        {
            if (!dicCharacterCardData.ContainsKey(character.display_name)) {
                dicCharacterCardData.Add(character.display_name, character);

                //디버그 출력
                DebugLogger.Log($"캐릭터 이름: {character.display_name}");
                foreach (var level in character.levels) {
                    DebugLogger.Log($"레벨 {level.level}: {level.description}");
                }
            }
        }
        characterCardLevelInfoDatas.Add(dicCharacterCardData);
    }

    public void LoadCharacterData()
    {
        dicCharacterDatas = new Dictionary<string, CharacterData>();
        string json = Resources.Load<TextAsset>("Datas/character_data").text;
        CharacterDataList characterDataList = JsonConvert.DeserializeObject<CharacterDataList>(json);
        foreach (CharacterData charactersData in characterDataList.characterDatas)
        {
            if (!dicCharacterDatas.ContainsKey(charactersData.displayName)) {
                dicCharacterDatas.Add(charactersData.displayName, charactersData);
            }
        }
    }

    public void LoadCharacterSkillData()
    {
        dicCharacterSkillDatas = new Dictionary<string, CharacterSkillData>();
        string json = Resources.Load<TextAsset>("Datas/characterSkill_data").text;
        CharacterSkillDataList CharacterSkillDataList = JsonConvert.DeserializeObject<CharacterSkillDataList>(json);
        foreach (CharacterSkillData characterSkillData in CharacterSkillDataList.characterSkillDatas)
        {
            if (!dicCharacterSkillDatas.ContainsKey(characterSkillData.skillName)) {
                dicCharacterSkillDatas.Add(characterSkillData.skillName, characterSkillData);
            }
        }
    }

    public void LoadCharacterRecipeData()
    {
        dicCharacterRecipeDatas = new Dictionary<string, List<CharacterRecipeData>>();
        string json = Resources.Load<TextAsset>("Datas/characterRecipe_data").text;
        CharacterRecipeDataList characterRecipeDataList = JsonConvert.DeserializeObject<CharacterRecipeDataList>(json);
        foreach (CharacterRecipeData characterRecipeData in characterRecipeDataList.characterRecipeDatas)
        {
            ////성능 개선 전
            //if (dicCharacterRecipeDatas.ContainsKey(characterRecipeData.selectName)) {
            //    dicCharacterRecipeDatas[characterRecipeData.selectName].Add(characterRecipeData);
            //}
            //else dicCharacterRecipeDatas[characterRecipeData.selectName] = new List<CharacterRecipeData> { characterRecipeData };

            //성능 개선 후
            if (!dicCharacterRecipeDatas.TryGetValue(characterRecipeData.selectName, out var recipeList)) {
                recipeList = new List<CharacterRecipeData>();
                dicCharacterRecipeDatas[characterRecipeData.selectName] = recipeList;
            }
            recipeList.Add(characterRecipeData);
        }
    }

    public void LoadPlayWaveData()
    {
        dicPlayWaveDatas = new Dictionary<int, PlayWaveData>();
        string json = Resources.Load<TextAsset>("Datas/playWave_data").text;
        PlayWaveDataList PlayWaveDataList = JsonConvert.DeserializeObject<PlayWaveDataList>(json);
        foreach (PlayWaveData playWaveData in PlayWaveDataList.playWaveDatas)
        {
            if (!dicPlayWaveDatas.ContainsKey(playWaveData.waveId)) {
                dicPlayWaveDatas.Add(playWaveData.waveId, playWaveData);
            }
        }
    }

    public void LoadPlayMapData()
    {
        dicPlayMapDatas = new Dictionary<int, PlayMapData>();
        string json = Resources.Load<TextAsset>("Datas/playMap_data").text;
        PlayMapDataList playMapDataList = JsonConvert.DeserializeObject<PlayMapDataList>(json);
        foreach (PlayMapData playMapData in playMapDataList.playMapDatas)
        {
            if (!dicPlayMapDatas.ContainsKey(playMapData.mapId)) {
                dicPlayMapDatas.Add(playMapData.mapId, playMapData);
            }
        }
    }

    public void LoadPlayEnemyData()
    {
        dicPlayEnemyDatas = new Dictionary<string, PlayEnemyData>();
        string json = Resources.Load<TextAsset>("Datas/playEnemy_data").text;
        PlayEnemyDataList playEnemyDataList = JsonConvert.DeserializeObject<PlayEnemyDataList>(json);
        foreach (PlayEnemyData playEnemyData in playEnemyDataList.playEnemyDatas)
        {
            if (!dicPlayEnemyDatas.ContainsKey(playEnemyData.enemyName)) {
                dicPlayEnemyDatas.Add(playEnemyData.enemyName, playEnemyData);
            }
        }
    }

    public List<Dictionary<string, CharacterCardData>> GetCharacterCardLevelInfoData() => characterCardLevelInfoDatas;

    public Dictionary<string, CharacterData> GetCharacterData() => dicCharacterDatas;

    public Dictionary<string, CharacterSkillData> GetCharacterSkillData() => dicCharacterSkillDatas;

    public Dictionary<string, List<CharacterRecipeData>> GetCharacterRecipeData() => dicCharacterRecipeDatas;

    public Dictionary<int, PlayWaveData> GetPlayWaveData() => dicPlayWaveDatas;

    public Dictionary<int, PlayMapData> GetPlayMapData() => dicPlayMapDatas;

    public Dictionary<string, PlayEnemyData> GetPlayEnemyData() => dicPlayEnemyDatas;

    public int GetCharacterCardLevelQuentityData(int level, int tierNum)
    {
        if (dicCharacterCardLevelDatas.ContainsKey(level))
        {
            if (tierNum == 1) return dicCharacterCardLevelDatas[level].quantityTierCommon;
            else if (tierNum == 2) return dicCharacterCardLevelDatas[level].quantityTierUncommon;
            else if (tierNum == 3) return dicCharacterCardLevelDatas[level].quantityTierRare;
            else if (tierNum == 4) return dicCharacterCardLevelDatas[level].quantityTierUniqe;
            else if (tierNum == 5) return dicCharacterCardLevelDatas[level].quantityTierLegendary;
            else return 0;
        }
        else return 0;
    }
}
