using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldOutput : MonoBehaviour
{
    private SensorOutput[][] sensorOutputs;
    private int selectedField;

    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject slider;

    [SerializeField] private GameObject logicFields;
    private int clickedButton;

    public void setup(SensorOutput[][] sensorOutputs)
    {
        this.sensorOutputs = new SensorOutput[5][];
        for (int i = 0; i < 5; i++)
        {
            this.sensorOutputs[i] = new SensorOutput[5];
            for (int j = 0; j < 5; j++)
            {
                if (j < sensorOutputs[i].Length)
                {
                    this.sensorOutputs[i][j] = sensorOutputs[i][j];
                }
                else
                {
                    string type = TypesOfOutputs.getTypes()[0];
                    string spec = TypesOfOutputs.getSpecificationsForType(type)[0];
                    this.sensorOutputs[i][j] = new SensorOutput(type, spec, new int[0]);
                }
            }
            int number = i;
            buttons[i].GetComponent<Button>().onClick.AddListener(() => onClickButton(number));
        }
    }

    public void goToField(int number, int sliderValue)
    {
        selectedField = number;
        for (int i = 0; i < 5; i++)
        {
            string type = this.sensorOutputs[number][i].type;
            string spec = this.sensorOutputs[number][i].specification;
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = TypesOfOutputs.getSymbolForType(type, spec);
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
        string type = this.sensorOutputs[selectedField][number].type;
        int typenumber = System.Array.IndexOf(TypesOfOutputs.getTypes(), type);
        int spec = System.Array.IndexOf(TypesOfOutputs.getSpecificationsForType(type), sensorOutputs[selectedField][number].specification);
        int[] inputs = sensorOutputs[selectedField][number].inputs;
        logicFields.GetComponent<ChoosePrompt>().activateOutput(this.gameObject, typenumber, spec, inputs);
    }
    public void onChangeValue()
    {
        int value = (int)slider.GetComponent<Slider>().value;
        logicFields.GetComponent<LogicFields>().sliderChange(4, value);
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

    public SensorOutput[][] getSensorOutputs()
    {
        return sensorOutputs;
    }

    public void onButtonChange(int type, int spec, int[] inputs)
    {
        sensorOutputs[selectedField][clickedButton].type = TypesOfOutputs.getTypes()[type];
        sensorOutputs[selectedField][clickedButton].specification = TypesOfOutputs.getSpecificationsForType(TypesOfOutputs.getTypes()[type])[spec];
        sensorOutputs[selectedField][clickedButton].inputs = inputs;
        string typeString = TypesOfOutputs.getTypes()[type];
        string specString = TypesOfOutputs.getSpecificationsForType(typeString)[spec];
        buttons[clickedButton].GetComponentInChildren<TextMeshProUGUI>().text = TypesOfOutputs.getSymbolForType(typeString, specString);
    }
}
