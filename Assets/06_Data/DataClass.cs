using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 각 캐릭터 카드 레벨 정보 데이터
[Serializable]
public class CharacterCardDataList
{
    public List<CharacterCardData> characterCardDatas;
}

[Serializable]
public class CharacterCardData
{
    public string display_name;
    public List<CharacterCardLevelInfoData> levels;
}

[Serializable]
public class CharacterCardLevelInfoData
{
    public int level;          //레벨
    public string description; //설명
    public int damage;         //공격력 (증가 없으면 0)
    public float attackSpeed;  //공격속도 (증가 없으면 0)
    public float moveSpeed;    //이동속도 (증가 없으면 0)
    public string skill;       //특정 레벨에서 추가되는 스킬
}
#endregion

#region 각 캐릭터 카드 레벨업에 필요한 수량 데이터
[Serializable]
public class CharacterCardLevelDataList
{
    public List<CharacterCardLevelData> characterCardLevelDatas;
}

[Serializable]
public class CharacterCardLevelData
{
    public int level;
    public int quantityTierCommon;
    public int quantityTierUncommon;
    public int quantityTierRare;
    public int quantityTierUniqe;
    public int quantityTierLegendary;
}
#endregion

#region 각 캐릭터 데이터
[Serializable]
public class CharacterDataList
{
    public List<CharacterData> characterDatas;
}

[Serializable]
public class CharacterData
{
    public string displayName;
    public float damage;
    public float attackSpeed;
    public float attackRange;
    public float moveSpeed;
    public int tierNum;
    public CharacterAttackType attackType;
    public CharacterDamageType damageType;
    public string skill_1_name;
    public string skill_2_name;
    public string skill_3_name;
}
#endregion

#region 각 캐릭터의 조합법 데이터
[Serializable]
public class CharacterRecipeDataList
{
    public List<CharacterRecipeData> characterRecipeDatas;
}

[Serializable]
public class CharacterRecipeData
{
    public string selectName;
    public string recipeNameA;
    public string recipeNameB;
    public string recipeNameC;
    public string resultName;
}
#endregion

#region 각 캐릭터의 스킬 데이터
[Serializable]
public class CharacterSkillDataList
{
    public List<CharacterSkillData> characterSkillDatas;
}

[Serializable]
public class CharacterSkillData
{
    public string skillName;
    public string characterDisplayName;
    public string skillDescription;
    public string skillImagePath;
    public string skillDamage;
    public float skillTriggerChance;
    public string skillEffect;
    public bool skillBasic;
}
#endregion

#region 게임 플레이 웨이브 데이터
[Serializable]
public class PlayWaveDataList
{
    public List<PlayWaveData> playWaveDatas;
}

[Serializable]
public class PlayWaveData
{
    public int wave;
    public int waveEnemyCount;
    public float waveTimer;
    public float enemyHp;
    public float enemyDefense;
    public float enemySpeed;
    public EnemyType enemyType;
    public int characterDrawCount;
}
#endregion