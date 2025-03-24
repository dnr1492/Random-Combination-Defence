using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private UIPlay uiPlay;
    private SpriteRenderer spriteRenderer;
    private AnimatorController animationController;

    private Hp hp;
    private float curHp;
    private float maxHp;
    private float defense;
    private float speed;

    private readonly int defenseOffset = 20;  //방어력 20일 때 50% 감소, 40일 때 66.6% 감소 

    private Transform curWaypoint;
    private int wayIndex = 1;

    private bool isDie = false;

    public bool IsDie => isDie;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animationController = new AnimatorController(GetComponent<Animator>());

        hp = transform.Find("Hp").GetComponent<Hp>();
    }

    private void Start()
    {
        curWaypoint = Waypoint.waypoints[wayIndex];
    }

    private void Update()
    {
        MoveWaypoint();
    }

    public void Init(UIPlay uiPlay, float maxHp, float defense, float speed)
    {
        this.uiPlay = uiPlay;

        curHp = maxHp;
        this.maxHp = maxHp;
        this.defense = defense;
        this.speed = speed;
    }

    #region 사각 이동
    private void MoveWaypoint()
    {
        if (isDie) return;

        Vector2 dir = curWaypoint.position - transform.position;
        float distanceToWaypoint = dir.magnitude;
        float moveStep = speed * Time.deltaTime;

        if (distanceToWaypoint <= moveStep) {
            transform.position = curWaypoint.position;
            GetNextWaypoint();
        }
        else transform.Translate(moveStep * dir.normalized, Space.World);

        animationController.ChangeState(AnimatorState.Move);
    }

    private void GetNextWaypoint()
    {
        if (wayIndex >= Waypoint.waypoints.Length - 1) {
            wayIndex = 0;
            curWaypoint = Waypoint.waypoints[wayIndex];
            return;
        }

        if (wayIndex >= 2 && wayIndex < 3) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;

        wayIndex++;
        curWaypoint = Waypoint.waypoints[wayIndex];
    }
    #endregion

    #region 데미지를 입다
    public float TakeDamage(float damage, CharacterDamageType damageType)
    {
        curHp -= CalculateDamage(damage, damageType);

        hp.SetHp(curHp, maxHp);

        if (curHp <= 0) {
            StartCoroutine(Die());
            return 0;
        }

        return curHp;
    }

    private float CalculateDamage(float damage, CharacterDamageType damageType)
    {
        float damageReduction = 0;
        if (damageType == CharacterDamageType.Melee) damageReduction = defense / (defense + defenseOffset);
        else if (damageType == CharacterDamageType.Magic) damageReduction = 0;
        else if (damageType == CharacterDamageType.Blood) damageReduction = 0;
        float finalDamage = damage * (1 - damageReduction);
        DebugLogger.Log($"Damage {damage} - DamageReduction {damageReduction}: finalDamage {finalDamage}");
        return finalDamage;
    }
    #endregion

    private IEnumerator Die()
    {
        isDie = true;
        EnemyGenerator.ExistingEnemys.Remove(gameObject);
        GetComponent<BoxCollider2D>().enabled = false;

        uiPlay.SetUI_EnemyCount();

        animationController.ChangeState(AnimatorState.Death);
        float length = animationController.GetClipLength(AnimatorState.Death.ToString());
        yield return new WaitForSeconds(length);

        Destroy(gameObject);
    }
}