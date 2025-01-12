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
        //���ݻ�Ÿ� �ð�ȭ (����׿�)
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
        //���� ���� ���ų� ���ݻ�Ÿ��� ��� ��� �� ����
        if (curTargetEnemy == null || !IsInRange(curTargetEnemy)) {
            curTargetEnemy = FindClosestEnemy();
        }

        //Ÿ���� �����ϰ� ������ ������ ���¶�� ����
        if (curTargetEnemy != null && canAttack) 
        {
            DebugLogger.Log($"'{name}'�� ��ų ��� �� : {curCharacterInfo.unlockSkills.Count}");

            totalDamage = curCharacterInfo.damage;
            totalAttackDelay = curCharacterInfo.attackSpeed;

            for (int i = curCharacterInfo.unlockSkills.Count - 1; i >= 0; i--)
            {
                if (Random.Range(0, 100f) <= characterSkillDatas[i].skill_triggerChance)
                {
                    //��ų �켱����
                    if (i == 2) AttackPassiveSkill_3(i);
                    else if (i == 1) AttackPassiveSkill_2(i);
                    else if (i == 0) AttackPassiveSkill_1(i);
                    break;
                }
            }

            StartCoroutine(Attack(totalDamage, totalAttackDelay));
        }
    }

    #region ����
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="damage"></param>
    private IEnumerator Attack(float damage, float attackSpeed)
    {
        canAttack = false;

        //�� ������
        float curEnemyHp;
        if (curTargetEnemy != null) {
            curEnemyHp = curTargetEnemy.GetComponent<EnemyController>().TakeDamage(damage);
            if (curEnemyHp <= 0) curTargetEnemy = null;
        }

        DebugLogger.Log("�̸� : " + curCharacterInfo.displayName
            + "\n" + "������ : " + damage
            + "\n" + "���ݼӵ� : " + attackSpeed);

        //���� ������
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }
    #endregion

    #region ĳ������ ���� ��Ÿ� �ȿ� �ִ��� Ȯ��
    /// <summary>
    /// ĳ������ ���� ��Ÿ� �ȿ� �ִ��� Ȯ��
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

    #region ĳ���Ϳ� ���� ����� ���� ã��
    /// <summary>
    /// ĳ���Ϳ� ���� ����� ���� ã��
    /// </summary>
    /// <returns></returns>
    private GameObject FindClosestEnemy()
    {
        //���ݻ�Ÿ� ���� ��� �� �˻�
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
    /// ������ �߰�
    /// </summary>
    /// <param name="damage"></param>
    protected void AddDamage(float damage)
    {
        totalDamage += damage;
    }

    /// <summary>
    /// ���ݵ����� �߰�
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
    /// ��ų������ ������ ���
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