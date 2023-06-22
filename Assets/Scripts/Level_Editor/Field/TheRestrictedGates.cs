using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TheRestrictedGates : MonoBehaviour
{
    private int[] restrictedGates;
    [SerializeField] private GameObject[] gates;
    [SerializeField] private GameObject prompt;

    public void setup(int[] restrictedGates)
    {
        if (restrictedGates == null)
            restrictedGates = new int[0];
        this.restrictedGates = restrictedGates;
        for (int i = 0; i < gates.Length; i++)
        {
            int temp = i;
            gates[i].GetComponent<Button>().onClick.AddListener(() => onClickGate(temp));
        }
        for (int i = 0; i < restrictedGates.Length; i++)
        {
            gates[restrictedGates[i]].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            gates[restrictedGates[i]].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
        }
    }
    public int[] getRestrictedGates()
    {
        return restrictedGates;
    }
    public void onClickGate(int number)
    {
        if (restrictedGates.Contains(number))
        {
            int[] temp = new int[restrictedGates.Length - 1];
            int index = 0;  
            for (int i = 0; i < restrictedGates.Length; i++)
            {
                if (restrictedGates[i] != number)
                {
                    temp[index] = restrictedGates[i];
                    index++;
                }
            }
            restrictedGates = temp;
            gates[number].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            gates[number].GetComponentInChildren<TextMeshProUGUI>().color = new Color32(50, 50, 50, 255);
        }
        else
        {
            int[] temp = new int[restrictedGates.Length + 1];
            for (int i = 0; i < restrictedGates.Length; i++)
            {
                temp[i] = restrictedGates[i];
            }
            temp[temp.Length - 1] = number;
            restrictedGates = temp;
            gates[number].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
            gates[number].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
        }
    }
    public void onClickPrompt()
    {
        prompt.SetActive(true);
    }
    public void onClickPromptClose()
    {
        prompt.SetActive(false);
    }
}
