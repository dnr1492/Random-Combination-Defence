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
        animationController = new AnimatorController(GetComponentInChildren<Animator>());
        characterController = GetComponent<CharacterController>();
        curCharacterInfo = InfoManager.instance.LoadCharacterInfo(gameObject.name);
        characterSkillDatas = curCharacterInfo.skillDatas;
    }

    private void Update()
    {
        //���� ���� ���ų� ���ݻ�Ÿ��� ����ų� ���� �׾��ٸ� �� ����
        if (curTargetEnemyController == null 
            || !IsInRange(curTargetEnemyController.gameObject)
            || curTargetEnemyController.IsDie) 
        {
            curTargetEnemyController = FindClosestEnemy();
        }

        //Ÿ���� �����ϰ� �̵� ���� �ƴϸ� ������ ������ ���¶�� ����
        if (curTargetEnemyController != null && canAttack && !characterController.CheckMoving()) 
        {
            DebugLogger.Log($"'{name}'�� ��ų ��� �� : {curCharacterInfo.unlockSkills.Count}");

            totalDamage = curCharacterInfo.damage;
            totalAttackDelay = curCharacterInfo.attackSpeed;

            for (int i = curCharacterInfo.unlockSkills.Count - 1; i >= 0; i--)
            {
                if (characterSkillDatas.Count == 0) break;

                if (Random.Range(0, 100f) <= characterSkillDatas[i].skillTriggerChance)
                {
                    //��ų �켱����
                    if (i == 2) AttackPassiveSkill_3(characterSkillDatas[i].skillName, i);
                    else if (i == 1) AttackPassiveSkill_2(characterSkillDatas[i].skillName, i);
                    else if (i == 0) AttackPassiveSkill_1(characterSkillDatas[i].skillName, i);
                    break;
                }
            }

            characterController.SetDirection(curTargetEnemyController.transform.position);
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

        animationController.ChangeState(AnimatorState.Attack_Normal, CharacterAttackType.Normal);

        //�� ������
        float curEnemyHp;
        if (curTargetEnemyController != null) {
            curEnemyHp = curTargetEnemyController.TakeDamage(damage);
            if (curEnemyHp <= 0) curTargetEnemyController = null;
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
    private EnemyController FindClosestEnemy()
    {
        //���ݻ�Ÿ� ���� ��� �� �˻�
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
        // ===== ���ӵ����� �� ȿ���� �پ��ѵ� ����� ���� �����̸� �߰��ϴ� ���̹Ƿ� ���Ŀ� ���� ��� ===== //
        // ===== ���ӵ����� �� ȿ���� �پ��ѵ� ����� ���� �����̸� �߰��ϴ� ���̹Ƿ� ���Ŀ� ���� ��� ===== //
        // ===== ���ӵ����� �� ȿ���� �پ��ѵ� ����� ���� �����̸� �߰��ϴ� ���̹Ƿ� ���Ŀ� ���� ��� ===== //
        MatchCollection matches = Regex.Matches(characterSkillDatas[skillIndex].skillEffect, @"\d+");
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

    protected abstract void AttackPassiveSkill_1(string skillName, int index);

    protected abstract void AttackPassiveSkill_2(string skillName, int index);

    protected abstract void AttackPassiveSkill_3(string skillName, int index);
}