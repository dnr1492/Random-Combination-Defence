using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    private Dictionary<AttackEffectType, Queue<GameObject>> attackEffectPools = new Dictionary<AttackEffectType, Queue<GameObject>>();
    private Dictionary<PlayEffectType, Queue<GameObject>> playEffectPools = new Dictionary<PlayEffectType, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    #region Attack Effect
    public void PlayAttackEffect(Vector3 position, AttackEffectData attackEffectData, float duration, Transform parent = null)
    {
        if (attackEffectData == null || attackEffectData.attackEffectPrefab == null) return;

        GameObject effect = GetAttackEffectFromPool(attackEffectData);
        if (effect != null)
        {
            effect.transform.position = position;
            if (parent != null) effect.transform.SetParent(parent);
            effect.SetActive(true);

            //이펙트 지속 시간이 설정되어 있다면 자동 반환
            if (duration > 0) StartCoroutine(ReturnAttackEffectAfterTime(effect, attackEffectData, duration));
        }
    }

    private GameObject GetAttackEffectFromPool(AttackEffectData attackEffectData)
    {
        if (!attackEffectPools.ContainsKey(attackEffectData.attackEffectType)) attackEffectPools[attackEffectData.attackEffectType] = new Queue<GameObject>();

        Queue<GameObject> pool = attackEffectPools[attackEffectData.attackEffectType];
        if (pool.Count > 0) return pool.Dequeue();
        return InstantiateAttackEffect(attackEffectData);
    }

    private GameObject InstantiateAttackEffect(AttackEffectData attackEffectData)
    {
        GameObject newEffect = Instantiate(attackEffectData.attackEffectPrefab);
        newEffect.SetActive(false);
        return newEffect;
    }

    private IEnumerator ReturnAttackEffectAfterTime(GameObject effect, AttackEffectData attackEffectData, float duration)
    {
        yield return new WaitForSeconds(duration);
        ReturnAttackEffectToPool(effect, attackEffectData);
    }

    public void ReturnAttackEffectToPool(GameObject effect, AttackEffectData attackEffectData)
    {
        effect.SetActive(false);
        effect.transform.SetParent(null);
        attackEffectPools[attackEffectData.attackEffectType].Enqueue(effect);
    }
    #endregion

    #region Play Effect
    public void PlayPlayEffect(Vector3 position, PlayEffectData playEffectData, float duration, Transform parent = null)
    {
        if (playEffectData == null || playEffectData.playEffectPrefab == null) return;

        GameObject effect = GetPlayEffectFromPool(playEffectData);
        if (effect != null)
        {
            effect.transform.position = position;
            if (parent != null) effect.transform.SetParent(parent);
            effect.SetActive(true);

            //이펙트 지속 시간이 설정되어 있다면 자동 반환
            if (duration > 0) StartCoroutine(ReturnPlayEffectAfterTime(effect, playEffectData, duration));
        }
    }

    private GameObject GetPlayEffectFromPool(PlayEffectData playEffectData)
    {
        if (!playEffectPools.ContainsKey(playEffectData.playEffectType)) playEffectPools[playEffectData.playEffectType] = new Queue<GameObject>();

        Queue<GameObject> pool = playEffectPools[playEffectData.playEffectType];
        if (pool.Count > 0) return pool.Dequeue();
        return InstantiatePlayEffect(playEffectData);
    }

    private GameObject InstantiatePlayEffect(PlayEffectData playEffectData)
    {
        GameObject newEffect = Instantiate(playEffectData.playEffectPrefab);
        newEffect.SetActive(false);
        return newEffect;
    }

    private IEnumerator ReturnPlayEffectAfterTime(GameObject effect, PlayEffectData playEffectData, float duration)
    {
        yield return new WaitForSeconds(duration);
        ReturnPlayEffectToPool(effect, playEffectData);
    }

    public void ReturnPlayEffectToPool(GameObject effect, PlayEffectData playEffectData)
    {
        effect.SetActive(false);
        effect.transform.SetParent(null);
        playEffectPools[playEffectData.playEffectType].Enqueue(effect);
    }
    #endregion
}