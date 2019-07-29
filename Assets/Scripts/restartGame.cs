using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restartGame : MonoBehaviour
{
    public Sprite gameActive;
    public Sprite gameLost;
    public bool gameEnded = Playfield.gameEndedBool;
    void Update()
    {
        if (Playfield.gameEndedBool == true)
        {
            GetComponent<SpriteRenderer>().sprite = gameLost;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = gameActive;
        }
    }

    void OnMouseUpAsButton()
    {
        SceneManager.LoadScene(0);
        Playfield.gameEndedBool = false;
    }
}
