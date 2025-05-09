using PlayFab.ProfilesModels;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class CharacterCombat : MonoBehaviour
{
    protected AnimatorController animationController;
    protected CharacterController characterController;
    protected CharacterInfo curCharacterInfo;
    protected List<CharacterSkillData> characterSkillDatas;

    private const string TAG_ENEMY = "Enemy";

    private EnemyController curTargetEnemyController;
    private CombatEffectType effectType = CombatEffectType.None;
    private bool canAttack = true;
    private float totalDamage = 0;
    private float totalAttackDelay = 0;

    private void OnDrawGizmosSelected()
    {
        //공격사거리 시각화 (디버그용)
        if (curCharacterInfo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, curCharacterInfo.attackRange);
        }
    }

    private void Awake()
    {
        animationController = new AnimatorController(GetComponentInChildren<Animator>());
        characterController = GetComponent<CharacterController>();
        curCharacterInfo = InfoManager.instance.LoadCharacterInfo(gameObject.name);
        characterSkillDatas = curCharacterInfo.skillDatas;
    }

    private void Update()
    {
        //공격이 불가능하다면 리턴
        if (!canAttack) return;

        //현재 적이 없거나, 공격사거리를 벗어났거나, 적이 죽었다면 적 갱신
        if (curTargetEnemyController == null 
            || !IsInRange(curTargetEnemyController.gameObject)
            || curTargetEnemyController.IsDie) 
        {
            curTargetEnemyController = FindClosestEnemy();
        }

        //타겟이 존재하고 이동 중이 아니면 공격
        if (curTargetEnemyController != null && !characterController.CheckMoving()) 
        {
            DebugLogger.Log($"'{name}'의 스킬 언락 수 : {curCharacterInfo.unlockSkills.Count}");

            totalDamage = curCharacterInfo.damage;
            totalAttackDelay = curCharacterInfo.attackSpeed;

            for (int i = curCharacterInfo.unlockSkills.Count - 1; i >= 0; i--)
            {
                if (characterSkillDatas.Count == 0) break;

                if (Random.Range(0, 100f) <= characterSkillDatas[i].skillTriggerChance) {
                    //스킬 우선순위
                    if (i == 2) AttackPassiveSkill_3(characterSkillDatas[i].skillName, i);
                    else if (i == 1) AttackPassiveSkill_2(characterSkillDatas[i].skillName, i);
                    else if (i == 0) AttackPassiveSkill_1(characterSkillDatas[i].skillName, i);
                    break;
                }
            }

            characterController.SetDirection(curTargetEnemyController.transform.position);
            if (effectType == CombatEffectType.None) SetEffectType(CombatEffectType.Hit_Attack_Normal);
            //if (Random.value > 0.8f) SetEffectType(CombatEffectType.Critical);
            StartCoroutine(Attack(totalDamage, totalAttackDelay));
        }
    }

    #region 공격
    /// <summary>
    /// 공격
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="attackSpeed"></param>
    /// <returns></returns>
    private IEnumerator Attack(float damage, float attackSpeed)
    {
        canAttack = false;

        var attackType = animationController.ConvertCharacterAttackTypeToAnimatorStateAttackType(curCharacterInfo.attackType);
        animationController.ChangeState(attackType);
        var length = animationController.GetClipLength(attackType.ToString()) / 2;
        yield return new WaitForSeconds(length);

        if (curTargetEnemyController != null) AttackEffect(curTargetEnemyController.transform.position, length);

        //적 데미지
        float curEnemyHp;
        if (curTargetEnemyController != null) {
            curEnemyHp = curTargetEnemyController.TakeDamage(damage, curCharacterInfo.damageType);
            if (curEnemyHp <= 0) curTargetEnemyController = null;
        }

        DebugLogger.Log("이름 : " + curCharacterInfo.displayName
            + "\n" + "데미지 : " + damage
            + "\n" + "공격속도 : " + attackSpeed);

        //공격 딜레이
        yield return new WaitForSeconds(attackSpeed - length);
        canAttack = true;
        SetEffectType(CombatEffectType.None);
    }

    /// <summary>
    /// 공격 효과
    /// </summary>
    /// <param name="effectPosition"></param>
    /// <param name="duration"></param>
    private void AttackEffect(Vector3 effectPosition, float duration)
    {
        //캐릭터별 이펙트 데이터 불러오기
        CharacterActionData effectData = ActionManager.Instance.GetCharacterActionData(curCharacterInfo.displayName);
        if (effectData == null) return;

        //이펙트 실행
        switch (effectType)
        {
            case CombatEffectType.Hit_Attack_Normal:
                ActionManager.Instance.PlayCombatAction(effectPosition, effectData.attackNormalEffect, duration);
                break;
            //case AttackEffectType.FireFlamme:
            //    ActionManager.Instance.PlayAttackAction(effectPosition, effectData.fireFlammeEffect, duration);
            //    break;
            //case AttackEffectType.Ice:
            //    ActionManager.Instance.PlayAttackAction(effectPosition, effectData.iceEffect, duration);
            //    break;
            //case AttackEffectType.WaterWave:
            //    ActionManager.Instance.PlayAttackAction(effectPosition, effectData.waterWaveEffect, duration);
            //    break;
            //case AttackEffectType.Electric:
            //    ActionManager.Instance.PlayAttackAction(effectPosition, effectData.electricEffect, duration);
            //    break;
            //case AttackEffectType.Explosion:
            //    ActionManager.Instance.PlayAttackAction(effectPosition, effectData.explosionEffect, duration);
            //    break;
            //case AttackEffectType.Critical:
            //    EffectManager.Instance.PlayEffect(effectPosition, effectData.criticalEffect, duration);
            //    break;
        }
    }
    #endregion

    #region 캐릭터의 공격 사거리 안에 있는지 확인
    /// <summary>
    /// 캐릭터의 공격 사거리 안에 있는지 확인
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool IsInRange(GameObject target)
    {
        if (target == null) return false;
        float distance = Vector2.Distance(transform.position, target.transform.position);
        return distance <= curCharacterInfo.attackRange;
    }
    #endregion

    #region 캐릭터와 가장 가까운 적을 찾기
    /// <summary>
    /// 캐릭터와 가장 가까운 적을 찾기
    /// </summary>
    /// <returns></returns>
    private EnemyController FindClosestEnemy()
    {
        //공격사거리 내의 모든 적 검색
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, curCharacterInfo.attackRange);
        EnemyController closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider2D enemyCollider in enemies)
        {
            if (!enemyCollider.CompareTag(TAG_ENEMY)) continue;

            float distance = Vector2.Distance(transform.position, enemyCollider.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestEnemy = enemyCollider.GetComponent<EnemyController>();
            }
        }

        return closestEnemy;
    }
    #endregion

    /// <summary>
    /// 데미지 추가
    /// </summary>
    /// <param name="damage"></param>
    protected void AddDamage(float damage)
    {
        totalDamage += damage;
    }

    /// <summary>
    /// 공격딜레이 추가
    /// </summary>
    /// <param name="skillIndex"></param>
    protected void AddAttackDelay(int skillIndex)
    {
        // ===== 지속데미지 등 효과가 다양한데 현재는 공격 딜레이만 추가하는 중이므로 추후에 수정 요망 ===== //
        // ===== 지속데미지 등 효과가 다양한데 현재는 공격 딜레이만 추가하는 중이므로 추후에 수정 요망 ===== //
        // ===== 지속데미지 등 효과가 다양한데 현재는 공격 딜레이만 추가하는 중이므로 추후에 수정 요망 ===== //
        MatchCollection matches = Regex.Matches(characterSkillDatas[skillIndex].skillEffect, @"\d+");
        foreach (Match match in matches) {
            totalAttackDelay += float.Parse(match.Value);
        }
    }

    /// <summary>
    /// 스킬데미지 공식을 계산
    /// </summary>
    /// <param name="strSkillDamage"></param>
    /// <param name="characterDamage"></param>
    /// <returns></returns>
    protected float CalculateSkillDamageFormula(string strSkillDamage, float characterDamage)
    {
        //Replace {0} with characterDamage
        string replacedFormula = strSkillDamage.Replace("{0}", characterDamage.ToString());
        //Compute the skillDamage
        DataTable table = new DataTable();
        object result = table.Compute(replacedFormula, "");
        //Convert result to float
        return System.Convert.ToSingle(result);
    }

    /// <summary>
    /// 이펙트 종류를 설정
    /// </summary>
    /// <param name="effectType"></param>
    protected void SetEffectType(CombatEffectType effectType)
    {
        this.effectType = effectType;
    }

    protected abstract void AttackPassiveSkill_1(string skillName, int index);

    protected abstract void AttackPassiveSkill_2(string skillName, int index);

    protected abstract void AttackPassiveSkill_3(string skillName, int index);
}