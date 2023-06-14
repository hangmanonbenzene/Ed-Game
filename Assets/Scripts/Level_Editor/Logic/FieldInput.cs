using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldInput : MonoBehaviour
{
    private SensorInput[][] sensorInputs;
    private int selectedField;

    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject slider;

    [SerializeField] private GameObject logicFields;
    private int clickedButton;

    public void setup(SensorInput[][] sensorInputs)
    {
        this.sensorInputs = new SensorInput[5][];
        for (int i = 0; i < 5; i++)
        {
            this.sensorInputs[i] = new SensorInput[5];
            for (int j = 0; j < 5; j++)
            {
                if (j < sensorInputs[i].Length)
                {
                    this.sensorInputs[i][j] = sensorInputs[i][j];
                }
                else
                {
                    string type = TypesOfInputs.getTypes()[0];
                    string spec1 = TypesOfInputs.getSpecificationsForType(type, 1)[0];
                    string spec2 = TypesOfInputs.getSpecificationsForType(type, 2)[0];
                    string spec3 = TypesOfInputs.getSpecificationsForType(type, 3)[0];
                    this.sensorInputs[i][j] = new SensorInput(type, spec1, spec2, spec3);
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
            string type = this.sensorInputs[number][i].type;
            string spec1 = this.sensorInputs[number][i].specificationOne;
            string spec2 = this.sensorInputs[number][i].specificationTwo;
            string spec3 = this.sensorInputs[number][i].specificationThree;
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = TypesOfInputs.getSymbolForType(type, spec1, spec2, spec3);
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
        string type = sensorInputs[selectedField][number].type;
        int typenumber = System.Array.IndexOf(TypesOfInputs.getTypes(), type);
        int spec1 = System.Array.IndexOf(TypesOfInputs.getSpecificationsForType(type, 1), sensorInputs[selectedField][number].specificationOne);
        int spec2 = System.Array.IndexOf(TypesOfInputs.getSpecificationsForType(type, 2), sensorInputs[selectedField][number].specificationTwo);
        int spec3 = System.Array.IndexOf(TypesOfInputs.getSpecificationsForType(type, 3), sensorInputs[selectedField][number].specificationThree);
        logicFields.GetComponent<ChoosePrompt>().activateInput(this.gameObject, typenumber, spec1, spec2, spec3);
    }
    public void onChangeValue()
    {
        int value = (int)slider.GetComponent<Slider>().value;
        logicFields.GetComponent<LogicFields>().sliderChange(0, value);
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
    
    public SensorInput[][] getSensorInputs()
    {
        return sensorInputs;
    }

    public void onButtonChange(int type, int spec1, int spec2, int spec3)
    {
        sensorInputs[selectedField][clickedButton].type = TypesOfInputs.getTypes()[type];
        sensorInputs[selectedField][clickedButton].specificationOne = TypesOfInputs.getSpecificationsForType(TypesOfInputs.getTypes()[type], 1)[spec1];
        sensorInputs[selectedField][clickedButton].specificationTwo = TypesOfInputs.getSpecificationsForType(TypesOfInputs.getTypes()[type], 2)[spec2];
        sensorInputs[selectedField][clickedButton].specificationThree = TypesOfInputs.getSpecificationsForType(TypesOfInputs.getTypes()[type], 3)[spec3];
        string typeString = sensorInputs[selectedField][clickedButton].type;
        string spec1String = sensorInputs[selectedField][clickedButton].specificationOne;
        string spec2String = sensorInputs[selectedField][clickedButton].specificationTwo;
        string spec3String = sensorInputs[selectedField][clickedButton].specificationThree;
        buttons[clickedButton].GetComponentInChildren<TextMeshProUGUI>().text = TypesOfInputs.getSymbolForType(typeString, spec1String, spec2String, spec3String);
    }
}
