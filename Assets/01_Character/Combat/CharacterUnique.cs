using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnique : CharacterCombat
{
    protected override void AttackPassiveSkill_1(string skillName, int index)
    {
        DebugLogger.Log($"{name}의 {index}번째 - {skillName} 스킬");
    }

    protected override void AttackPassiveSkill_2(string skillName, int index)
    {
        DebugLogger.Log($"{name}의 {index}번째 - {skillName} 스킬");
    }

    protected override void AttackPassiveSkill_3(string skillName, int index)
    {
        DebugLogger.Log($"{name}의 {index}번째 - {skillName} 스킬");
    }
}