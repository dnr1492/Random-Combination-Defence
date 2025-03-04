using UnityEngine;

[CreateAssetMenu(fileName = "NewCombatActionData", menuName = "Effect/CombatActionData")]
public class CombatActionData : ScriptableObject
{
    public CombatEffectType combatEffectType;
    public GameObject combatEffectPrefab;
    public AudioClip combatSoundClip;
}