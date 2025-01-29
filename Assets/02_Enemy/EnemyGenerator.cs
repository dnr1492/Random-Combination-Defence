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

    private Dictionary<string, PlayEnemyData> dicPlayEnemyDatas;
    private Dictionary<int, PlayWaveData> dicPlayWaveDatas;
    private Dictionary<int, PlayMapData> dicPlayMapDatas;

    private GameObject[] arrEnemyPrefab;
    private float curWaveTimer;  //현재 다음 웨이브 시작 전까지 대기한 시간
    private int curWaveIndex;  //현재 진행 중인 웨이브
    private bool startTimer = false;  //시간 체크 시작 유무
    private bool isSpawning = false;  //생성 중 유무
    private float unitMinimumDistance = 2f;  //유닛 간 원하는 간격 (Enemy Scale 1 -> 1f, 2 -> 1.5f, 3 -> 2f)

    private void Awake()
    {
        existingEnemys = new List<GameObject>();

        dicPlayEnemyDatas = DataManager.GetInstance().GetPlayEnemyData();
        dicPlayWaveDatas = DataManager.GetInstance().GetPlayWaveData();
        dicPlayMapDatas = DataManager.GetInstance().GetPlayMapData();

        arrEnemyPrefab = Resources.LoadAll<GameObject>("EnemyPrefabs");
    }

    private void OnEnable()
    {
        startTimer = !startTimer;
        curWaveTimer = dicPlayWaveDatas[curWaveIndex].waveTimer;

        uiPlay.SetUI_Wave(curWaveIndex, dicPlayEnemyDatas.Count);
        uiPlay.SetUI_WaveTimer((int)curWaveTimer);
        uiPlay.SetUI_EnemyCount(0);
    }

    private void Update()
    {
        if (!startTimer) return;
        if (isSpawning) {
            uiPlay.SetUI_WaveSpawning();
            return;
        }
        else uiPlay.SetUI_WaveTimer((int)curWaveTimer);

        CheckWaveTimer();
    }

    private bool CheckWaveTimer()
    {
        if (IsMaximumWave()) return false;

        if (curWaveTimer <= 0) {
            if (!IsMaximumWave()) uiPlay.SetUI_Wave(curWaveIndex + 1, dicPlayEnemyDatas.Count);
            StartCoroutine(SpawnEnemy());
            return true;
        }

        curWaveTimer -= Time.deltaTime;
        return false;
    }

    private IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < dicPlayWaveDatas[curWaveIndex].waveEnemyCount; i++)
        {
            GameObject go = Instantiate(GetEnemyByName(dicPlayWaveDatas[curWaveIndex].waveEnemyName), Waypoint.waypoints[0].position, Waypoint.waypoints[0].rotation, enemyParant);
            go.name = Rename(go.name);
            go.GetComponent<EnemyController>().Init(uiPlay, dicPlayEnemyDatas[go.name].enemyHp, dicPlayEnemyDatas[go.name].enemySpeed, dicPlayEnemyDatas[go.name].dropGold, dicPlayEnemyDatas[go.name].dropDarkGold);
            existingEnemys.Add(go);
            isSpawning = true;
            uiPlay.SetUI_EnemyCount(1);
            uiPlay.SetUI_GameOver(RestartGame);

            //일정 간격 유지
            while (true) {
                if (go == null) break;
                int currentWaypointIndex = 0;
                float progress = currentWaypointIndex + Vector3.Distance(go.transform.position, Waypoint.waypoints[currentWaypointIndex].position);
                if (progress >= unitMinimumDistance) break;
                yield return null;
            }
        }

        curWaveIndex++;
        if (!IsMaximumWave()) curWaveTimer = dicPlayWaveDatas[curWaveIndex].waveTimer;
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
        if (curWaveIndex >= dicPlayEnemyDatas.Count) return true;
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
        uiPlay.SetUI_EnemyCount(-count);
        uiPlay.SetUI_WaveTimer((int)curWaveTimer);
        uiPlay.SetUI_Wave(curWaveIndex, dicPlayEnemyDatas.Count);
    }
}
