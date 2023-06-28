using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextReplace
{
    public static string Replace<T>(string textToReplace, params T[] values)
    {
        string newString ="";
        for(int i = 0; i < values.Length; i++)
        {
            newString = textToReplace.Replace("{" + i + "}", values[i].ToString());
        }
        return newString;
    }
}
