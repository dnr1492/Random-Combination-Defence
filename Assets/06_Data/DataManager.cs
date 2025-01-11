using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
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
    private Dictionary<string, CharacterCardLevelInfoData> dicCharacterCardLevelInfoDatas;
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
        CharacterCardLevelData[] arrCardLevelData = JsonConvert.DeserializeObject<CharacterCardLevelData[]>(json);
        foreach (CharacterCardLevelData cardLevelData in arrCardLevelData)
        {
            if (!dicCharacterCardLevelDatas.ContainsKey(cardLevelData.level)) {
                dicCharacterCardLevelDatas.Add(cardLevelData.level, cardLevelData);
            }
        }
    }

    public void LoadCharacterCardLevelInfoData()
    {
        dicCharacterCardLevelInfoDatas = new Dictionary<string, CharacterCardLevelInfoData>();
        string json = Resources.Load<TextAsset>("Datas/characterCardLevelInfo_data").text;
        CharacterCardLevelInfoData[] arrCardLevelInfoData = JsonConvert.DeserializeObject<CharacterCardLevelInfoData[]>(json);
        foreach (CharacterCardLevelInfoData cardLevelInfoData in arrCardLevelInfoData)
        {
            if (!dicCharacterCardLevelInfoDatas.ContainsKey(cardLevelInfoData.display_name)) {
                dicCharacterCardLevelInfoDatas.Add(cardLevelInfoData.display_name, cardLevelInfoData);
            }
        }
    }

    public void LoadCharacterData()
    {
        dicCharacterDatas = new Dictionary<string, CharacterData>();
        string json = Resources.Load<TextAsset>("Datas/character_data").text;
        CharacterData[] arrCharactersData = JsonConvert.DeserializeObject<CharacterData[]>(json);
        foreach (CharacterData charactersData in arrCharactersData)
        {
            if (!dicCharacterDatas.ContainsKey(charactersData.display_name)) {
                dicCharacterDatas.Add(charactersData.display_name, charactersData);
            }
        }
    }

    public void LoadCharacterSkillData()
    {
        dicCharacterSkillDatas = new Dictionary<string, CharacterSkillData>();
        string json = Resources.Load<TextAsset>("Datas/characterSkill_data").text;
        CharacterSkillData[] arrCharacterSkillData = JsonConvert.DeserializeObject<CharacterSkillData[]>(json);
        foreach (CharacterSkillData characterSkillData in arrCharacterSkillData)
        {
            if (!dicCharacterSkillDatas.ContainsKey(characterSkillData.skill_name)) {
                dicCharacterSkillDatas.Add(characterSkillData.skill_name, characterSkillData);
            }
        }
    }

    public void LoadCharacterRecipeData()
    {
        dicCharacterRecipeDatas = new Dictionary<string, List<CharacterRecipeData>>();
        string json = Resources.Load<TextAsset>("Datas/characterRecipe_data").text;
        CharacterRecipeData[] arrCharacterRecipeData = JsonConvert.DeserializeObject<CharacterRecipeData[]>(json);
        foreach (CharacterRecipeData characterRecipeData in arrCharacterRecipeData)
        {
            ////성능 개선 전
            //if (dicCharacterRecipeDatas.ContainsKey(characterRecipeData.select_name)) {
            //    dicCharacterRecipeDatas[characterRecipeData.select_name].Add(characterRecipeData);
            //}
            //else dicCharacterRecipeDatas[characterRecipeData.select_name] = new List<CharacterRecipeData> { characterRecipeData };

            //성능 개선 후
            if (!dicCharacterRecipeDatas.TryGetValue(characterRecipeData.select_name, out var recipeList)) {
                recipeList = new List<CharacterRecipeData>();
                dicCharacterRecipeDatas[characterRecipeData.select_name] = recipeList;
            }
            recipeList.Add(characterRecipeData);
        }
    }

    public void LoadPlayWaveData()
    {
        dicPlayWaveDatas = new Dictionary<int, PlayWaveData>();
        string json = Resources.Load<TextAsset>("Datas/playWave_data").text;
        PlayWaveData[] arrPlayWaveData = JsonConvert.DeserializeObject<PlayWaveData[]>(json);
        foreach (PlayWaveData playWaveData in arrPlayWaveData)
        {
            if (!dicPlayWaveDatas.ContainsKey(playWaveData.wave_id)) {
                dicPlayWaveDatas.Add(playWaveData.wave_id, playWaveData);
            }
        }
    }

    public void LoadPlayMapData()
    {
        dicPlayMapDatas = new Dictionary<int, PlayMapData>();
        string json = Resources.Load<TextAsset>("Datas/playMap_data").text;
        PlayMapData[] arrPlayMapData = JsonConvert.DeserializeObject<PlayMapData[]>(json);
        foreach (PlayMapData playMapData in arrPlayMapData)
        {
            if (!dicPlayMapDatas.ContainsKey(playMapData.map_id)) {
                dicPlayMapDatas.Add(playMapData.map_id, playMapData);
            }
        }
    }

    public void LoadPlayEnemyData()
    {
        dicPlayEnemyDatas = new Dictionary<string, PlayEnemyData>();
        string json = Resources.Load<TextAsset>("Datas/playEnemy_data").text;
        PlayEnemyData[] arrPlayEnemyData = JsonConvert.DeserializeObject<PlayEnemyData[]>(json);
        foreach (PlayEnemyData playEnemyData in arrPlayEnemyData)
        {
            if (!dicPlayEnemyDatas.ContainsKey(playEnemyData.enemy_name)) {
                dicPlayEnemyDatas.Add(playEnemyData.enemy_name, playEnemyData);
            }
        }
    }

    public Dictionary<string, CharacterCardLevelInfoData> GetCharacterCardLevelInfoData() => dicCharacterCardLevelInfoDatas;

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
            if (tierNum == 1) return dicCharacterCardLevelDatas[level].quentity_tier_1;
            else if (tierNum == 2) return dicCharacterCardLevelDatas[level].quentity_tier_2;
            else if (tierNum == 3) return dicCharacterCardLevelDatas[level].quentity_tier_3;
            else if (tierNum == 4) return dicCharacterCardLevelDatas[level].quentity_tier_4;
            else if (tierNum == 5) return dicCharacterCardLevelDatas[level].quentity_tier_5;
            else return 0;
        }
        else return 0;
    }
}
