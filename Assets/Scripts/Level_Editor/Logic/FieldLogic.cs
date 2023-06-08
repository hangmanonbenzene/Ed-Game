using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldLogic : MonoBehaviour
{
    private LogicGate[] logicGates;

    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject slider;

    [SerializeField] private GameObject logicFields;
    private int number;

    public void setup(LogicGate[] logicGates, int number, int sliderValue)
    {
        this.number = number;
        int length = 0;
        foreach (LogicGate logicGate in logicGates)
        {
            if (logicGate.row == number)
            {
                length++;
            }
        }
        LogicGate[] newLogicGates = new LogicGate[length];
        int i = 0;
        foreach (LogicGate logicGate in logicGates)
        {
            if (logicGate.row == number)
            {
                newLogicGates[i] = logicGate;
                i++;
            }
        }
        this.logicGates = new LogicGate[5];
        for (int j = 0; j < 5; j++)
        {
            if (j < length)
            {
                this.logicGates[j] = newLogicGates[j];
            }
            else
            {
                this.logicGates[j] = new LogicGate(number, j, "empty", new int[0]);
            }
        }
        for (int j = 0; j < 5; j++)
        {
            int index = j;
            buttons[j].GetComponent<Button>().onClick.RemoveAllListeners();
            buttons[j].GetComponent<Button>().onClick.AddListener(() => onClickButton(index));
            buttons[j].GetComponentInChildren<TextMeshProUGUI>().text = TypesOfLogic.getSymbolForType(this.logicGates[j].type);
        }
        if (slider.GetComponent<Slider>().value == sliderValue)
        {
            onChangeValue();
        }
        else
        {
            slider.GetComponent<Slider>().value = sliderValue;
        }
    }

    private void onClickButton(int number)
    {
        Debug.Log(number);
    }
    public void onChangeValue()
    {
        int value = (int)slider.GetComponent<Slider>().value;
        logicFields.GetComponent<LogicFields>().sliderChange(number + 1, value);
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
    }
}
