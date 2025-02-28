using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackEffectData", menuName = "Effect/AttackEffectData")]
public class AttackEffectData : ScriptableObject
{
    public string attackEffectName;
    public AttackEffectType attackEffectType;
    public GameObject attackEffectPrefab;
}