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
    public int level;          //����
    public string description; //����
    public int damage;         //���ݷ� (���� ������ 0)
    public float attackSpeed;  //���ݼӵ� (���� ������ 0)
    public float moveSpeed;    //�̵��ӵ� (���� ������ 0)
    public string skill;       //Ư�� �������� �߰��Ǵ� ��ų
}