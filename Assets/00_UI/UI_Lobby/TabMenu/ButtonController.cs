using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] protected Button[] btns;

    /// <summary>
    /// �������� ��ư �̺�Ʈ �Ҵ� - �Ű����� 0��
    /// </summary>
    /// <param name="action"></param>
    protected void SetButtonEvent(Action action)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int index = i;
            btns[index].onClick.AddListener(() => {
                action();
                Debug.Log(index + " ��° ��ư�� �̸� : " + btns[index].name + " �� �̺�Ʈ �Ҵ�");
            });
        }
    }

    /// <summary>
    /// �������� ��ư �̺�Ʈ �Ҵ� - �Ű����� 1��
    /// </summary>
    /// <param name="action"></param>
    protected void SetButtonEvent(Action<object> action)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int index = i;
            btns[index].onClick.AddListener(() => {
                action(index);
                Debug.Log(index + " ��° ��ư�� �̸� : " + btns[index].name + " �� �̺�Ʈ �Ҵ�");
            });
        }
    }

    /// <summary>
    /// �������� ��ư �̺�Ʈ ����
    /// </summary>
    protected void RemoveButtonEvent()
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int index = i;
            btns[index].onClick.RemoveAllListeners();
            Debug.Log(index + " ��° ��ư�� �̸� : " + btns[index].name + " �� �̺�Ʈ ����");
        }
    }
}