using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private static Transform target;
    private float timer = 60f;

    public Text txtTimer;
    public Camera mainCamera;

    private void Awake()
    {
        Time.timeScale = 5;
    }

    private void Update()
    {
        if (target == null || mainCamera == null) {
            gameObject.SetActive(false);
            return;
        }

        //Ÿ�̸� ����
        timer -= Time.deltaTime;
        txtTimer.text = Mathf.Clamp(timer, 0, 999).ToString("F1");

        //������ ���� ��ǥ�� UI ĵ���� ��ǥ�� ��ȯ
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(target.position + Vector3.up * 3f);
        transform.position = screenPosition;
    }

    public static void SetTimer(Transform target)
    {
        Timer.target = target;
    }
}
