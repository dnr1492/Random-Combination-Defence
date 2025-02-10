using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private UIPlay uiPlay;
    private SpriteRenderer spriteRenderer;
    private AnimatorController animationController;

    private Hp hp;
    private float curHp;
    private float maxHp;
    private float speed;
    private int dropGold;
    private int dropDarkGold;

    private Transform curWaypoint;
    private int wayIndex = 1;

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

    public void Init(UIPlay uiPlay, float maxHp, float speed, int dropGold, int dropDarkGold)
    {
        this.uiPlay = uiPlay;

        curHp = maxHp;
        this.maxHp = maxHp;
        this.speed = speed;
        this.dropGold = dropGold;
        this.dropDarkGold = dropDarkGold;
    }

    #region 사각 이동
    private void MoveWaypoint()
    {
        Vector2 dir = curWaypoint.position - transform.position;
        transform.Translate(speed * Time.deltaTime * dir.normalized, Space.World);

        animationController.ChangeState(AnimatorState.Move);

        if (Vector2.Distance(curWaypoint.position, transform.position) <= 0.05f) GetNextWaypoint();
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
    public float TakeDamage(float damage)
    {
        curHp -= damage;

        hp.SetHp(curHp, maxHp);

        if (curHp <= 0) {
            Die();
            Destroy(gameObject);
            return 0;
        }

        return curHp;
    }
    #endregion

    private async void Die()
    {
        uiPlay.SetUI_Gold(dropGold);
        uiPlay.SetUI_Diamond(dropDarkGold);
        uiPlay.SetUI_EnemyCount(-1);

        animationController.ChangeState(AnimatorState.Death);

        try {
            //await animationController.WaitForAnimationEnd(AnimatorState.Death.ToString());
        }
        finally {
            EnemyGenerator.ExistingEnemys.Remove(gameObject);
        }
    }
}