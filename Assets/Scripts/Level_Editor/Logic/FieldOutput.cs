using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldOutput : MonoBehaviour
{
    private SensorOutput[] sensorOutputs;

    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject slider;

    [SerializeField] private GameObject logicFields;

    public void setup(SensorOutput[] sensorOutputs, int sliderValue)
    {
        this.sensorOutputs = new SensorOutput[5];
        for (int i = 0; i < 5; i++)
        {
            if (i < sensorOutputs.Length)
            {
                this.sensorOutputs[i] = sensorOutputs[i];
            }
            else
            {
                this.sensorOutputs[i] = new SensorOutput("empty", "", new int[0]);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            int number = i;
            buttons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            buttons[i].GetComponent<Button>().onClick.AddListener(() => onClickButton(number));
            string type = this.sensorOutputs[i].type;
            string spec = this.sensorOutputs[i].specification;
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = TypesOfOutputs.getSymbolForType(type, spec);
            if (slider.GetComponent<Slider>().value == sliderValue)
            {
                onChangeValue();
            }
            else
            {
                slider.GetComponent<Slider>().value = sliderValue;
            }
        }
    }

    private void onClickButton(int number)
    {
        Debug.Log(number);
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
}
