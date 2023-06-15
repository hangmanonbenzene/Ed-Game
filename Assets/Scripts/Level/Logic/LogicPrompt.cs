using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LogicPrompt : MonoBehaviour
{
    [SerializeField] private GameObject levelLogic;
    [SerializeField] private GameObject lines;

    [SerializeField] private GameObject prompt;
    [SerializeField] private GameObject titel;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject[] buttons;

    private string type;
    private LogicGate logic;
    private int selectedButton;
    private int[] activeButtons;

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int button = i;
            buttons[i].GetComponent<Button>().onClick.AddListener(() => selectButton(button));
        }
    }
    public void setInput(SensorInput input)
    {
        type = "input";
        titel.GetComponent<TMPro.TextMeshProUGUI>().text = "Input";
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
        }
        activatePrompt();
    }
    public void setLogic(LogicGate logic, bool multipleInputs)
    {
        type = "logic";
        this.logic = logic;
        titel.GetComponent<TMPro.TextMeshProUGUI>().text = "Logik";
        activeButtons = multipleInputs ? new int[] { 0, 1, 2, 3, 5, 6, 7 } : new int[] { 0, 4 };
        buttons[0].GetComponent<Button>().interactable = !multipleInputs;
        buttons[0].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
        buttons[1].GetComponent<Button>().interactable = multipleInputs;
        buttons[1].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = multipleInputs ? "AND" : "";
        buttons[2].GetComponent<Button>().interactable = multipleInputs;
        buttons[2].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = multipleInputs ? "OR" : "";
        buttons[3].GetComponent<Button>().interactable = multipleInputs;
        buttons[3].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = multipleInputs ? "XOR" : "";
        buttons[4].GetComponent<Button>().interactable = !multipleInputs;
        buttons[4].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = multipleInputs ? "" : "NOT";
        buttons[5].GetComponent<Button>().interactable = multipleInputs;
        buttons[5].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = multipleInputs ? "NAND" : "";
        buttons[6].GetComponent<Button>().interactable = multipleInputs;
        buttons[6].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = multipleInputs ? "NOR" : "";
        buttons[7].GetComponent<Button>().interactable = multipleInputs;
        buttons[7].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = multipleInputs ? "XNOR" : "";

        switch (logic.type)
        {
            case "buffer":
                selectButton(0);
                break;
            case "and":
                selectButton(1);
                break;
            case "or":
                selectButton(2);
                break;
            case "xor":
                selectButton(3);
                break;
            case "not":
                selectButton(4);
                break;
            case "nand":
                selectButton(5);
                break;
            case "nor":
                selectButton(6);
                break;
            case "xnor":
                selectButton(7);
                break;
            default:
                selectButton(-1);
                break;
        }

        activatePrompt();
    }
    public void setOutput(SensorOutput output)
    {
        type = "output";
        titel.GetComponent<TMPro.TextMeshProUGUI>().text = "Output";
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
        }
        activatePrompt();
    }

    private void selectButton(int button)
    {
        selectedButton = button;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == button)
            {
                buttons[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                buttons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().color = new Color(1f, 1f, 1f);
            }
            else if (activeButtons.Contains(i))
            {
                buttons[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);
                buttons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().color = new Color32(50, 50, 50, 255);
            }
        }
        logic.type = TypesOfLogic.getTypes()[button + 1];
    }
    private void activatePrompt()
    {
        lines.SetActive(false);
        prompt.SetActive(true);
    }
    public void onCLickDone()
    {         
        if (type == "logic")
        {
            levelLogic.GetComponent<LevelLogic>().setLogic(logic);
        }
        prompt.SetActive(false);
        lines.SetActive(true);
    }
}
