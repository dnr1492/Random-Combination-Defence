using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BossTimer : MonoBehaviour
{
    private static BossTimer instance;

    private UnityAction restartGameAction;
    private Transform target;
    private float localScaleY;
    private float offsetY;
    private float timer = 75f;

    [SerializeField] UIPlay uiPlay;
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
        if (timer <= 0 && restartGameAction != null) uiPlay.SetUI_GameOver(restartGameAction);

        if (target == null || mainCam == null) {
            instance.gameObject.SetActive(false);
            return;
        }

        timer -= Time.deltaTime;
        txtTimer.text = Mathf.Clamp(timer, 0, 999).ToString("F2");

        Vector2 screenPosition = mainCam.WorldToScreenPoint(target.position + localScaleY * offsetY * Vector3.up);
        frame.transform.position = screenPosition;
    }

    public static void SetTimer(Transform target)
    {
        instance.gameObject.SetActive(true);
        instance.target = target;
        instance.offsetY = target.GetComponent<BoxCollider2D>().bounds.extents.y * 1.5f;
        instance.localScaleY = instance.gameObject.transform.localScale.y;
        instance.timer = 75f;
    }

    public static void SetRestart(UnityAction restartGameAction)
    {
        instance.restartGameAction = restartGameAction;
    }
}
