using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;

    [SerializeField] List<CharacterActionData> characterActions;
    [SerializeField] List<PlayActionData> playActions;

    private Dictionary<string, CharacterActionData> characterActionDictionary = new Dictionary<string, CharacterActionData>();
    private Dictionary<string, PlayActionData> playActionDictionary = new Dictionary<string, PlayActionData>();

    private Dictionary<CombatEffectType, Queue<GameObject>> combatEffectPools = new Dictionary<CombatEffectType, Queue<GameObject>>();
    private Dictionary<PlayEffectType, Queue<GameObject>> playEffectPools = new Dictionary<PlayEffectType, Queue<GameObject>>();

    private AudioSource globalAudioSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (globalAudioSource == null) globalAudioSource = GetComponent<AudioSource>();

        foreach (var effect in characterActions) characterActionDictionary.Add(effect.name, effect);
        foreach (var effect in playActions) playActionDictionary.Add(effect.name, effect);
    }

    public CharacterActionData GetCharacterActionData(string characterName) => characterActionDictionary.ContainsKey(characterName) ? characterActionDictionary[characterName] : null;

    public PlayActionData GetPlayActionData(string playName) => playActionDictionary.ContainsKey(playName) ? playActionDictionary[playName] : null;

    #region Combat Action
    public void PlayCombatAction(Vector3 position, CombatActionData combatActionData, float duration, Transform parent = null)
    {
        if (combatActionData == null || combatActionData.combatEffectPrefab == null) return;

        //이펙트
        GameObject effect = GetCombatEffectFromPool(combatActionData);
        if (effect != null)
        {
            effect.transform.position = position;
            if (parent != null) effect.transform.SetParent(parent);
            effect.SetActive(true);

            //이펙트 지속 시간이 설정되어 있다면 자동 반환
            if (duration > 0) StartCoroutine(ReturnCombatEffectAfterTime(effect, combatActionData, duration));
        }

        //사운드
        if (combatActionData.combatSoundClip != null) globalAudioSource.PlayOneShot(combatActionData.combatSoundClip);
    }

    private GameObject GetCombatEffectFromPool(CombatActionData combatActionData)
    {
        if (!combatEffectPools.ContainsKey(combatActionData.combatEffectType)) combatEffectPools[combatActionData.combatEffectType] = new Queue<GameObject>();

        Queue<GameObject> pool = combatEffectPools[combatActionData.combatEffectType];
        if (pool.Count > 0) return pool.Dequeue();
        return InstantiateCombatEffect(combatActionData);
    }

    private GameObject InstantiateCombatEffect(CombatActionData combatActionData)
    {
        GameObject newEffect = Instantiate(combatActionData.combatEffectPrefab);
        newEffect.SetActive(false);
        return newEffect;
    }

    private IEnumerator ReturnCombatEffectAfterTime(GameObject effect, CombatActionData combatActionData, float duration)
    {
        yield return new WaitForSeconds(duration);
        ReturnCombatEffectToPool(effect, combatActionData);
    }

    public void ReturnCombatEffectToPool(GameObject effect, CombatActionData combatActionData)
    {
        effect.SetActive(false);
        effect.transform.SetParent(null);
        combatEffectPools[combatActionData.combatEffectType].Enqueue(effect);
    }
    #endregion

    #region Play Action
    public void PlayPlayAction(Vector3 position, PlayActionData playActionData, float duration, Transform parent = null)
    {
        if (playActionData == null || playActionData.playEffectPrefab == null) return;

        //이펙트
        GameObject effect = GetPlayEffectFromPool(playActionData);
        if (effect != null)
        {
            effect.transform.position = position;
            if (parent != null) effect.transform.SetParent(parent);
            effect.SetActive(true);

            //이펙트 지속 시간이 설정되어 있다면 자동 반환
            if (duration > 0) StartCoroutine(ReturnPlayEffectAfterTime(effect, playActionData, duration));
        }

        //사운드
        if (playActionData.playSoundClip != null) globalAudioSource.PlayOneShot(playActionData.playSoundClip);
    }

    private GameObject GetPlayEffectFromPool(PlayActionData playActionData)
    {
        if (!playEffectPools.ContainsKey(playActionData.playEffectType)) playEffectPools[playActionData.playEffectType] = new Queue<GameObject>();

        Queue<GameObject> pool = playEffectPools[playActionData.playEffectType];
        if (pool.Count > 0) return pool.Dequeue();
        return InstantiatePlayEffect(playActionData);
    }

    private GameObject InstantiatePlayEffect(PlayActionData playActionData)
    {
        GameObject newEffect = Instantiate(playActionData.playEffectPrefab);
        newEffect.SetActive(false);
        return newEffect;
    }

    private IEnumerator ReturnPlayEffectAfterTime(GameObject effect, PlayActionData playActionData, float duration)
    {
        yield return new WaitForSeconds(duration);
        ReturnPlayEffectToPool(effect, playActionData);
    }

    public void ReturnPlayEffectToPool(GameObject effect, PlayActionData playActionData)
    {
        effect.SetActive(false);
        effect.transform.SetParent(null);
        playEffectPools[playActionData.playEffectType].Enqueue(effect);
    }
    #endregion
}