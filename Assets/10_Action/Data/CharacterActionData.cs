using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterActionData", menuName = "Effect/CharacterActionData")]
public class CharacterActionData : ScriptableObject
{
    public CombatActionData attackNormalEffect;
    public CombatActionData attackBowEffect;
    public CombatActionData attackMagicEffect;
    public CombatActionData attackLongSpearEffect;
    public CombatActionData attackAxeEffect;
    public CombatActionData attackShotSwordEffect;
    public CombatActionData concentrateEffect;
    public CombatActionData buffEffect;
    public CombatActionData debuffEffect;
    public CombatActionData skillNormalEffect;
    public CombatActionData skillBowEffect;
    public CombatActionData skillMagicEffect;
}