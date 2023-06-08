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
        LevelData levelData = new LevelData("", new Field[0], new LogicField[0]);
        if (SaveSystem.exists(levelName))
        {
            levelData = SaveSystem.getLevel(levelName);
        }
        else
        {
            levelData = emptyLevel();
        }

        fieldMenu.GetComponent<FieldMenu>().setupField(levelData.field);
        logicMenu.GetComponent<LogicMenu>().setupLogic(levelData.logicField);

        select.GetComponent<Select>().onClickField();
    }

    public string getLevelName()
    {
        return levelName;
    }

    private string findLevelName()
    {
        GameObject levelName = GameObject.FindGameObjectWithTag("LevelName");
        if (levelName == null || levelName.GetComponent<LevelName>().getLevelName().Equals("Neues Level"))
        {
            return "";
        }
        else
        {
            return levelName.GetComponent<LevelName>().getLevelName();
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

        return new LevelData("", field, logicfields);
    }
    private LogicField emptyLogicField()
    {
        SensorInput[] sensorInputs = new SensorInput[1];
        sensorInputs[0] = new SensorInput("empty", "", "", "");

        LogicGate[] logicGates = new LogicGate[1];
        logicGates[0] = new LogicGate(0, 0, "empty", new int[0]);

        SensorOutput[] sensorOutputs = new SensorOutput[1];
        sensorOutputs[0] = new SensorOutput("empty", "", new int[0]);

        return new LogicField(sensorInputs, logicGates, sensorOutputs);
    }
}
