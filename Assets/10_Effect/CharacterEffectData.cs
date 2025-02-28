using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterEffectData", menuName = "Effect/CharacterEffectData")]
public class CharacterEffectData : ScriptableObject
{
    public string characterName;
    public AttackEffectData attackEffect;
    public AttackEffectData fireEffect;
    public AttackEffectData iceEffect;
    public AttackEffectData waterEffect;
    public AttackEffectData electricEffect;
    public AttackEffectData explosionEffect;
}