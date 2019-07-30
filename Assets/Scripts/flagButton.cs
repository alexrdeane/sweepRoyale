using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagButton : MonoBehaviour
{
    //variables
    #region variables
    public static bool flagTile = false;
    public Sprite[] uiFlag;
    #endregion
    //sets the button to the active or not active sprites
    #region OnMouseUpAsButton
    void OnMouseUpAsButton()
    {
        if (flagTile == true)
        {
            GetComponent<SpriteRenderer>().sprite = uiFlag[1];
            flagTile = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = uiFlag[0];
            flagTile = true;
        }
    }
    #endregion
}
