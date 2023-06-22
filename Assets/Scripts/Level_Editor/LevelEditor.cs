using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private string levelName;

    [SerializeField] private GameObject fieldMenu;
    [SerializeField] private GameObject logicMenu;

    [SerializeField] private GameObject select;

    void Start()
    {
        levelName = findLevelName();
        LevelData levelData;
        if (SaveSystem.exists(levelName))
        {
            levelData = SaveSystem.getLevel(levelName, false);
        }
        else
        {
            levelData = emptyLevel();
        }

        fieldMenu.GetComponent<FieldMenu>().setupField(levelData.field, levelData.restrictedGates);
        logicMenu.GetComponent<LogicMenu>().setupLogic(levelData.logicField);

        select.GetComponent<Select>().onClickField();
    }

    public string getLevelName()
    {
        return levelName;
    }

    private string findLevelName()
    {
        if (LevelName.getLevelName().Equals("") || LevelName.getLevelName().Equals("Neues Level"))
        {
            return "";
        }
        else
        {
            return LevelName.getLevelName();
        }
    }
    private LevelData emptyLevel()
    {
        // Create a new emty field with one tile
        Field[] field = new Field[1];
        field[0] = new Field(0, 0, "empty", "");

        // Create the logic field
        LogicField[] logicfields = new LogicField[1];
        logicfields[0] = emptyLogicField();

        return new LevelData("", field, logicfields, new int[0]);
    }
    private LogicField emptyLogicField()
    {
        SensorInput[] sensorInputs = new SensorInput[1];
        string type = TypesOfInputs.getTypes()[0];
        string spec1 = TypesOfInputs.getSpecificationsForType(type, 1)[0];
        string spec2 = TypesOfInputs.getSpecificationsForType(type, 2)[0];
        string spec3 = TypesOfInputs.getSpecificationsForType(type, 3)[0];
        sensorInputs[0] = new SensorInput(type, spec1, spec2, spec3);

        LogicGate[] logicGates = new LogicGate[1];
        logicGates[0] = new LogicGate(0, 0, TypesOfLogic.getTypes()[0], new int[0]);

        SensorOutput[] sensorOutputs = new SensorOutput[1];
        type = TypesOfOutputs.getTypes()[0];
        string spec = TypesOfOutputs.getSpecificationsForType(type)[0];
        sensorOutputs[0] = new SensorOutput(type, spec, new int[0]);

        return new LogicField(sensorInputs, logicGates, sensorOutputs);
    }
}
