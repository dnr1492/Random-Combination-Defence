using UnityEngine;

public class PlayerCommon : PlayerCombat
{
    protected override void AttackPassiveSkill_1(int index)
    {
        AddDamage(
            CalculateSkillDamageFormula(characterSkillDatas[index].skill_damage, curCharacterInfo.damage));
 
        AddAttackDelay(index);

        Debug.Log($"{name}�� {index}��° ��ų");
    }

    protected override void AttackPassiveSkill_2(int index)
    {
        AddDamage(
            CalculateSkillDamageFormula(characterSkillDatas[index].skill_damage, curCharacterInfo.damage));

        AddAttackDelay(index);

        Debug.Log($"{name}�� {index}��° ��ų");
    }

    protected override void AttackPassiveSkill_3(int index)
    {
        AddDamage(
            CalculateSkillDamageFormula(characterSkillDatas[index].skill_damage, curCharacterInfo.damage));

        AddAttackDelay(index);

        Debug.Log($"{name}�� {index}��° ��ų");
    }
}
