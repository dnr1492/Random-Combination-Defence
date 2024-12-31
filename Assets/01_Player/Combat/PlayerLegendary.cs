using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegendary : PlayerCombat
{
    protected override void AttackPassiveSkill_1(int index)
    {
        Debug.Log($"{name}의 {index}번째 스킬");
    }

    protected override void AttackPassiveSkill_2(int index)
    {
        Debug.Log($"{name}의 {index}번째 스킬");
    }

    protected override void AttackPassiveSkill_3(int index)
    {
        Debug.Log($"{name}의 {index}번째 스킬");
    }
}