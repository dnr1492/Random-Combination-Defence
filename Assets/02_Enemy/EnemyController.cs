using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Hp hp;
    private float curHp;
    private float maxHp;
    private float speed;
    
    private Transform curWaypoint;
    private int waveIndex = 1;

    private void Awake()
    {
        hp = transform.Find("Hp").GetComponent<Hp>();
    }

    private void Start()
    {
        curWaypoint = Waypoint.waypoints[waveIndex];
    }

    private void Update()
    {
        MoveWaypoint();
    }

    public void Init(float maxHp, float speed)
    {
        curHp = maxHp;
        this.maxHp = maxHp;
        this.speed = speed;
    }

    #region 사각 이동
    private void MoveWaypoint()
    {
        Vector2 dir = curWaypoint.position - transform.position;
        transform.Translate(speed * Time.deltaTime * dir.normalized, Space.World);

        if (Vector2.Distance(curWaypoint.position, transform.position) <= 0.05f) GetNextWaypoint();
    }

    private void GetNextWaypoint()
    {
        if (waveIndex >= Waypoint.waypoints.Length - 1)
        {
            waveIndex = 0;
            curWaypoint = Waypoint.waypoints[waveIndex];
            return;
        }

        waveIndex++;
        curWaypoint = Waypoint.waypoints[waveIndex];
    }
    #endregion

    #region 데미지를 입다
    public float TakeDamage(float damage)
    {
        curHp -= damage;

        hp.SetHp(curHp, maxHp);

        if (curHp <= 0) {
            Destroy(gameObject);
            return 0;
        }

        return curHp;
    }
    #endregion
}