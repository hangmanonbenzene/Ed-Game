using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicFields : MonoBehaviour
{
    [SerializeField] private GameObject[] fields;
    [SerializeField] private GameObject slider;

    private int[][] sliderValues;
    private int selectedField;
    private int[] sliderValue;

    public void setup(LogicField[] logicFields)
    {
        sliderValues = new int[5][];
        LogicField[] newlogicFields = new LogicField[5];
        int length = logicFields.Length;
        for (int i = 0; i < 5; i++)
        {
            if (i < length)
            {
                newlogicFields[i] = logicFields[i];
            }
            else
            {
                SensorInput[] sensorInputs = new SensorInput[1];
                string type = TypesOfInputs.getTypes()[0];
                string spec1 = TypesOfInputs.getSpecificationsForType(type, 1)[0];
                string spec2 = TypesOfInputs.getSpecificationsForType(type, 2)[0];
                string spec3 = TypesOfInputs.getSpecificationsForType(type, 3)[0];
                sensorInputs[0] = new SensorInput(type, spec1, spec2, spec3);
                LogicGate[] logicGates = new LogicGate[1];
                logicGates[0] = new LogicGate(0, 0, TypesOfLogic.getTypes()[0], new int[0]);
                SensorOutput[] sensorOutput = new SensorOutput[1];
                type = TypesOfOutputs.getTypes()[0];
                string spec = TypesOfOutputs.getSpecificationsForType(type)[0];
                sensorOutput[0] = new SensorOutput(type, spec, new int[0]);

                LogicField logicField = new LogicField(sensorInputs, logicGates, sensorOutput);
                newlogicFields[i] = logicField;
            }
            sliderValues[i] = new int[5];
            int[] logicLengths = new int[] { 0, 0, 0 };
            foreach (LogicGate logicGate in newlogicFields[i].logicGates)
            {
                logicLengths[logicGate.row]++;
            }
            sliderValues[i][0] = newlogicFields[i].sensorInputs.Length - 1;
            sliderValues[i][1] = logicLengths[0] - 1;
            sliderValues[i][2] = logicLengths[1] - 1;
            sliderValues[i][3] = logicLengths[2] - 1;
            sliderValues[i][4] = newlogicFields[i].sensorOutputs.Length - 1;
            for (int j = 0; j < 5; j++)
            {
                sliderValues[i][j] = Mathf.Clamp(sliderValues[i][j], 0, 4);
            }
        }
        setSliderValue(newlogicFields);
        setupField(newlogicFields);
    }

    public void setup(int number)
    {
        selectedField = number;
        goToField(number);

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

    private void setupField(LogicField[] logicFields)
    {
        SensorInput[][] sensorInputs = new SensorInput[5][];
        LogicGate[][] logicRowOne = new LogicGate[5][];
        LogicGate[][] logicRowTwo = new LogicGate[5][];
        LogicGate[][] logicRowThree = new LogicGate[5][];
        SensorOutput[][] sensorOutputs = new SensorOutput[5][];
        for (int i = 0; i < 5; i++)
        {
            sensorInputs[i] = logicFields[i].sensorInputs;
            sensorOutputs[i] = logicFields[i].sensorOutputs;
            int[] lenghts = new int[] { 0, 0, 0 };
            foreach (LogicGate logicGate in logicFields[i].logicGates)
            {
                lenghts[logicGate.row]++;
            }
            logicRowOne[i] = new LogicGate[lenghts[0]];
            logicRowTwo[i] = new LogicGate[lenghts[1]];
            logicRowThree[i] = new LogicGate[lenghts[2]];
            int[] index = new int[] { 0, 0, 0 };
            foreach (LogicGate logicGate in logicFields[i].logicGates)
            {
                switch (logicGate.row)
                {
                    case 0:
                        logicRowOne[i][index[0]] = logicGate;
                        index[0]++;
                        break;
                    case 1:
                        logicRowTwo[i][index[1]] = logicGate;
                        index[1]++;
                        break;
                    case 2:
                        logicRowThree[i][index[2]] = logicGate;
                        index[2]++;
                        break;
                }
            }
        }

        fields[0].GetComponent<FieldInput>().setup(sensorInputs);
        fields[1].GetComponent<FieldLogic>().setup(logicRowOne, 0);
        fields[2].GetComponent<FieldLogic>().setup(logicRowTwo, 1);
        fields[3].GetComponent<FieldLogic>().setup(logicRowThree, 2);
        fields[4].GetComponent<FieldOutput>().setup(sensorOutputs);
    }
    private void goToField(int number)
    {
        fields[0].GetComponent<FieldInput>().goToField(number, sliderValues[number][0]);
        fields[1].GetComponent<FieldLogic>().goToField(number, sliderValues[number][1]);
        fields[2].GetComponent<FieldLogic>().goToField(number, sliderValues[number][2]);
        fields[3].GetComponent<FieldLogic>().goToField(number, sliderValues[number][3]);
        fields[4].GetComponent<FieldOutput>().goToField(number, sliderValues[number][4]);
    }

    private void setSliderValue(LogicField[] logicFields)
    {
        sliderValue = new int[5];
        for (int i = 0; i < 5; i++)
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
    }

    public int[] getSliderValues(int number)
    {
        return sliderValues[number];
    }
    public int[] getSliderValue()
    {
        return sliderValue;
    }
    public LogicField[] getLogicFields()
    {
        LogicField[] newLogicFields = new LogicField[5];

        SensorInput[][] sensorInputs = fields[0].GetComponent<FieldInput>().getSensorInputs();
        SensorOutput[][] sensorOutputs = fields[4].GetComponent<FieldOutput>().getSensorOutputs();
        LogicGate[][] logicRowOne = fields[1].GetComponent<FieldLogic>().getLogicGates();
        LogicGate[][] logicRowTwo = fields[2].GetComponent<FieldLogic>().getLogicGates();
        LogicGate[][] logicRowThree = fields[3].GetComponent<FieldLogic>().getLogicGates();
        for (int i = 0; i < 5; i++)
        {
            SensorInput[] newSensorInput = sensorInputs[i];
            SensorOutput[] newSensorOutput = sensorOutputs[i];
            LogicGate[] newLogicGates = new LogicGate[15];
            for (int j = 0; j < 5; j++)
            {
                newLogicGates[j] = logicRowOne[i][j];
                newLogicGates[j + 5] = logicRowTwo[i][j];
                newLogicGates[j + 10] = logicRowThree[i][j];
            }
            newLogicFields[i] = new LogicField(newSensorInput, newLogicGates, newSensorOutput);
        }

        return newLogicFields;
    }
}
