using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    [SerializeField] private GameObject field;
    [SerializeField] private GameObject logicField;

    [SerializeField] private GameObject fieldButton;
    [SerializeField] private GameObject logicButton;

    public void onClickField()
    {
        field.SetActive(true);
        logicField.SetActive(false);
        activateButton(fieldButton, true);
        activateButton(logicButton, false);
    }
    public void onClickLogic()
    {
        field.SetActive(false);
        logicField.SetActive(true);
        activateButton(fieldButton, false);
        activateButton(logicButton, true);
    }
    private void activateButton(GameObject button, bool activate)
    {
        if (activate)
        {
            button.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            button.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1);
        }
        else
        {
            button.GetComponent<Image>().color = new Color(1, 1, 1);
            button.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(50, 50, 50, 255);
        }
    }
}
