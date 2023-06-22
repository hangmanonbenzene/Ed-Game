using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostScreens : MonoBehaviour
{
    [SerializeField] private GameObject[] screens;
    private int currentScreen;
    private GameObject currentScreenObject;
    [SerializeField] private GameObject screenHolder;

    private void Start()
    {
        if (screens != null && screens.Length > 0)
        {
            currentScreen = 0;
            currentScreenObject = Instantiate(screens[currentScreen], screenHolder.transform);
        }
        else
        {
            nextLevel();
        }
    }

    public void onClickDone()
    {
        currentScreen++;
        if (currentScreen < screens.Length)
        {
            Destroy(currentScreenObject);
            currentScreenObject = Instantiate(screens[currentScreen], screenHolder.transform);
        }
        else
        {
            nextLevel();
        }
    }

    private void nextLevel()
    {
        string levelName = LevelName.getLevelName();
        int level = SaveSystem.getStory(levelName).Level + 1;
        if (StorySettings.getLevelCount() > level)
        {
            SaveSystem.saveStory(new StoryData(levelName, level));
            SceneManager.LoadScene(1);
        }
        else
        {
            LevelName.setMenu(LevelName.getStoryMode() ? "Story" : "Level");
            SceneManager.LoadScene(0);
        }
    }
}
