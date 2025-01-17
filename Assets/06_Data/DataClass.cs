using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region �� ĳ���� ī�� ���� ���� ������
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
    public int level;          //����
    public string description; //����
    public int damage;         //���ݷ� (���� ������ 0)
    public float attackSpeed;  //���ݼӵ� (���� ������ 0)
    public float moveSpeed;    //�̵��ӵ� (���� ������ 0)
    public string skill;       //Ư�� �������� �߰��Ǵ� ��ų
}
#endregion

#region �� ĳ���� ī�� �������� �ʿ��� ���� ������
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

#region �� ĳ���� ������
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
    public string classType;
    public string skill_1_name;
    public string skill_2_name;
    public string skill_3_name;
}
#endregion

#region �� ĳ������ ���չ� ������
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

#region �� ĳ������ ��ų ������
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

#region ���� �÷��� ���̺� ������
[Serializable]
public class PlayWaveDataList
{
    public List<PlayWaveData> playWaveDatas;
}

[Serializable]
public class PlayWaveData
{
    public int waveId;
    public string waveEnemyName;
    public int waveEnemyCount;
    public float waveTimer;
}
#endregion

#region ���� �÷��� �� ������
[Serializable]
public class PlayMapDataList
{
    public List<PlayMapData> playMapDatas;
}

[Serializable]
public class PlayMapData
{
    public int mapId;
    public int maximumPopulation;
    public int maximumEnemyCount;
}
#endregion

#region ���� �÷��� �� ���� ������
[Serializable]
public class PlayEnemyDataList
{
    public List<PlayEnemyData> playEnemyDatas;
}

[Serializable]
public class PlayEnemyData
{
    public string enemyName;
    public float enemySpeed;
    public float enemyHp;
    public int dropGold;
    public int dropDarkGold;
}
#endregion