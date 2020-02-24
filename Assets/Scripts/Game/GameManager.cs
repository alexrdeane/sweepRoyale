using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Button startGame;
    public Button exitGame;
    public GameObject mainMenuPanel;
    public GameObject gameplayOptionsPanel;
    public TMP_Text gamemodeOptionText;
    public TMP_Text mineAmountOptionText;
    public Button gamemodeOptionLeft;
    public Button gamemodeOptionRight;
    public Button beginGameButton;
    public Button backToMenuButton;

    public static int gamemodeInt = 0;
    public static string gamemode;

    public static int[] mineAmounts = { 10, 15, 20, 25, 30, 35, 40, 45, 50 };
    public static int mineAmountInt = 0;

    public enum Gamemodes
    {
        Default,
        Colour,
        EndOfEnum
    }

    void Start()
    {

    }

    void Update()
    {
        GamemodeOption();
        MineAmountOption();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GamemodeOptionLeft()
    {
        gamemodeInt--;
        if (gamemodeInt <= -1)
        {
            gamemodeInt = (int)Gamemodes.EndOfEnum - 1;
        }
        print(gamemodeInt);
    }

    public void GamemodeOptionRight()
    {
        gamemodeInt++;
        if (gamemodeInt >= (int)Gamemodes.EndOfEnum)
        {
            gamemodeInt = 0;
        }
        print(gamemodeInt);
    }

    public void GamemodeOption()
    {
        gamemodeOptionText.text = ((Gamemodes)gamemodeInt).ToString();
        gamemode = ((Gamemodes)gamemodeInt).ToString();
    }

    public void MineAmountOptionLeft()
    {
        mineAmountInt--;
        if (mineAmountInt <= -1)
        {
            mineAmountInt = mineAmounts.Length - 1;
        }
        print(mineAmountInt);
    }

    public void MineAmountOptionRight()
    {
        mineAmountInt++;
        if (mineAmountInt >= mineAmounts.Length)
        {
            mineAmountInt = 0;
        }
        print(mineAmountInt);
    }

    public void MineAmountOption()
    {
        mineAmountOptionText.text = mineAmounts[mineAmountInt].ToString();
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        gameplayOptionsPanel.SetActive(true);
    }

    public void Begin()
    {
        SceneManager.LoadScene(1);
    }

    public void BackToMenu()
    {
        mainMenuPanel.SetActive(true);
        gameplayOptionsPanel.SetActive(false);
    }
}
