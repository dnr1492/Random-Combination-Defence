using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectManager : MonoBehaviour
{
    public static CharacterEffectManager Instance;

    [SerializeField] List<CharacterEffectData> characterEffects;
    [SerializeField] List<PlayEffectData> playEffects;

    private Dictionary<string, CharacterEffectData> characterEffectDictionary = new Dictionary<string, CharacterEffectData>();
    private Dictionary<string, PlayEffectData> playEffectDictionary = new Dictionary<string, PlayEffectData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;

        foreach (var effect in characterEffects) {
            characterEffectDictionary.Add(effect.characterName, effect);
        }

        foreach (var effect in playEffects) {
            playEffectDictionary.Add(effect.playEffectName, effect);
        }
    }

    public CharacterEffectData GetAttackEffectData(string characterName)
    {
        return characterEffectDictionary.ContainsKey(characterName) ? characterEffectDictionary[characterName] : null;
    }

    public PlayEffectData GetPlayEffectData(string playName)
    {
        return playEffectDictionary.ContainsKey(playName) ? playEffectDictionary[playName] : null;
    }
}