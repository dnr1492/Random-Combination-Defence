using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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