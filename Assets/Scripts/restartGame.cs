using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    //variables
    #region variables
    //emoji with glasses
    public Sprite[] gameEmoji;
    //bool if game is still going
    public bool gameEnded = Grid.gameEndedBool;
    #endregion
    //gameEndedBool changes sprite based on if the game is still going
    #region update
    void Update()
    {
        if (Grid.gameEndedBool == true)
        {
            GetComponent<SpriteRenderer>().sprite = gameEmoji[1];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = gameEmoji[0];
        }
    }
    #endregion
    //if pressed will reload the game scene
    #region OnMouseUpAsButton
    void OnMouseUpAsButton()
    {
        SceneManager.LoadScene(0);
        Grid.gameEndedBool = false;
    }
    #endregion
}
