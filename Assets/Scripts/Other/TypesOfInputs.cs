using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TypesOfInputs
{
    private static string[] types = new string[]
    {
        "false",
        "true",
        "look",
        "read"
    };
    private static string[][] specificationsOne = new string[][]
    {
        new string[] { "" },
        new string[] { "" },
        new string[] { "forwards", "right", "backwards", "left" },
        new string[] { "A", "B", "C", "D"}
    };
    private static string[][] specificationsTwo = new string[][]
    {
        new string[] { "" },
        new string[] { "" },
        new string[] { "empty", "player", "goal", "wall", "hole", "switch", "mummy" },
        new string[] { "" }
    };
    private static string[][] specificationsThree = new string[][]
    {
        new string[] { "" },
        new string[] { "" },
        new string[] { "one", "two", "three", "four" },
        new string[] { "" }
    };
    private static string[][] symbolOne = new string[][]
    {   
        new string[] { "0" },
        new string[] { "1" },
        new string[] { "/\\", ">", "\\/", "<" },
        new string[] { "A", "B", "C", "D" }
    };
    private static string[][] symbolTwo = new string[][]
    {
        new string[] { "" },
        new string[] { "" },
        new string[] { "F", "S", "Z", "W", "L", "D", "M" },
        new string[] { "" }
    };
    private static string[][] symbolThree = new string[][]
    {
        new string[] { "" },
        new string[] { "" },
        new string[] { "1", "2", "3", "4" },
        new string[] { "" }
    };

    public static string getSymbolForType(string type, string specificationOne, string specificationTwo, string specificationThree)
    {
        if (specificationOne.Equals("letter"))
            return type[..1].ToUpper();
        int typeIndex = System.Array.IndexOf(types, type);
        int specificationIndexOne = System.Array.IndexOf(specificationsOne[typeIndex], specificationOne);
        int specificationIndexTwo = System.Array.IndexOf(specificationsTwo[typeIndex], specificationTwo);
        int specificationIndexThree = System.Array.IndexOf(specificationsThree[typeIndex], specificationThree);
        return symbolOne[typeIndex][specificationIndexOne] + 
               symbolThree[typeIndex][specificationIndexThree] +
               symbolTwo[typeIndex][specificationIndexTwo];
    }
    public static string getSymbolForType(string type, int specification, int number)
    {
        int typeIndex = System.Array.IndexOf(types, type);
        switch (specification)
        {
            case 1:
                return symbolOne[typeIndex][number];
            case 2:
                return symbolTwo[typeIndex][number];
            case 3:
                return symbolThree[typeIndex][number];
            default:
                return null;
        }
    }
    public static string[] getSpecificationsForType(string type, int specification)
    {
        int typeIndex = System.Array.IndexOf(types, type);
        switch (specification)
        {
            case 1:
                return specificationsOne[typeIndex];
            case 2:
                return specificationsTwo[typeIndex];
            case 3:
                return specificationsThree[typeIndex];
            default:
                return null;
        }
    }
    public static string[] getTypes()
    {
        return types;
    }
}
