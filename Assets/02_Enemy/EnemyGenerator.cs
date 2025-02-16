using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public static List<GameObject> ExistingEnemys { get => existingEnemys; private set => existingEnemys = value; }
    private static List<GameObject> existingEnemys;  //존재하는 적들

    [SerializeField] UIPlay uiPlay;
    [SerializeField] Transform enemyParant;

    private Dictionary<int, PlayWaveData> dicPlayWaveDatas;

    private GameObject[] arrEnemyPrefab;
    private float curWaveTimer;  //현재 다음 웨이브 시작 전까지 대기한 시간
    private int curWaveIndex = 1;  //현재 진행 중인 웨이브
    private bool isSpawning = false;  //생성 중 유무

    private void Awake()
    {
        existingEnemys = new List<GameObject>();

        dicPlayWaveDatas = DataManager.GetInstance().GetPlayWaveData();

        arrEnemyPrefab = Resources.LoadAll<GameObject>("EnemyPrefabs");
    }

    private void OnEnable()
    {
        curWaveTimer = dicPlayWaveDatas[curWaveIndex].waveTimer;
        
        uiPlay.SetUI_Wave(curWaveIndex, dicPlayWaveDatas.Count);
        uiPlay.SetUI_WaveTimer(curWaveTimer);
        uiPlay.SetUI_EnemyCount();
    }

    private void Update()
    {
        CheckWaveTimer();
    }

    private bool CheckWaveTimer()
    {
        uiPlay.SetUI_WaveTimer(curWaveTimer);

        if (IsMaximumWave()) return false;

        if (curWaveTimer <= 0 && !isSpawning) {
            if (!IsMaximumWave()) uiPlay.SetUI_Wave(curWaveIndex, dicPlayWaveDatas.Count);
            StartCoroutine(SpawnEnemy());
            return true;
        }

        curWaveTimer -= Time.deltaTime;
        return false;
    }

    private IEnumerator SpawnEnemy()
    {
        isSpawning = true;
        if (!IsMaximumWave()) curWaveTimer = dicPlayWaveDatas[curWaveIndex + 1].waveTimer;

        for (int i = 0; i < dicPlayWaveDatas[curWaveIndex].waveEnemyCount; i++)
        {
            GameObject go = Instantiate(GetEnemyByName(dicPlayWaveDatas[curWaveIndex].wave.ToString()), Waypoint.waypoints[0].position, Waypoint.waypoints[0].rotation, enemyParant);
            go.name = Rename(go.name);
            go.GetComponent<EnemyController>().Init(uiPlay, dicPlayWaveDatas[curWaveIndex].enemyHp, dicPlayWaveDatas[curWaveIndex].enemyDefense, dicPlayWaveDatas[curWaveIndex].enemySpeed);
            float enemyInterval = go.GetComponent<BoxCollider2D>().bounds.size.magnitude;
            existingEnemys.Add(go);
            uiPlay.SetUI_EnemyCount();
            uiPlay.SetUI_GameOver(RestartGame);

            //일정 간격 유지
            while (true) {
                if (go == null) break;
                int currentWaypointIndex = 0;
                float progress = currentWaypointIndex + Vector3.Distance(go.transform.position, Waypoint.waypoints[currentWaypointIndex].position);
                if (progress >= enemyInterval) break;
                yield return null;
            }
        }

        curWaveIndex++;
        isSpawning = false;
    }

    private GameObject GetEnemyByName(string enemyName)
    {
        for (int i = 0; i < arrEnemyPrefab.Length; i++) {
            if (arrEnemyPrefab[i].name == enemyName) return arrEnemyPrefab[i];
        }
        return null;
    }

    private string Rename(string name)
    {
        name = name.Split("(")[0];
        return name;
    }

    private bool IsMaximumWave()
    {
        if (curWaveIndex >= dicPlayWaveDatas.Count) return true;
        return false;
    }

    private void RestartGame()
    {
        //기존 적 제거
        var count = existingEnemys.Count;
        existingEnemys.RemoveAll(enemy => {
            Destroy(enemy);
            return true;
        });

        //현재 웨이브 초기화
        StopAllCoroutines();
        isSpawning = false;
        curWaveTimer = dicPlayWaveDatas[curWaveIndex].waveTimer;

        //UI 초기화
        uiPlay.SetUI_Wave(curWaveIndex, dicPlayWaveDatas.Count);
        uiPlay.SetUI_WaveTimer(curWaveTimer);
        uiPlay.SetUI_EnemyCount();
    }
}
