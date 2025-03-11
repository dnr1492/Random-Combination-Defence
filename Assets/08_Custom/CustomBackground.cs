using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomBackground : MonoBehaviour
{
    [SerializeField] Image bg;
    [SerializeField] Sprite[] sps;  //Unselect 0, Select 1

    /// <summary>
    /// 이미지 자체를 변경
    /// </summary>
    /// <param name="isSelect"></param>
    public void SetSelect(bool isSelect)
    {
        int curSelectIndex = isSelect ? 1 : 0;
        if (sps != null && sps.Length > 0) bg.sprite = sps[curSelectIndex];
    }

    public void SetSelect(int index)
    {
        bg.sprite = sps[index];
    }

    public void UnlockSkill()
    {
        bg.sprite = sps[2];
    }

    /// <summary>
    /// 이미지 색상 변경
    /// </summary>
    /// <param name="isSelect"></param>
    /// <param name="selectColor"></param>
    /// <param name="defaultColor"></param>
    public void SetSelect(bool isSelect, Color selectColor, Color defaultColor)
    {
        bg.color = isSelect == true ? selectColor : defaultColor;
    }
}

public class CustomBackgroundText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bg;

    public virtual void SetSelectColor(bool isSelect, string selectHex, string unselectHex)
    {
        if (isSelect) bg.color = HexToColor(selectHex);
        else bg.color = HexToColor(unselectHex);
    }

    private Color HexToColor(string hex)
    {
        if (hex.StartsWith("#")) hex = hex.Substring(1);

        byte r = 255, g = 255, b = 255, a = 255;

        if (hex.Length == 6)
        {
            r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        }
        else if (hex.Length == 8)
        {
            r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }

        return new Color32(r, g, b, a);
    }
}
