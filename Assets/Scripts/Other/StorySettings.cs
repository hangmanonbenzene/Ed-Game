using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StorySettings
{
    private static string[] levels = new string[] 
    { 
        "Level 01.0",
        "Level 02.0",
        "Level 03.0",
        "Level 04.1",
        "Level 05.0",
        "Level 06.1",
        "Level 07.0",
        "Level 08.0",
        "Level 09.0"
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
