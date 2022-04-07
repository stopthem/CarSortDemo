using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtensions
{
    public static Color WithA(this Color color, float a)
    {
        var toColor = color;
        toColor.a = a;
        return toColor;
    }
}
