﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame
{
    public bool gameEnded = Grid.gameEndedBool;

    public static void GameRestart()
    {
        SceneManager.LoadScene(1);
        Grid.gameEndedBool = false;
    }
}
