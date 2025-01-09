using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager
{
    public static Color HexToColor(string hex)
    {
        if (string.IsNullOrEmpty(hex)) return Color.white;
        hex = hex.Replace("#", "");
        byte r = 255, g = 255, b = 255, a = 255;

        //#RRGGBB
        if (hex.Length == 6) 
        {
            r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        }
        //#RRGGBBAA
        else if (hex.Length == 8) 
        {
            r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        else
        {
            DebugLogger.Log("Hex 코드 형식이 잘못되었습니다.");
        }

        return new Color32(r, g, b, a);
    }
}
