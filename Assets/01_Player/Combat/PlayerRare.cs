using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRare : PlayerCombat
{
    protected override void AttackPassiveSkill_1(int index)
    {
        DebugLogger.Log($"{name}�� {index}��° ��ų");
    }

    protected override void AttackPassiveSkill_2(int index)
    {
        DebugLogger.Log($"{name}�� {index}��° ��ų");
    }

    protected override void AttackPassiveSkill_3(int index)
    {
        DebugLogger.Log($"{name}�� {index}��° ��ų");
    }
}