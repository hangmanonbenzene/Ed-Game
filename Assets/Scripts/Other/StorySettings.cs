using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StorySettings
{
    private static string[] levels = new string[] 
    { 
        "Level 1",
        "Level 2",
        "Level 3",
        "Level 4",
        "Level 5",
        "Level 6",
        "Level 10",
        "Level 11",
        "Level 12"
    };
    private static int[][] screensPerLevel = new int[][]
    {
        new int[] { 3, 2 },
        new int[] { 1, 2 },
        new int[] { 1, 2 },
        new int[] { 1, 2 },
        new int[] { 1, 2 },
        new int[] { 1, 2 },
        new int[] { 1, 2 },
        new int[] { 1, 2 },
        new int[] { 1, 0 }
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
