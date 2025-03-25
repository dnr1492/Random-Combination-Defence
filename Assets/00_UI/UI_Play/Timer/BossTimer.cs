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
    private BoxCollider2D boxCollider2D;
    private float timer = 75f;
    private bool gameOver = false;

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
        if (timer <= 0 && restartGameAction != null) {
            if (!gameOver) {
                gameOver = true;
                uiPlay.SetUI_GameOver(restartGameAction, gameOver);
            }
        }

        if (target == null || mainCam == null) {
            instance.gameObject.SetActive(false);
            return;
        }

        timer -= Time.deltaTime;
        txtTimer.text = Mathf.Clamp(timer, 0, 999).ToString("F2");

        Vector2 screenPosition = mainCam.WorldToScreenPoint(boxCollider2D.bounds.center + new Vector3(0, boxCollider2D.bounds.extents.y + 1f, 0));
        frame.transform.position = screenPosition;
    }

    public static void SetTimer(Transform target)
    {
        instance.gameObject.SetActive(true);
        instance.target = target;
        instance.boxCollider2D = target.GetComponent<BoxCollider2D>();
        instance.timer = 75f;
        instance.gameOver = false;
    }

    public static void SetRestart(UnityAction restartGameAction)
    {
        instance.restartGameAction = restartGameAction;
    }
}
