using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private static Timer instance;

    private Transform target;
    private float timer = 75f;

    [SerializeField] GameObject frame;
    [SerializeField] Camera mainCam;
    [SerializeField] Text txtTimer;

    private void Awake()
    {
        instance = this;
        instance.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (target == null || mainCam == null) {
            instance.gameObject.SetActive(false);
            return;
        }

        timer -= Time.deltaTime;
        txtTimer.text = Mathf.Clamp(timer, 0, 999).ToString("F2");

        Vector2 screenPosition = mainCam.WorldToScreenPoint(target.position + Vector3.up * 3.5f);
        frame.transform.position = screenPosition;
    }

    public static void SetTimer(Transform target)
    {
        instance.gameObject.SetActive(true);
        instance.target = target;
        instance.timer = 60f;
    }
}
