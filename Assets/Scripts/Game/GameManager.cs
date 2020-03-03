using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenuPanel, gameplayOptionsPanel, matchmakingPanel, serverPanel;
    public TMP_Text gamemodeOptionText, mineAmountOptionText;
    public TMP_InputField usernameField, ipAddressField;
    public Toggle hostingToggle;

    public static int gamemodeInt = 0;
    public static string gamemode, ipAddress;

    public static int[] mineAmounts = { 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    public static int mineAmountInt = 0;

    public Menu[] menuPanels;
    public string[] panelNames = { "Main", "Matchmaking", "GameOptions", "Server" };
    public int mainInt = -1, matchInt = -1, gameInt = -1, serverInt = -1, menuPanelInt, menuPanelIntPrev;
    public bool singlePlay;

    public static GameManager instance;

    public enum Gamemodes
    {
        Default,
        Diagonal,
        Colour,
        EndOfEnum
    }

    public enum MenuPanel
    {
        Main = 0,
        Matchmaking = 1,
        GameOptions = 2,
        Server = 3
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
        GM();
        MA();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GamemodeOptionButton(int g)
    {
        gamemodeInt -= g;
    }

    public void GM()
    {
        gamemodeOptionText.text = ((Gamemodes)gamemodeInt).ToString();
        gamemode = ((Gamemodes)gamemodeInt).ToString();
        if (gamemodeInt <= -1)
        {
            gamemodeInt = (int)Gamemodes.EndOfEnum - 1;
        }
        if (gamemodeInt >= (int)Gamemodes.EndOfEnum)
        {
            gamemodeInt = 0;
        }
    }

    public void MineOptionButton(int m)
    {
        mineAmountInt -= m;
    }

    public void MA()
    {
        if (mineAmountInt <= -1)
        {
            mineAmountInt = mineAmounts.Length - 1;
        }
        if (mineAmountInt >= mineAmounts.Length)
        {
            mineAmountInt = 0;
        }
        mineAmountOptionText.text = mineAmounts[mineAmountInt].ToString();
    }

    public void ConnectToServer()
    {
        usernameField.interactable = false;
        Client.instance.ConnectToServer();
        SceneManager.LoadScene(0);
    }

    public void SinglePlay(int sP)
    {
        singlePlay = true;
        menuPanelInt -= sP;
    }

    public void Begin()
    {
        if (hostingToggle == true && singlePlay == false)
        {
            menuPanelInt = (int)MenuPanel.Server;
        }
        else if (singlePlay == true)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void MenuButton(int mB)
    {
        menuPanelInt -= mB;
        if (menuPanelInt >= menuPanels.Length)
        {
            menuPanelInt = menuPanels.Length - 1;
        }
        if (menuPanelInt == (int)MenuPanel.GameOptions)
        {
            if (singlePlay == false)
            {
                if (hostingToggle.isOn == true)
                {
                    menuPanelInt = (int)MenuPanel.GameOptions;
                }
                else if (hostingToggle.isOn == false)
                {
                    menuPanelInt = (int)MenuPanel.Server;
                }
            }
            if (singlePlay == true && hostingToggle == true)
            {
                menuPanelInt = (int)MenuPanel.GameOptions;
            }
        }
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
}

[System.Serializable]
public class Menu
{
    public string panelName;
    public GameObject panel;
}