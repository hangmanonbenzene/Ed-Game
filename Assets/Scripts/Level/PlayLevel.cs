using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLevel : MonoBehaviour
{
    [SerializeField] private GameObject field;
    [SerializeField] private GameObject logic;

    [SerializeField] private GameObject[] preAndPostScreens;
    [SerializeField] private GameObject screenHolder;

    void Start()
    {
        string levelName = string.Empty;
        if (LevelName.getStoryMode())
        {
            levelName = LevelName.getLevelName();
            StoryData story = SaveSystem.getStory(levelName);
            if (story == null)
            {
                Debug.Log("Story " + levelName + " does not exist");
                return;
            }
            else
            {
                levelName = StorySettings.getLevel(story.Level);
            }
            if (!SaveSystem.existsStoryLevel(levelName))
            {
                Debug.Log("Storylevel " + levelName + " does not exist");
                return;
            }
            else
            {
                int[] screens = StorySettings.getScreenNumbers(story.Level);
                if (screens[0] >= 0 && screens[0] < preAndPostScreens.Length)
                {
                    Instantiate(preAndPostScreens[screens[0]], screenHolder.transform);
                }
            }
        }
        else
        {
            levelName = LevelName.getLevelName();
            if (!SaveSystem.exists(levelName))
            {
                Debug.Log("Level " + levelName + " does not exist");
                return;
            }
        }
        LevelData levelData = SaveSystem.getLevel(levelName, LevelName.getStoryMode());
        field.GetComponent<LevelField>().setupField(levelData.field);
        logic.GetComponent<LevelLogic>().setupLogic(levelData.logicField, levelData.restrictedGates);
    }

    public GameObject getPostScreen(int number)
    {
        return preAndPostScreens[number];
    }
}
