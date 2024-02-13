using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueCheck : MonoBehaviour
{
    public static int ConvertToInteger(Text text)
    {
        return int.Parse(text.text);
    }

    public static float ConvertToFloat(Text text)
    {
        return float.Parse(text.text);
    }

    public static int ConvertToInteger(string text)
    {
        return int.Parse(text);
    }

    public static float ConvertToFloat(string text)
    {
        return float.Parse(text);
    }
}
