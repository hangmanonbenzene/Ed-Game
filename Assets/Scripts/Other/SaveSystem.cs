using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/SavedLevels";
    private static string streamingPath = Application.streamingAssetsPath;
    private static string storyPath = Application.persistentDataPath + "/SavedStories";
    
    public static void saveLevel(LevelData levelData)
    {
        string levelName = levelData.levelName;
        createFolder();

        // Convert level data from LevelData object to string
        string levelDataString = JsonUtility.ToJson(levelData);

        // Save level data to file
        System.IO.File.WriteAllText(path + "/" + levelName + ".json", levelDataString);
    }
    public static void deleteLevel(string levelName)
    {
        if (exists(levelName))
        {
            System.IO.File.Delete(path + "/" + levelName + ".json");
        }
    }
    public static LevelData getLevel(string levelName, bool story)
    {
        string path = story ? streamingPath : SaveSystem.path;
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
        createFolder();

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

    public static void saveStory(StoryData storyData)
    {
        string storyName = storyData.SaveName;
        createFolder();

        // Convert story data from StoryData object to string
        string storyDataString = JsonUtility.ToJson(storyData);

        // Save story data to file
        System.IO.File.WriteAllText(storyPath + "/" + storyName + ".json", storyDataString);
    }
    public static void deleteStory(string storyName)
    {
        if (existsStory(storyName))
        {
            System.IO.File.Delete(storyPath + "/" + storyName + ".json");
        }
    }
    public static StoryData getStory(string storyName)
    {
        if (!System.IO.File.Exists(storyPath + "/" + storyName + ".json"))
        {
            return null;
        }

        // Get story data from file
        string storyDataString = System.IO.File.ReadAllText(storyPath + "/" + storyName + ".json");

        // Convert story data from string to StoryData object
        StoryData storyData = JsonUtility.FromJson<StoryData>(storyDataString);

        return storyData;
    }
    public static string[] getAllStoryNames()
    {
        createFolder();

        // Get all files with .json extension in SavedStories folder
        string[] filePaths = System.IO.Directory.GetFiles(storyPath + "/", "*.json");

        // Get story names from file paths
        string[] storyNames = new string[filePaths.Length];
        for (int i = 0; i < filePaths.Length; i++)
        {
            storyNames[i] = filePaths[i].Substring(filePaths[i].LastIndexOf('/') + 1,
                                                                  filePaths[i].LastIndexOf('.') - filePaths[i].LastIndexOf('/') - 1);
        }
        return storyNames;
    }
    public static bool existsStory(string storyName)
    {
        return System.IO.File.Exists(storyPath + "/" + storyName + ".json");
    }
    public static bool existsStoryLevel(string storyLevel)
    {
        return System.IO.File.Exists(streamingPath + "/" + storyLevel + ".json");
    }

    public static void createFolder()
    {
        // Create SavedLevels folder if it doesn't exist
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }
        // Create SavedStories folder if it doesn't exist
        if (!System.IO.Directory.Exists(storyPath))
        {
            System.IO.Directory.CreateDirectory(storyPath);
        }
    }
}
