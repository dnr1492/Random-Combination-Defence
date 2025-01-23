using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo
{
    public int level;
    public string displayName;
    public float damage;
    public float attackSpeed = 0;
    public float attackRange = 0;
    public float moveSpeed = 0;
    public int tierNum = 0;
    public List<CharacterSkillData> skillDatas;  //��� ��ų
    public List<bool> unlockSkills;  //ī�� ������ ���� ���Ӱ� ���� ��ų

    public CharacterInfo(int level, string displayName, float damage, float attackSpeed, float attackRange, float moveSpeed, int tierNum, List<CharacterSkillData> skillDatas, List<bool> unlockSkills)
    {
        this.level = level;
        this.displayName = displayName;
        this.damage = damage;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.moveSpeed = moveSpeed;
        this.tierNum = tierNum;
        this.skillDatas = skillDatas;
        this.unlockSkills = unlockSkills;
    }
}
