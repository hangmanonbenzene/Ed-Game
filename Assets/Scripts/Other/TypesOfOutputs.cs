using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TypesOfOutputs
{
    private static string[] types = new string[]
    {
        "wait",
        "walk",
        "turn",
        "jump",
        "write"
    };
    private static string[][] specifications = new string[][]
    {
        new string[] { "" },
        new string[] { "" },
        new string[] { "left", "right" },
        new string[] { "" },
        new string[] { "A", "B", "C", "D"}
    };

    public static string[][] symbols = new string[][]
    {
        new string[] { "-" },
        new string[] { "/\\" },
        new string[] { "<", ">" },
        new string[] { "o" },
        new string[] { "A", "B", "C", "D" }
    };

    public static string getSymbolForType(string type, string specification)
    {
        if (specification.Equals("letter"))
            return type[..1].ToUpper();
        int typeIndex = System.Array.IndexOf(types, type);
        int specificationIndex = System.Array.IndexOf(specifications[typeIndex], specification);
        return symbols[typeIndex][specificationIndex];
    }
    public static string getSymbolForType(string type, int number)
    {
        int typeIndex = System.Array.IndexOf(types, type);
        return symbols[typeIndex][number];
    }
    public static string[] getSpecificationsForType(string type)
    {
        int typeIndex = System.Array.IndexOf(types, type);
        return specifications[typeIndex];
    }
    public static string[] getTypes()
    {
        return types;
    }
}
