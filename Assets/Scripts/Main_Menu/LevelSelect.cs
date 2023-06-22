using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : Buttons
{
    private void Start()
    {
        string[] levelNames = SaveSystem.getAllNames();
        for (int i = 0; i < levelNames.Length; i++)
        {
            GameObject button = Instantiate(Button, transform);
            button.GetComponent<LevelButton>().setupButton(i, levelNames[i], this);
            addButton(button);
        }
    }

    public void loadLevel(string levelName)
    {
        LevelName.setStoryMode(false);
        mainMenu.GetComponent<MainMenu>().goToScene(1, levelName);
    }
}
