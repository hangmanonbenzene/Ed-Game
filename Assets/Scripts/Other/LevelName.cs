using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelName
{
    private static string levelName = "";
    private static string menu = "";
    private static bool storyMode = false;

    public static void setLevelName(string levelName)
    {
        LevelName.levelName = levelName;
    }
    public static string getLevelName()
    {
        return levelName;
    }
    public static void setMenu(string lastvisitedMenu)
    {
        LevelName.menu = lastvisitedMenu;
    }
    public static string getMenu()
    {
        return menu;
    }
    public static void setStoryMode(bool storyMode)
    {
        LevelName.storyMode = storyMode;
    }
    public static bool getStoryMode()
    {
        return storyMode;
    }
}
