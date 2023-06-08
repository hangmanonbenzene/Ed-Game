using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LogicFields : MonoBehaviour
{
    [SerializeField] private GameObject[] fields;
    [SerializeField] private GameObject slider;

    private LogicField[] logicFields;
    private int[][] sliderValues;
    private int selectedField;
    private int[] sliderValue;

    public void setup(LogicField[] logicFields)
    {
        sliderValues = new int[5][];
        this.logicFields = new LogicField[5];
        int length = logicFields.Length;
        for (int i = 0; i < 5; i++)
        {
            if (i < length)
            {
                this.logicFields[i] = logicFields[i];
            }
            else
            {
                SensorInput[] sensorInputs = new SensorInput[1];
                sensorInputs[0] = new SensorInput("empty", "", "", "");
                LogicGate[] logicGates = new LogicGate[1];
                logicGates[0] = new LogicGate(Mathf.Clamp(i - 1, 0, 2), 0, "empty", new int[0]);
                SensorOutput[] sensorOutput = new SensorOutput[1];
                sensorOutput[0] = new SensorOutput("empty", "", new int[0]);

                LogicField logicField = new LogicField(sensorInputs, logicGates, sensorOutput);
                this.logicFields[i] = logicField;
            }
            sliderValues[i] = new int[5];
            sliderValues[i][0] = this.logicFields[i].sensorInputs.Length - 1;
            int[] logicLengths = new int[3];
            foreach (LogicGate logicGate in this.logicFields[i].logicGates)
            {
                logicLengths[logicGate.row]++;
            }
            sliderValues[i][1] = logicLengths[0] - 1;
            sliderValues[i][2] = logicLengths[1] - 1;
            sliderValues[i][3] = logicLengths[2] - 1;
            sliderValues[i][4] = this.logicFields[i].sensorOutputs.Length - 1;
            for (int j = 0; j < 5; j++)
            {
                sliderValues[i][j] = Mathf.Clamp(sliderValues[i][j], 0, 4);
            }
        }
        selectedField = 0;
        setSliderValue(logicFields);
    }

    public void setup(int number)
    {
        selectedField = number;
        setupField(logicFields[number]);

        if (slider.GetComponent<Slider>().value == sliderValue[number])
        {
            onChangeValue();
        }
        else
        {
            slider.GetComponent<Slider>().value = sliderValue[number];
        }
    }

    public void onChangeValue()
    {
        int value = (int)slider.GetComponent<Slider>().value;

        for (int i = 1; i < fields.Length - 1; i++)
        {
            if (i <= value)
            {
                fields[i].SetActive(true);
            }
            else
            {
                fields[i].SetActive(false);
            }
        }
        sliderValue[selectedField] = value;
    }
    public void sliderChange(int number, int value)
    {
        sliderValues[selectedField][number] = value;
    }

    private void setupField(LogicField logicField)
    {
        fields[0].GetComponent<FieldInput>().setup(logicField.sensorInputs, sliderValues[selectedField][0]);
        fields[1].GetComponent<FieldLogic>().setup(logicField.logicGates, 0, sliderValues[selectedField][1]);
        fields[2].GetComponent<FieldLogic>().setup(logicField.logicGates, 1, sliderValues[selectedField][2]);
        fields[3].GetComponent<FieldLogic>().setup(logicField.logicGates, 2, sliderValues[selectedField][3]);
        fields[4].GetComponent<FieldOutput>().setup(logicField.sensorOutputs, sliderValues[selectedField][4]);
    }

    private void setSliderValue(LogicField[] logicFields)
    {
        sliderValue = new int[5];
        for (int i = 0; i < 5; i++)
        {
            if (i < logicFields.Length)
            {
                int length = 0;
                foreach (LogicGate logicGate in logicFields[i].logicGates)
                {
                    if (logicGate.row + 1 > length)
                    {
                        length = logicGate.row + 1;
                    }
                }
                sliderValue[i] = length;
            }
            else
            {
                sliderValue[i] = 1;
            }
        }
    }

    public int[] getSliderValues(int number)
    {
        return sliderValues[number];
    }
    public int[] getSliderValue()
    {
        return sliderValue;
    }
}
