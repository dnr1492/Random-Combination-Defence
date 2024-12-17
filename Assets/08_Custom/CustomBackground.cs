using UnityEngine;
using UnityEngine.UI;

public class CustomBackground : MonoBehaviour
{
    [SerializeField] Image bg;
    [SerializeField] Sprite[] sps;
    private int currentSelectID;

    public void SetSelect(bool isSelect)
    {
        currentSelectID = isSelect ? 1 : 0;  //Select 1, Unselect 0
        if (sps != null && sps.Length > 0) bg.sprite = sps[currentSelectID];
    }

    public void Unlock()
    {
        bg.sprite = sps[2];
    }
}
