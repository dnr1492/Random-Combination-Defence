using UnityEngine;

public class CharacterCommon : CharacterCombat
{
    protected override void AttackPassiveSkill_1(string skillName, int index)
    {
        //SetEffectType(EffectType.Water);
        AddDamage(CalculateSkillDamageFormula(characterSkillDatas[index].skillDamage, curCharacterInfo.damage));
        AddAttackDelay(index);
        DebugLogger.Log($"{name}�� {index}��° - {skillName} ��ų");
    }

    protected override void AttackPassiveSkill_2(string skillName, int index)
    {
        //SetEffectType(EffectType.Water);
        AddDamage(CalculateSkillDamageFormula(characterSkillDatas[index].skillDamage, curCharacterInfo.damage));
        AddAttackDelay(index);
        DebugLogger.Log($"{name}�� {index}��° - {skillName} ��ų");
    }

    protected override void AttackPassiveSkill_3(string skillName, int index)
    {
        //SetEffectType(EffectType.Water);
        AddDamage(CalculateSkillDamageFormula(characterSkillDatas[index].skillDamage, curCharacterInfo.damage));
        AddAttackDelay(index);
        DebugLogger.Log($"{name}�� {index}��° - {skillName} ��ų");
    }
}
