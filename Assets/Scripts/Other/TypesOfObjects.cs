using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TypesOfObjects
{
    private static string[] types = new string[]
    {
        "empty",
        "player",
        "goal",
        "wall",
        "hole",
        "switch"
    };
    private static string[][] specifications = new string[][]
    {
        new string[]{ "", "toWall" },
        new string[]{ "up", "right", "down", "left" },
        new string[]{ "" },
        new string[]{ "", "fake", "toEmpty" },
        new string[]{ "" },
        new string[]{ "" }
    };
    public static string[][] symbols = new string[][]
    {
        new string[]{ "", "eX" },
        new string[]{ "/\\", ">", "\\/", "<" },
        new string[]{ "$" },
        new string[]{ "X", "F", "xE" },
        new string[]{ "O" },
        new string[]{ "_" }
    };

    public static string getSymbolForType(string type, string specification)
    {
        if (specification.Equals("letter"))
            return type[..1].ToUpper();
        int typeIndex = System.Array.IndexOf(types, type);
        int specificationIndex = System.Array.IndexOf(specifications[typeIndex], specification);
        return symbols[typeIndex][specificationIndex];
    }
    public static string[] getSpecificationsForType(string type)
    {
        int typeIndex = System.Array.IndexOf(types, type);
        if (typeIndex < 0 || typeIndex >= types.Length)
            return new string[] { "" };
        return specifications[typeIndex];
    }
    public static string[] getTypes()
    {
        return types;
    }   
}
