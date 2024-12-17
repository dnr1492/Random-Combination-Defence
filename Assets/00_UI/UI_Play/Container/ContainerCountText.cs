using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerCountText : MonoBehaviour
{
    private Text countTxt;

    private void Awake()
    {
        countTxt = GetComponentInChildren<Text>();
        countTxt.text = 0.ToString();
    }

    public void Increase()
    {
        countTxt.text = (int.Parse(countTxt.text) + 1).ToString();
    }

    public void Decrease()
    {
        countTxt.text = (int.Parse(countTxt.text) - 1).ToString();
    }

    public int GetCurrentCount() => int.Parse(countTxt.text);
}
