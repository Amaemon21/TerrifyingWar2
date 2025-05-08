using UnityEngine;

public static class Palette
{
    public static Color HexToColor(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex[0] != '#' || (hex.Length != 7 && hex.Length != 9))
        {
            Debug.LogError($"Hex color '{hex}' is not valid. Use format like '#RRGGBB' or '#RRGGBBAA'.");
            return Color.white;
        }

        byte r = byte.Parse(hex.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
        byte a = 255;

        if (hex.Length == 9)
        {
            a = byte.Parse(hex.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);
        }

        return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
    }

    public static string ColorToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255f);
        int g = Mathf.RoundToInt(color.g * 255f);
        int b = Mathf.RoundToInt(color.b * 255f);
        int a = Mathf.RoundToInt(color.a * 255f);

        return a < 255
            ? $"#{r:X2}{g:X2}{b:X2}{a:X2}"
            : $"#{r:X2}{g:X2}{b:X2}";
    }
}
