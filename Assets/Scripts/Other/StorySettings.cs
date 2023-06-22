using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StorySettings
{
    private static string[] levels = new string[] 
    { 
        "test1",
        "test2"
    };
    private static int[][] screensPerLevel = new int[][]
    {
        new int[] { 0, 1 },
        new int[] { 0, 2 }
    };
    public static string getLevel(int index)
    {
        if (index < 0 || index >= levels.Length)
            return "";
        return levels[index];
    }
    public static int getLevelCount()
    {
        return levels.Length;
    }
    public static int[] getScreenNumbers(int index)
    {
        if (index < 0 || index >= screensPerLevel.Length)
            return new int[] { -1, -1 };
        return screensPerLevel[index];
    }
}
