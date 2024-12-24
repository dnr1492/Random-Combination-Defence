using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] UIPlay uiPlay;

    private Dictionary<string, PlayEnemyData> dicPlayEnemyDatas;
    private Dictionary<int, PlayWaveData> dicPlayWaveDatas;
    private Dictionary<int, PlayMapData> dicPlayMapDatas;

    private GameObject[] arrEnemyPrefab;
    private float curWaveTimer;  //���� ���� ���̺� ���� ������ ����� �ð�
    private int curWaveIndex;  //���� ���� ���� ���̺�
    private int curAliveEnemyCount = 0;  //���� ����ִ� �� ���� ��
    private int curMapId = 10000;  //���� �� ID
    private bool startTimer = false;  //�ð� üũ ���� ����
    private bool isSpawning = false;  //���� �� ����
    private float spawnWaitingTimer = 0.25f;  //�� ���� ��� �ð�

    private void Awake()
    {
        dicPlayEnemyDatas = DataManager.instance.GetPlayEnemyData();
        dicPlayWaveDatas = DataManager.instance.GetPlayWaveData();
        dicPlayMapDatas = DataManager.instance.GetPlayMapData();

        arrEnemyPrefab = Resources.LoadAll<GameObject>("EnemyPrefabs");
    }

    private void OnEnable()
    {
        startTimer = !startTimer;
        curWaveTimer = dicPlayWaveDatas[curWaveIndex].wave_timer;

        uiPlay.SetUI_Wave(curWaveIndex);
        uiPlay.SetUI_WaveTimer((int)curWaveTimer);
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
            go.GetComponent<EnemyController>().Init(dicPlayEnemyDatas[go.name].enemy_hp, dicPlayEnemyDatas[go.name].enemy_speed);
            curAliveEnemyCount++;
            isSpawning = true;
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
        if (curWaveIndex >= dicPlayMapDatas[curMapId].maximum_wave) return true;  //�ʰ�
        return false;
    }
}
