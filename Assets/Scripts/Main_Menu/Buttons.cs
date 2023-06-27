using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    [SerializeField] protected GameObject mainMenu;

    [SerializeField] protected GameObject Button;
    [SerializeField] protected GameObject[] buttons;
    [SerializeField] protected GameObject backButton;
    [SerializeField] protected GameObject upButton;
    [SerializeField] protected GameObject downButton;
    protected int currentScrollAmount;
    protected bool isActive;
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
        isActive = false;
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
        isActive = true;
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
        if (buttons == null)
            return;
        for (int i = 0; i < buttons.Length; i++)
        {
            SelectButton button = buttons[i].GetComponent<SelectButton>();
            if (button != null)
                button.setPosition(i);
        }
    }
    public void onClickDown()
    {
        foreach (GameObject button in buttons)
        {
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 800);
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
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - 800);
        }
        currentScrollAmount--;
        enableButtons();
        if (currentScrollAmount == 0)
        {
            StartCoroutine(SelectFirstButtonDelayed());
        }
    }
    public void removeButton(string levelName)
    {
        // Remove button from array
        GameObject[] newButtons = new GameObject[buttons.Length - 1];
        int j = 0;
        int numberOfLevel = 0;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetComponent<SelectButton>().getLevelName() != levelName)
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
            RectTransform rectTransform = buttons[i].GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 200);
        }
    }

    protected void Update()
    {
        if (isActive)
        {
            if (Input.GetKeyDown("joystick button 1"))
            {
                onClickBack();
            }
        }
    }
}
