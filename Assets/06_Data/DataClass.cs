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
    public CharacterAttackType attackType;
    public CharacterDamageType damageType;
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