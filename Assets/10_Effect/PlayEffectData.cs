using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayEffectData", menuName = "Effect/PlayEffectData")]
public class PlayEffectData : ScriptableObject
{
    public string playEffectName;
    public PlayEffectType playEffectType;
    public GameObject playEffectPrefab;
}
