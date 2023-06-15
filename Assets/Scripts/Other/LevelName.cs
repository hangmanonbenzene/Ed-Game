using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelName : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private string menu;

    private static bool created = false;

    private void Awake()
    {
        if (!created)
        {
            // Keep this object when loading new scenes
            DontDestroyOnLoad(gameObject);
            created = true;
            levelName = "";
            menu = "";
        }
        else
        {
            // Object has already been created, destroy the duplicate
            Destroy(gameObject);
        }
    }

    public void setLevelName(string levelName)
    {
        this.levelName = levelName;
    }
    public string getLevelName()
    {
        return levelName;
    }
    public void setMenu(string lastvisitedMenu)
    {
        this.menu = lastvisitedMenu;
    }
    public string getMenu()
    {
        return menu;
    }
}
