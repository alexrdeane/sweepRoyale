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
    public GameObject matchmakingPanel;
    public GameObject serverPanel;
    public TMP_Text gamemodeOptionText;
    public TMP_Text mineAmountOptionText;
    public Button gamemodeOptionLeft;
    public Button gamemodeOptionRight;
    public Button beginGameButton;
    public Button backToMenuButton;
    public Button singleplayerButton;
    public Button versusButton;
    public Button connectButton;
    public TMP_InputField ipAddressField;
    public Toggle hostingToggle;

    public static int gamemodeInt = 0;
    public static string gamemode;

    public int matchmakingInt;

    public static int[] mineAmounts = { 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    public static int mineAmountInt = 0;

    public static string ipAddress;



    public static GameManager instance;
    public TMP_InputField usernameField;

    public enum Gamemodes
    {
        Default,
        Diagonal,
        Colour,
        EndOfEnum
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    void Update()
    {
        ipAddress = ipAddressField.text;
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
        serverPanel.SetActive(false);
        gameplayOptionsPanel.SetActive(false);
        matchmakingPanel.SetActive(true);
    }

    public void Singleplayer()
    {
        mainMenuPanel.SetActive(false);
        serverPanel.SetActive(false);
        gameplayOptionsPanel.SetActive(true);
        matchmakingPanel.SetActive(false);
        matchmakingInt = 0;
    }

    public void Versus()
    {
        if (hostingToggle.isOn == true)
        {
            mainMenuPanel.SetActive(false);
            gameplayOptionsPanel.SetActive(true);
            matchmakingPanel.SetActive(false);
            matchmakingInt = 1;
        }
        else if (hostingToggle.isOn == false)
        {
            mainMenuPanel.SetActive(false);
            serverPanel.SetActive(true);
            gameplayOptionsPanel.SetActive(false);
            matchmakingPanel.SetActive(false);
            matchmakingInt = 1;
        }
    }

    public void Begin()
    {
        if (hostingToggle.isOn == true)
        {
            mainMenuPanel.SetActive(false);
            serverPanel.SetActive(false);
            gameplayOptionsPanel.SetActive(false);
            matchmakingPanel.SetActive(false);
            serverPanel.SetActive(true);
        }
        else if (hostingToggle.isOn == false)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void BackToMenu()
    {
        mainMenuPanel.SetActive(true);
        gameplayOptionsPanel.SetActive(false);
        matchmakingPanel.SetActive(false);
    }

    public void ConnectToServer()
    {
        usernameField.interactable = false;
        Client.instance.ConnectToServer();
    }
}
