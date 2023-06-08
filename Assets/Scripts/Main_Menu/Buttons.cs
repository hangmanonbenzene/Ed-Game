using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    [SerializeField] protected GameObject mainMenu;

    [SerializeField] protected GameObject[] buttons;
    [SerializeField] protected GameObject backButton;
    [SerializeField] protected GameObject upButton;
    [SerializeField] protected GameObject downButton;
    protected int currentScrollAmount;
    public void onClickBack()
    {
        disableButtons();
        resetButtons();
        mainMenu.GetComponent<MainMenu>().goToMenu(MainMenu.MenuState.MAIN_MENU);
    }
    public void setupButtons()
    {
        currentScrollAmount = 0;
        enableButtons();
        StartCoroutine(SelectFirstButtonDelayed());
    }
    protected void disableButtons()
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }
        if (backButton != null)
        {
            backButton.SetActive(false);
        }
        if (upButton != null && downButton != null)
        {
            upButton.SetActive(false);
            downButton.SetActive(false);
        }
    }
    public void enableButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i >= (4 * currentScrollAmount) && i < (4 * (currentScrollAmount + 1)))
                buttons[i].SetActive(true);
            else
                buttons[i].SetActive(false);
        }
        if (backButton != null)
        {
            backButton.SetActive(true);
        }
        if (upButton != null && downButton != null)
        {
            if (currentScrollAmount == 0)
            {
                upButton.SetActive(false);
            }
            else
            {
                upButton.SetActive(true);
            }
            if (currentScrollAmount == (buttons.Length - 1) / 4)
            {
                downButton.SetActive(false);
            }
            else
            {
                downButton.SetActive(true);
            }
        }
    }
    protected void addButton(GameObject button)
    {
        GameObject[] temp = new GameObject[buttons.Length + 1];
        for (int i = 0; i < buttons.Length; i++)
        {
            temp[i] = buttons[i];
        }
        temp[buttons.Length] = button;
        buttons = temp;
    }
    public IEnumerator SelectFirstButtonDelayed()
    {
        yield return null; // Wait for one frame

        if (buttons.Length > 0)
        {
            buttons[0 + 4 * currentScrollAmount].GetComponent<Button>().Select();
        }
    }
    public void resetButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetComponent<EditorButton>() != null)
            {
                buttons[i].GetComponent<EditorButton>().setPosition(i);
            }
            else if (buttons[i].GetComponent<LevelButton>() != null)
            {
                buttons[i].GetComponent<LevelButton>().setPosition(i);
            }
        }
    }
}
