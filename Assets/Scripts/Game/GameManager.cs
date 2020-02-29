using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public GameObject mainMenuPanel;
    [HideInInspector] public GameObject gameplayOptionsPanel;
    [HideInInspector] public GameObject matchmakingPanel;
    [HideInInspector] public GameObject serverPanel;
    [HideInInspector] public TMP_Text gamemodeOptionText;
    [HideInInspector] public TMP_Text mineAmountOptionText;
    [HideInInspector] public TMP_InputField ipAddressField;
    [HideInInspector] public Toggle hostingToggle;

    [HideInInspector] public static int gamemodeInt = 0;
    [HideInInspector] public static string gamemode;

    [HideInInspector] public static int[] mineAmounts = { 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    [HideInInspector] public static int mineAmountInt = 0;

    [HideInInspector] public static string ipAddress;

    [HideInInspector] public Menu[] menuPanels;
    [HideInInspector] public string[] panelNames = { "Main", "Matchmaking", "GameOptions", "Server" };
    [HideInInspector] public int mainInt = -1, matchInt = -1, gameInt = -1, serverInt = -1;
    [HideInInspector] public bool singlePlay;

    [HideInInspector] private int menuPanelInt, menuPanelIntPrev;

    [HideInInspector] public static GameManager instance;
    [HideInInspector] public TMP_InputField usernameField;

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
        if (menuPanelInt != menuPanelIntPrev)
        {
            CurrentMenu();
            menuPanelIntPrev = menuPanelInt;
        }
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
    }

    public void GamemodeOptionRight()
    {
        gamemodeInt++;
        if (gamemodeInt >= (int)Gamemodes.EndOfEnum)
        {
            gamemodeInt = 0;
        }
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
    }

    public void MineAmountOptionRight()
    {
        mineAmountInt++;
        if (mineAmountInt >= mineAmounts.Length)
        {
            mineAmountInt = 0;
        }
    }

    public void MineAmountOption()
    {
        mineAmountOptionText.text = mineAmounts[mineAmountInt].ToString();
    }

    public void ConnectToServer()
    {
        usernameField.interactable = false;
        Client.instance.ConnectToServer();
        SceneManager.LoadScene(0);
    }

    public void SinglePlay()
    {
        singlePlay = true;
        NextButton();
    }

    public void Begin()
    {
        if (hostingToggle == true && singlePlay == false)
        {
            menuPanelInt = serverInt;
        }
        else if (singlePlay == true)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void NextButton()
    {
        MenuToInt();
        menuPanelInt++;
        if (menuPanelInt >= menuPanels.Length)
        {
            menuPanelInt = menuPanels.Length - 1;
        }
        if (menuPanelInt == gameInt)
        {
            if (singlePlay == false)
            {
                if (hostingToggle.isOn == true)
                {
                    menuPanelInt = gameInt;
                }
                else if (hostingToggle.isOn == false)
                {
                    menuPanelInt = serverInt;
                }
            }
        }
    }

    public void PreviousButton()
    {
        menuPanelInt--;
        if (menuPanelInt <= -1)
        {
            menuPanelInt = 0;
        }
    }

    public void CurrentMenu()
    {
        foreach (Menu menuPanel in menuPanels)
        {
            menuPanel.panel.SetActive(false);
        }
        menuPanels[menuPanelInt].panel.SetActive(true);
    }

    public void MenuToInt()
    {
        for (int i = 0; i < panelNames.Length; i++)
        {
            if (panelNames[i] == "Menu")
            {
                mainInt = i;
            }
        }
        for (int i = 0; i < panelNames.Length; i++)
        {
            if (panelNames[i] == "Matchmaking")
            {
                matchInt = i;
            }
        }
        for (int i = 0; i < panelNames.Length; i++)
        {
            if (panelNames[i] == "GameOptions")
            {
                gameInt = i;
            }
        }
        for (int i = 0; i < panelNames.Length; i++)
        {
            if (panelNames[i] == "Server")
            {
                serverInt = i;
            }
        }
    }
}

[System.Serializable]
public class Menu
{
    public string panelName;
    public GameObject panel;
}