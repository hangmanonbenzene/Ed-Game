using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TypesOfLogic
{
    private static string[] types = new string[]
    {
        "empty",
        "not",
        "and",
        "or",
        "xor",
        "nand",
        "nor",
        "xnor"
    };
    private static string[] symbol = new string[]
    {
        "",
        "NOT",
        "AND",
        "OR",
        "XOR",
        "NAND",
        "NOR",
        "XNOR"
    };

    public static string getSymbolForType(string type)
    {
        int typeIndex = System.Array.IndexOf(types, type);
        return symbol[typeIndex];
    }
    public static string[] getTypes()
    {
        return types;
    }
}
