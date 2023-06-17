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
        setInputText(input.type, input.specificationOne, input.specificationTwo, input.specificationThree);
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
        titel.GetComponent<TMPro.TextMeshProUGUI>().text = "Logik-Gatter";
        setLogicText(logic.type);
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
        setOutputText(output.type, output.specification);
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
        setLogicText(logic.type);
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

    private void setInputText(string type, string spec1, string spec2, string spec3)
    {
        switch (type)
        {
            case "false":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Ist immer falsch.";
                break;
            case "true":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Ist immer wahr.";
                break;
            case "look":
                string[] specs = getInputSpecText(type, spec1, spec2, spec3);
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Ist wahr, wenn der Spieler " + specs[0] + " sich " + specs[1] + " in einer Entfernung von höchstens " + specs[2] + " sieht.";
                break;
        }
    }
    private void setLogicText(string type)
    {
        switch (logic.type)
        {
            case "buffer":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt den Wert des Inputs zurück.";
                break;
            case "and":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt wahr zurück, wenn alle Inputs wahr sind.";
                break;
            case "or":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt wahr zurück, wenn mindestens ein Input wahr ist.";
                break;
            case "xor":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt wahr zurück, wenn genau ein Input wahr ist.";
                break;
            case "not":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt den umgekehrten Wert des Inputs zurück.";
                break;
            case "nand":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt wahr zurück, wenn mindestens ein Input falsch ist.";
                break;
            case "nor":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt wahr zurück, wenn alle Inputs falsch sind.";
                break;
            case "xnor":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt wahr zurück, wenn alle Inputs gleich sind.";
                break;
            default:
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Wähle ein Logik-Gatter aus.";
                break;
        }
    }
    private void setOutputText(string type, string spec)
    {
        switch (type)
        {
            case "wait":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Der Spieler macht nichts.";
                break;
            case "walk":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Der Spieler läuft nach vorne.";
                break;
            case "turn":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Der Spieler dreht sich nach " + (spec == "left" ? "links" : "rechts") + ".";
                break;
        }
    }

    private string[] getInputSpecText(string type, string spec1, string spec2, string spec3)
    {
        string[] text = new string[3];
        switch (type)
        {
            case "look":
                text[0] = spec1 switch
                {
                    "forwards" => "vor",
                    "left" => "links neben",
                    "backwards" => "hinter",
                    "right" => "rechts neben",
                    _ => "-",
                };
                text[1] = spec2 switch
                {
                    "empty" => "ein leeres Feld",
                    "wall" => "eine Wand",
                    "player" => "einen Spieler",
                    "goal" => "ein Ziel",
                    _ => "-",
                };
                text[2] = spec3 switch
                {
                    "one" => "einem Feld",
                    "two" => "zwei Feldern",
                    "three" => "drei Feldern",
                    "four" => "vier Feldern",
                    _ => "-",
                };
                break;
        }
        return text;
    }
}
