using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] protected Button[] btns;

    /// <summary>
    /// 동적으로 버튼 이벤트 할당 - 매개변수 0개
    /// </summary>
    /// <param name="action"></param>
    protected void SetButtonEvent(Action action)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int index = i;
            btns[index].onClick.AddListener(() => {
                action();
                Debug.Log(index + " 번째 버튼의 이름 : " + btns[index].name + " 의 이벤트 할당");
            });
        }
    }

    /// <summary>
    /// 동적으로 버튼 이벤트 할당 - 매개변수 1개
    /// </summary>
    /// <param name="action"></param>
    protected void SetButtonEvent(Action<object> action)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int index = i;
            btns[index].onClick.AddListener(() => {
                action(index);
                Debug.Log(index + " 번째 버튼의 이름 : " + btns[index].name + " 의 이벤트 할당");
            });
        }
    }

    /// <summary>
    /// 동적으로 버튼 이벤트 제거
    /// </summary>
    protected void RemoveButtonEvent()
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int index = i;
            btns[index].onClick.RemoveAllListeners();
            Debug.Log(index + " 번째 버튼의 이름 : " + btns[index].name + " 의 이벤트 제거");
        }
    }
}