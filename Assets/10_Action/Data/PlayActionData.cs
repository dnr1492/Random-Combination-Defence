using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayActionData", menuName = "Effect/PlayActionData")]
public class PlayActionData : ScriptableObject
{
    public PlayEffectType playEffectType;
    public GameObject playEffectPrefab;
    public AudioClip playSoundClip;
}
