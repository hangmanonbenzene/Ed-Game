using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseGate : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject slider;

    private int selectedButton;

    [SerializeField] private GameObject logicField;

    public void setup(LogicField[] logicFields)
    {
        int length = logicFields.Length;
        if (slider.GetComponent<Slider>().value == length - 1)
        {
            onChangeValue();
        }
        else
        {
            slider.GetComponent<Slider>().value = length - 1;
        }
        for (int i = 0; i < 5; i++)
        {
            int number = i;
            buttons[i].GetComponent<Button>().onClick.AddListener(() => onClickButton(number));
        }
        onClickButton(0);
    }

    private void onClickButton(int number)
    {
        selectedButton = number;
        logicField.GetComponent<LogicFields>().setup(number);
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == number)
            {
                buttons[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1);
            }
            else
            {
                buttons[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color32(50, 50, 50, 255);
            }
        }
    }
    public void onChangeValue()
    {
        int value = (int)slider.GetComponent<Slider>().value;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i <= value)
            {
                buttons[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                buttons[i].GetComponent<Button>().interactable = false;
            }
        }
        if (selectedButton > value)
        {
            onClickButton(value);
        }
    }

    public int getSliderValue()
    {
        return (int)slider.GetComponent<Slider>().value;
    }
}
