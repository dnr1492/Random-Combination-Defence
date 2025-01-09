using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRare : PlayerCombat
{
    protected override void AttackPassiveSkill_1(int index)
    {
        DebugLogger.Log($"{name}의 {index}번째 스킬");
    }

    protected override void AttackPassiveSkill_2(int index)
    {
        DebugLogger.Log($"{name}의 {index}번째 스킬");
    }

    protected override void AttackPassiveSkill_3(int index)
    {
        DebugLogger.Log($"{name}의 {index}번째 스킬");
    }
}