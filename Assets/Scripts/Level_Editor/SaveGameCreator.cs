using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameCreator : MonoBehaviour
{
    [SerializeField] private GameObject levelEditor;

    [SerializeField] private GameObject fieldMenu;
    [SerializeField] private GameObject logicMenu;

    public LevelData createSaveGame(string levelName)
    {   
        Field[] field = findField();
        LogicField[] logicFields = findLogicFields();
        int[] restrictedGates = findRestrictedGates();
        return new LevelData(levelName, field, logicFields, restrictedGates);
    }

    private Field[] findField()
    {
        return fieldMenu.GetComponent<FieldMenu>().getField();
    }
    private LogicField[] findLogicFields()
    {
        return logicMenu.GetComponent<LogicMenu>().getLogicField();
    }
    private int[] findRestrictedGates()
    {
        return fieldMenu.GetComponent<FieldMenu>().getRestrictedGates();
    }
}
