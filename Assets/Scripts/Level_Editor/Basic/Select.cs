using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    [SerializeField] private GameObject field;
    [SerializeField] private GameObject logicField;

    public void onClickField()
    {
        field.SetActive(true);
        logicField.SetActive(false);
    }
    public void onClickLogic()
    {
        field.SetActive(false);
        logicField.SetActive(true);
    }
}
