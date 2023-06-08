using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : Buttons
{
    [SerializeField] private GameObject levelButton;

    private void Start()
    {
        string[] levelNames = SaveSystem.getAllNames();
        for (int i = 0; i < levelNames.Length; i++)
        {
            GameObject button = Instantiate(levelButton, transform);
            button.GetComponent<LevelButton>().setupButton(i, levelNames[i], this);
            button.SetActive(false);
            addButton(button);
        }
    }

    public void onClickDown()
    {
        foreach (GameObject button in buttons)
        {
            button.GetComponent<LevelButton>().move(4);
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
            button.GetComponent<LevelButton>().move(-4);
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
        mainMenu.GetComponent<MainMenu>().goToScene(1, levelName);
    }
    public void removeButton(string levelName)
    {
        // Remove button from array
        GameObject[] newButtons = new GameObject[buttons.Length - 1];
        int j = 0;
        int numberOfLevel = 0;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetComponent<LevelButton>().getLevelName() != levelName)
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
            buttons[i].GetComponent<LevelButton>().move(1);
        }
    }
}
