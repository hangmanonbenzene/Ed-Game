using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldLogic : MonoBehaviour
{
    private LogicGate[][] logicGates;
    private int selectedField;

    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject slider;

    [SerializeField] private GameObject logicFields;
    private int row;
    private int clickedButton;

    public void setup(LogicGate[][] logicGates, int row)
    {
        this.row = row;

        this.logicGates = new LogicGate[5][];
        for (int i = 0; i < 5; i++)
        {
            this.logicGates[i] = new LogicGate[5];
            int index = 0;
            foreach (LogicGate logicGate in logicGates[i])
            {
                if (logicGate.row == row)
                {
                    this.logicGates[i][index] = logicGate;
                    index++;
                }
            }
            for (int j = index; j < 5; j++)
            {
                this.logicGates[i][j] = new LogicGate(row, j, "empty", new int[0]);
            }
            int number = i;
            buttons[i].GetComponent<Button>().onClick.AddListener(delegate { onClickButton(number); });
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().fontSize = 42;
        }
    }

    public void goToField(int number, int sliderValue)
    {
        selectedField = number;
        for (int i = 0; i < 5; i++)
        {
            string type = this.logicGates[number][i].type;
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = TypesOfLogic.getSymbolForType(type);
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
        clickedButton = number;
        string type = logicGates[selectedField][number].type;
        int typeNumber = System.Array.IndexOf(TypesOfLogic.getTypes(), type);
        int[] inputs = logicGates[selectedField][number].inputs;
        logicFields.GetComponent<ChoosePrompt>().activateGate(this.gameObject, typeNumber, inputs);
    }
    public void onChangeValue()
    {
        int value = (int)slider.GetComponent<Slider>().value;
        logicFields.GetComponent<LogicFields>().sliderChange(row + 1, value);
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

    public LogicGate[][] getLogicGates()
    {
        return logicGates;
    }

    public void onButtonChange(int type, int[] inputs)
    {
        logicGates[selectedField][clickedButton].type = TypesOfLogic.getTypes()[type];
        logicGates[selectedField][clickedButton].inputs = inputs;
        buttons[clickedButton].GetComponentInChildren<TextMeshProUGUI>().text = TypesOfLogic.getSymbolForType(TypesOfLogic.getTypes()[type]);
    }
}
