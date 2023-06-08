using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSelect : Buttons
{
    [SerializeField] private GameObject areYouSure;
    [SerializeField] private GameObject levelSelect;

    [SerializeField] private GameObject levelButton;

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
            GameObject button = Instantiate(levelButton, transform);
            button.GetComponent<EditorButton>().setupButton(i, levelNames[i], this);
            button.SetActive(false);
            addButton(button);
        }
    }

    public void onClickDown()
    {
        foreach (GameObject button in buttons)
        {
            button.GetComponent<EditorButton>().move(4);
        }
        currentScrollAmount++;
        enableButtons();
        if (currentScrollAmount == (buttons.Length - 1) / 4)
        {
            StartCoroutine(SelectFirstButtonDelayed());
        }
    }
    public void onClickUp()
    {
        foreach (GameObject button in buttons)
        {
            button.GetComponent<EditorButton>().move(-4);
        }
        currentScrollAmount--;
        enableButtons();
        if (currentScrollAmount == 0)
        {
            StartCoroutine(SelectFirstButtonDelayed());
        }
    }

    public void loadLevel(string levelName)
    {
        mainMenu.GetComponent<MainMenu>().goToScene(2, levelName);
    }
    public void activateAreYouSure(string levelName)
    {
        disableButtons();
        areYouSure.GetComponent<AreYouSure>().setupAreYouSure(levelName);
    }
    public void removeButton(string levelName)
    {
        // Remove button from array
        GameObject[] newButtons = new GameObject[buttons.Length - 1];
        int j = 0;
        int numberOfLevel = 0;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetComponent<EditorButton>().getLevelName() != levelName)
            {
                newButtons[j] = buttons[i];
                j++;
            }
            else
            {
                Destroy(buttons[i]);
                numberOfLevel = i;
            }
        }
        buttons = newButtons;

        // Move buttons up
        for (int i = numberOfLevel; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<EditorButton>().move(1);
        }

        removeButtonFromLevelSelect(levelName);
    }
    private void removeButtonFromLevelSelect(string levelName)
    {
        levelSelect.GetComponent<LevelSelect>().removeButton(levelName);
    }
}
