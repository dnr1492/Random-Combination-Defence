using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager
{
    public static DataManager instance = null;

    private DataManager() { }

    public static DataManager GetInstance()
    {
        if (instance == null) instance = new DataManager();
        return instance;
    }

    private readonly Dictionary<int, CharacterCardLevelData> dicCharacterCardLevelDatas = new Dictionary<int, CharacterCardLevelData>();
    private readonly Dictionary<string, CharacterCardLevelInfoData> dicCharacterCardLevelInfoDatas = new Dictionary<string, CharacterCardLevelInfoData>();
    private readonly Dictionary<string, CharacterData> dicCharacterDatas = new Dictionary<string, CharacterData>();
    private readonly Dictionary<string, CharacterSkillData> dicCharacterSkillDatas = new Dictionary<string, CharacterSkillData>();
    private readonly Dictionary<string, List<CharacterRecipeData>> dicCharacterRecipeDatas = new Dictionary<string, List<CharacterRecipeData>>();
    private readonly Dictionary<int, PlayWaveData> dicPlayWaveDatas = new Dictionary<int, PlayWaveData>();
    private readonly Dictionary<int, PlayMapData> dicPlayMapDatas = new Dictionary<int, PlayMapData>();
    private readonly Dictionary<string, PlayEnemyData> dicPlayEnemyDatas = new Dictionary<string, PlayEnemyData>();

    public void LoadCharacterCardLevelData()
    {
        string json = Resources.Load<TextAsset>("Datas/characterCardLevel_data").text;
        CharacterCardLevelData[] arrCardLevelData = JsonConvert.DeserializeObject<CharacterCardLevelData[]>(json);
        foreach (CharacterCardLevelData cardLevelData in arrCardLevelData)
        {
            dicCharacterCardLevelDatas.Add(cardLevelData.level, cardLevelData);
        }
    }

    public void LoadCharacterCardLevelInfoData()
    {
        string json = Resources.Load<TextAsset>("Datas/characterCardLevelInfo_data").text;
        CharacterCardLevelInfoData[] arrCardLevelInfoData = JsonConvert.DeserializeObject<CharacterCardLevelInfoData[]>(json);
        foreach (CharacterCardLevelInfoData cardLevelInfoData in arrCardLevelInfoData)
        {
            dicCharacterCardLevelInfoDatas.Add(cardLevelInfoData.name, cardLevelInfoData);
        }
    }

    public void LoadCharacterData()
    {
        string json = Resources.Load<TextAsset>("Datas/character_data").text;
        CharacterData[] arrCharactersData = JsonConvert.DeserializeObject<CharacterData[]>(json);
        foreach (CharacterData charactersData in arrCharactersData)
        {
            dicCharacterDatas.Add(charactersData.name, charactersData);
        }
    }

    public void LoadCharacterSkillData()
    {
        string json = Resources.Load<TextAsset>("Datas/characterSkill_data").text;
        CharacterSkillData[] arrCharacterSkillData = JsonConvert.DeserializeObject<CharacterSkillData[]>(json);
        foreach (CharacterSkillData characterSkillData in arrCharacterSkillData)
        {
            dicCharacterSkillDatas.Add(characterSkillData.skill_name, characterSkillData);
        }
    }

    public void LoadCharacterRecipeData()
    {
        string json = Resources.Load<TextAsset>("Datas/characterRecipe_data").text;
        CharacterRecipeData[] arrCharacterRecipeData = JsonConvert.DeserializeObject<CharacterRecipeData[]>(json);
        foreach (CharacterRecipeData characterRecipeData in arrCharacterRecipeData)
        {
            ////성능 개선 전
            //if (dicCharacterRecipeData.ContainsKey(characterRecipeData.select_name)) {
            //    dicCharacterRecipeData[characterRecipeData.select_name].Add(characterRecipeData);
            //}
            //else dicCharacterRecipeData[characterRecipeData.select_name] = new List<CharacterRecipeData> { characterRecipeData };

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
        string json = Resources.Load<TextAsset>("Datas/playWave_data").text;
        PlayWaveData[] arrPlayWaveData = JsonConvert.DeserializeObject<PlayWaveData[]>(json);
        foreach (PlayWaveData playWaveData in arrPlayWaveData)
        {
            dicPlayWaveDatas.Add(playWaveData.wave_id, playWaveData);
        }
    }

    public void LoadPlayMapData()
    {
        string json = Resources.Load<TextAsset>("Datas/playMap_data").text;
        PlayMapData[] arrPlayMapData = JsonConvert.DeserializeObject<PlayMapData[]>(json);
        foreach (PlayMapData playMapData in arrPlayMapData)
        {
            dicPlayMapDatas.Add(playMapData.map_id, playMapData);
        }
    }

    public void LoadPlayEnemyData()
    {
        string json = Resources.Load<TextAsset>("Datas/playEnemy_data").text;
        PlayEnemyData[] arrPlayEnemyData = JsonConvert.DeserializeObject<PlayEnemyData[]>(json);
        foreach (PlayEnemyData playEnemyData in arrPlayEnemyData)
        {
            dicPlayEnemyDatas.Add(playEnemyData.enemy_name, playEnemyData);
        }
    }

    public Dictionary<string, CharacterCardLevelInfoData> GetCharacterCardLevelInfoData()
    {
        return dicCharacterCardLevelInfoDatas;
    }

    public Dictionary<string, CharacterData> GetCharacterData()
    {
        return dicCharacterDatas;
    }

    public Dictionary<string, CharacterSkillData> GetCharacterSkillData()
    {
        return dicCharacterSkillDatas;
    }

    public Dictionary<string, List<CharacterRecipeData>> GetCharacterRecipeData()
    {
        return dicCharacterRecipeDatas;
    }

    public Dictionary<int, PlayWaveData> GetPlayWaveData()
    {
        return dicPlayWaveDatas;
    }

    public Dictionary<int, PlayMapData> GetPlayMapData()
    {
        return dicPlayMapDatas;
    }

    public Dictionary<string, PlayEnemyData> GetPlayEnemyData()
    {
        return dicPlayEnemyDatas;
    }

    public int GetCharacterCardLevelQuentityData(int level, int tier)
    {
        if (dicCharacterCardLevelDatas.ContainsKey(level))
        {
            if (tier == 1) return dicCharacterCardLevelDatas[level].quentity_tier_1;
            else if (tier == 2) return dicCharacterCardLevelDatas[level].quentity_tier_2;
            else if (tier == 3) return dicCharacterCardLevelDatas[level].quentity_tier_3;
            else if (tier == 4) return dicCharacterCardLevelDatas[level].quentity_tier_4;
            else if (tier == 5) return dicCharacterCardLevelDatas[level].quentity_tier_5;
            else return 0;
        }
        else return 0;
    }
}
