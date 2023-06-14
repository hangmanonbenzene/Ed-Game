using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLevel : MonoBehaviour
{
    [SerializeField] private GameObject field;
    [SerializeField] private GameObject logic;

    void Start()
    {
        string levelName = GameObject.FindGameObjectWithTag("LevelName").GetComponent<LevelName>().getLevelName();
        LevelData levelData = SaveSystem.getLevel(levelName);

        field.GetComponent<LevelField>().setupField(levelData.field);
        logic.GetComponent<LevelLogic>().setupLogic(levelData.logicField);
    }
}
