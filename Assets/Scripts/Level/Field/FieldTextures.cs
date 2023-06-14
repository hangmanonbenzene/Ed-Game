using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTextures : MonoBehaviour
{
    [SerializeField] private GameObject[] empty;
    [SerializeField] private GameObject[] player;
    [SerializeField] private GameObject[] goal;
    [SerializeField] private GameObject[] wall;

    public GameObject getTextureForType(string type, int spec)
    {
        switch (type)
        {
            case "empty":
                return empty[spec];
            case "player":
                return player[spec];
            case "goal":
                return goal[spec];
            case "wall":
                return wall[spec];
            default:
                return null;
        }
    }
}
