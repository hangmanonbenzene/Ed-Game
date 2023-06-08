using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AreYouSure : MonoBehaviour
{
    private string levelName;
    [SerializeField] private GameObject levelNameText;
    [SerializeField] private GameObject editorSelect;
    [SerializeField] private GameObject yesButton;
    [SerializeField] private GameObject popUp;
    public void setupAreYouSure(string levelName)
    {
        this.levelName = levelName;
        activatePopUp();
        levelNameText.GetComponent<TextMeshProUGUI>().text = levelName;
        yesButton.GetComponent<Button>().Select();
    }

    public void onClickYes()
    {
        SaveSystem.deleteLevel(levelName);
        editorSelect.GetComponent<EditorSelect>().removeButton(levelName);
        editorSelect.GetComponent<EditorSelect>().resetButtons();
        editorSelect.GetComponent<EditorSelect>().setupButtons();
        deactivatePopUp();
    }
    public void onClickNo()
    {
        editorSelect.GetComponent<EditorSelect>().enableButtons();
        StartCoroutine(editorSelect.GetComponent<EditorSelect>().SelectFirstButtonDelayed());
        deactivatePopUp();
    }

    public void activatePopUp()
    {
        popUp.SetActive(true);
    }
    public void deactivatePopUp()
    {
        popUp.SetActive(false);
    }
}
