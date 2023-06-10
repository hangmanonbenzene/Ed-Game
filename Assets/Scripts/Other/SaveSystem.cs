using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath;
    private static string oldPath = Application.dataPath + "/SavedLevels";
    public static void saveLevel(LevelData levelData, string levelName)
    {
        // Convert level data from LevelData object to string
        string levelDataString = JsonUtility.ToJson(levelData);

        // Save level data to file
        System.IO.File.WriteAllText(path + "/" + levelName + ".json", levelDataString);
    }
    public static void deleteLevel(string levelName)
    {
        // Delete level data file
        System.IO.File.Delete(path + "/" + levelName + ".json");
    }
    public static LevelData getLevel(string levelName)
    {
        if (!System.IO.File.Exists(path + "/" + levelName + ".json"))
        {
            return null;
        }

        // Get level data from file
        string levelDataString = System.IO.File.ReadAllText(path + "/" + levelName + ".json");

        // Convert level data from string to LevelData object
        LevelData levelData = JsonUtility.FromJson<LevelData>(levelDataString);

        return levelData;
    }
    public static string[] getAllNames()
    {
        // Get all files with .json extension in SavedLevels folder
        string[] filePaths = System.IO.Directory.GetFiles(path + "/", "*.json");

        // Get level names from file paths
        string[] levelNames = new string[filePaths.Length];
        for (int i = 0; i < filePaths.Length; i++)
        {
            levelNames[i] = filePaths[i].Substring(filePaths[i].LastIndexOf('/') + 1, 
                                                   filePaths[i].LastIndexOf('.') - filePaths[i].LastIndexOf('/') - 1);
        }
        return levelNames;
    }
    public static bool exists(string levelName)
    {
        return System.IO.File.Exists(path + "/" + levelName + ".json");
    }
}
