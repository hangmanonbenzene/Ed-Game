using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldInput : MonoBehaviour
{
    private SensorInput[] sensorInputs;

    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject slider;

    [SerializeField] private GameObject logicFields;

    public void setup(SensorInput[] sensorInputs, int sliderValue)
    {
        this.sensorInputs = new SensorInput[5];
        for (int i = 0; i < 5; i++)
        {
            if (i < sensorInputs.Length)
            {
                this.sensorInputs[i] = sensorInputs[i];
            }
            else
            {
                this.sensorInputs[i] = new SensorInput("empty", "", "", "");
            }
        }
        for (int i = 0; i < 5; i++)
        {
            int number = i;
            buttons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            buttons[i].GetComponent<Button>().onClick.AddListener(() => onClickButton(number));
            string type = this.sensorInputs[i].type;
            string spec1 = this.sensorInputs[i].specificationOne;
            string spec2 = this.sensorInputs[i].specificationTwo;
            string spec3 = this.sensorInputs[i].specificationThree;
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
        Debug.Log(number);
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
}
