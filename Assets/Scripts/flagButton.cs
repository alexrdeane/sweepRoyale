using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagButton : MonoBehaviour
{
    public static bool flagTile = false;
    void OnMouseUpAsButton()
    {
        if (flagTile == true)
        {
            flagTile = false;
        }
        else
        {
            flagTile = true;
        }
        Debug.Log(flagTile);
    }
}
