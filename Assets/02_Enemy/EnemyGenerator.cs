using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public static List<GameObject> ExistingEnemys { get => existingEnemys; private set => existingEnemys = value; }
    private static List<GameObject> existingEnemys;  //�����ϴ� ����

    [SerializeField] UIPlay uiPlay;

    private Dictionary<string, PlayEnemyData> dicPlayEnemyDatas;
    private Dictionary<int, PlayWaveData> dicPlayWaveDatas;
    private Dictionary<int, PlayMapData> dicPlayMapDatas;

    private GameObject[] arrEnemyPrefab;
    private float curWaveTimer;  //���� ���� ���̺� ���� ������ ����� �ð�
    private int curWaveIndex;  //���� ���� ���� ���̺�
    private bool startTimer = false;  //�ð� üũ ���� ����
    private bool isSpawning = false;  //���� �� ����
    private float spawnWaitingTimer = 0.25f;  //�� ���� ��� �ð�

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
        curWaveTimer = dicPlayWaveDatas[curWaveIndex].wave_timer;

        uiPlay.SetUI_Wave(curWaveIndex);
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
            if (!IsMaximumWave()) uiPlay.SetUI_Wave(curWaveIndex + 1);
            StartCoroutine(SpawnEnemy());
            return true;
        }

        curWaveTimer -= Time.deltaTime;
        return false;
    }

    private IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < dicPlayWaveDatas[curWaveIndex].wave_enemy_count; i++)
        {
            GameObject go = Instantiate(GetEnemyByName(dicPlayWaveDatas[curWaveIndex].wave_enemy_name), Waypoint.waypoints[0].position, Waypoint.waypoints[0].rotation);
            go.name = Rename(go.name);
            go.GetComponent<EnemyController>().Init(uiPlay, dicPlayEnemyDatas[go.name].enemy_hp, dicPlayEnemyDatas[go.name].enemy_speed, dicPlayEnemyDatas[go.name].drop_gold, dicPlayEnemyDatas[go.name].drop_dark_gold);
            existingEnemys.Add(go);
            isSpawning = true;
            uiPlay.SetUI_EnemyCount(1);
            uiPlay.SetUI_GameOver(RestartGame);
            yield return new WaitForSeconds(spawnWaitingTimer);
        }

        curWaveIndex++;
        if (!IsMaximumWave()) curWaveTimer = dicPlayWaveDatas[curWaveIndex].wave_timer;
        isSpawning = false;
    }

    private GameObject GetEnemyByName(string name)
    {
        for (int i = 0; i < arrEnemyPrefab.Length; i++) {
            if (arrEnemyPrefab[i].name == name) return arrEnemyPrefab[i];
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
        if (curWaveIndex >= dicPlayMapDatas[uiPlay.GetCurMapId].maximum_wave) return true;
        return false;
    }

    private void RestartGame()
    {
        //���� �� ����
        var count = existingEnemys.Count;
        existingEnemys.RemoveAll(enemy => {
            Destroy(enemy);
            return true;
        });

        //���� ���̺� �ʱ�ȭ
        StopAllCoroutines();
        isSpawning = false;
        curWaveTimer = dicPlayWaveDatas[curWaveIndex].wave_timer;

        //UI �ʱ�ȭ
        uiPlay.SetUI_EnemyCount(-count);
        uiPlay.SetUI_WaveTimer((int)curWaveTimer);
        uiPlay.SetUI_Wave(curWaveIndex);
    }
}
