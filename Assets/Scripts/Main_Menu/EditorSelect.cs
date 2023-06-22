using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSelect : Buttons
{
    [SerializeField] private GameObject areYouSure;
    [SerializeField] private GameObject levelSelect;

    private void Start()
    {
        string[] justLevels = SaveSystem.getAllNames();
        string[] levelNames = new string[justLevels.Length + 1];
        levelNames[0] = "Neues Level";
        for (int i = 0; i < justLevels.Length; i++)
        {
            levelNames[i + 1] = justLevels[i];
        }
        for (int i = 0; i < levelNames.Length; i++)
        {
            GameObject button = Instantiate(Button, transform);
            button.GetComponent<EditorButton>().setupButton(i, levelNames[i], this);
            addButton(button);
        }
    }

    public void loadLevel(string levelName)
    {
        LevelName.setStoryMode(false);
        mainMenu.GetComponent<MainMenu>().goToScene(2, levelName);
    }
    public void activateAreYouSure(string levelName)
    {
        disableButtons();
        areYouSure.GetComponent<AreYouSure>().setupAreYouSure(levelName, false);
    }
}
