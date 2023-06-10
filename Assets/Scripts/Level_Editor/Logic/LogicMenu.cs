using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicMenu : MonoBehaviour
{
    [SerializeField] private GameObject components;

    public void setupLogic(LogicField[] logicFields)
    {
        components.GetComponent<ChoosePrompt>().setup();
        components.GetComponent<LogicFields>().setup(logicFields);
        components.GetComponent<ChooseGate>().setup(logicFields);
    }

    public LogicField[] getLogicField()
    {
        LogicField[] logicFields = new LogicField[components.GetComponent<ChooseGate>().getSliderValue() + 1];
        LogicField[] currentLogicFields = components.GetComponent<LogicFields>().getLogicFields();
        int[] value = components.GetComponent<LogicFields>().getSliderValue();
        for (int i = 0; i < logicFields.Length; i++)
        {
            int[] values = components.GetComponent<LogicFields>().getSliderValues(i);

            SensorInput[] sensorInputs = new SensorInput[values[0] + 1];
            for (int j = 0; j < sensorInputs.Length; j++)
            {
                sensorInputs[j] = currentLogicFields[i].sensorInputs[j];
            }

            SensorOutput[] sensorOutputs = new SensorOutput[values[4] + 1];
            for (int j = 0; j < sensorOutputs.Length; j++)
            {
                sensorOutputs[j] = currentLogicFields[i].sensorOutputs[j];
            }

            int length = 0;
            for (int j = 0; j < value[i]; j++)
            {
                length += values[j + 1] + 1;
            }
            LogicGate[] logicGates = new LogicGate[length];
            int index = 0;
            foreach (LogicGate logicGate in currentLogicFields[i].logicGates)
            {
                if (logicGate.position <= values[logicGate.row + 1] && logicGate.row < value[i])
                {
                    logicGates[index] = logicGate;
                    index++;
                }
            }

            logicFields[i] = new LogicField(sensorInputs, logicGates, sensorOutputs);
        }

        return logicFields;
    }
}
