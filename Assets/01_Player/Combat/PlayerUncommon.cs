using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUncommon : PlayerCombat
{
    protected override void AttackPassiveSkill_1(int index)
    {
        Debug.Log($"{name}�� {index}��° ��ų");
    }

    protected override void AttackPassiveSkill_2(int index)
    {
        Debug.Log($"{name}�� {index}��° ��ų");
    }

    protected override void AttackPassiveSkill_3(int index)
    {
        Debug.Log($"{name}�� {index}��° ��ų");
    }
}