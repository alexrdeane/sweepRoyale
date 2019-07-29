using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagButton : MonoBehaviour
{
    public static bool flagTile = false;
    public Sprite uiFlagActive;
    public Sprite uiFlagInactive;
    void OnMouseUpAsButton()
    {
        if (flagTile == true)
        {
            GetComponent<SpriteRenderer>().sprite = uiFlagInactive;
            flagTile = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = uiFlagActive;
            flagTile = true;
        }
        Debug.Log(flagTile);
    }
}
