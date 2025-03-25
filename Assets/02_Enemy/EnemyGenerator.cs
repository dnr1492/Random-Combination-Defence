using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public static List<GameObject> ExistingEnemys { get => existingEnemys; private set => existingEnemys = value; }
    private static List<GameObject> existingEnemys;  //�����ϴ� ����

    [SerializeField] UIPlay uiPlay;
    [SerializeField] Transform enemyParant;

    private Dictionary<int, PlayWaveData> dicPlayWaveDatas;

    private GameObject[] arrEnemyPrefab;
    private float curWaveTimer;  //���� ���� ���̺� ���� ������ ����� �ð�
    private int curWaveIndex = 10;  //1;  //���� ���� ���� ���̺�
    private bool isSpawning = false;  //���� �� ����

    private void Awake()
    {
        existingEnemys = new List<GameObject>();

        dicPlayWaveDatas = DataManager.GetInstance().GetPlayWaveData();

        arrEnemyPrefab = Resources.LoadAll<GameObject>("EnemyPrefabs");
    }

    private void OnEnable()
    {
        curWaveTimer = 5f;  //dicPlayWaveDatas[curWaveIndex].waveTimer;

        uiPlay.SetUI_Wave(0, dicPlayWaveDatas.Count);
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
            //�ӽ� �������� ��� ���忡 �ߺ����� ����ϱ� ���ؼ� �� ��° �ڸ����� �������� ������ ����
            //��, �Ϲ� ���� 1 2 3 4 5 6 7 8 9 ���̸� 0�� ����
            int wave = dicPlayWaveDatas[curWaveIndex].wave;
            int prefabIndex = (wave - 1) % 10;  // 0~9 �ݺ�
            int secondDigit;
            if (wave % 10 == 0) secondDigit = 0;  //10, 20, 30... ���� ���̺�
            else if (wave > 10) secondDigit = prefabIndex + 1;  //11���� �ٽ� 1~9 �ݺ�
            else secondDigit = prefabIndex + 1;  //�⺻ 1~9 �ݺ�

            GameObject go = Instantiate(GetEnemyByName(secondDigit.ToString()), Waypoint.waypoints[0].position, Waypoint.waypoints[0].rotation, enemyParant);
            go.name = Rename(go.name);
            go.GetComponent<EnemyController>().Init(uiPlay, dicPlayWaveDatas[curWaveIndex].enemyHp, dicPlayWaveDatas[curWaveIndex].enemyDefense, dicPlayWaveDatas[curWaveIndex].enemySpeed);
            float enemyInterval = go.GetComponent<BoxCollider2D>().bounds.size.magnitude;
            existingEnemys.Add(go);
            uiPlay.SetUI_EnemyCount();
            uiPlay.SetUI_GameOver(RestartGame);

            if (dicPlayWaveDatas[curWaveIndex].enemyType == EnemyType.����) {
                BossTimer.SetTimer(go.transform);
                BossTimer.SetRestart(RestartGame);
            }

            //���� ���� ����
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
        //���� �� ����
        var count = existingEnemys.Count;
        existingEnemys.RemoveAll(enemy => {
            Destroy(enemy);
            return true;
        });

        //���� ���̺� �ʱ�ȭ
        StopAllCoroutines();
        isSpawning = false;
        curWaveTimer = dicPlayWaveDatas[curWaveIndex].waveTimer;

        //UI �ʱ�ȭ
        uiPlay.SetUI_Wave(curWaveIndex, dicPlayWaveDatas.Count);
        uiPlay.SetUI_WaveTimer(curWaveTimer);
        uiPlay.SetUI_EnemyCount();
    }
}
