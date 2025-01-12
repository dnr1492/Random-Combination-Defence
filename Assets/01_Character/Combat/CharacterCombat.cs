using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class CharacterCombat : MonoBehaviour
{
    protected CharacterInfo curCharacterInfo;
    protected List<CharacterSkillData> characterSkillDatas;
    
    private const string TAG_ENEMY = "Enemy";

    private GameObject curTargetEnemy;
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
        curCharacterInfo = InfoManager.instance.LoadCharacterInfo(gameObject.name);
        characterSkillDatas = curCharacterInfo.skillDatas;
    }

    private void Update()
    {
        //현재 적이 없거나 공격사거리를 벗어난 경우 적 갱신
        if (curTargetEnemy == null || !IsInRange(curTargetEnemy)) {
            curTargetEnemy = FindClosestEnemy();
        }

        //타겟이 존재하고 공격이 가능한 상태라면 공격
        if (curTargetEnemy != null && canAttack) 
        {
            DebugLogger.Log($"'{name}'의 스킬 언락 수 : {curCharacterInfo.unlockSkills.Count}");

            totalDamage = curCharacterInfo.damage;
            totalAttackDelay = curCharacterInfo.attackSpeed;

            for (int i = curCharacterInfo.unlockSkills.Count - 1; i >= 0; i--)
            {
                if (Random.Range(0, 100f) <= characterSkillDatas[i].skill_triggerChance)
                {
                    //스킬 우선순위
                    if (i == 2) AttackPassiveSkill_3(i);
                    else if (i == 1) AttackPassiveSkill_2(i);
                    else if (i == 0) AttackPassiveSkill_1(i);
                    break;
                }
            }

            StartCoroutine(Attack(totalDamage, totalAttackDelay));
        }
    }

    #region 공격
    /// <summary>
    /// 공격
    /// </summary>
    /// <param name="damage"></param>
    private IEnumerator Attack(float damage, float attackSpeed)
    {
        canAttack = false;

        //적 데미지
        float curEnemyHp;
        if (curTargetEnemy != null) {
            curEnemyHp = curTargetEnemy.GetComponent<EnemyController>().TakeDamage(damage);
            if (curEnemyHp <= 0) curTargetEnemy = null;
        }

        DebugLogger.Log("이름 : " + curCharacterInfo.displayName
            + "\n" + "데미지 : " + damage
            + "\n" + "공격속도 : " + attackSpeed);

        //공격 딜레이
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
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
    private GameObject FindClosestEnemy()
    {
        //공격사거리 내의 모든 적 검색
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, curCharacterInfo.attackRange);
        GameObject closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider2D enemyCollider in enemies)
        {
            if (!enemyCollider.CompareTag(TAG_ENEMY)) continue;

            float distance = Vector2.Distance(transform.position, enemyCollider.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestEnemy = enemyCollider.gameObject;
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
        MatchCollection matches = Regex.Matches(characterSkillDatas[skillIndex].skill_effect, @"\d+");
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

    protected abstract void AttackPassiveSkill_1(int index);

    protected abstract void AttackPassiveSkill_2(int index);

    protected abstract void AttackPassiveSkill_3(int index);
}