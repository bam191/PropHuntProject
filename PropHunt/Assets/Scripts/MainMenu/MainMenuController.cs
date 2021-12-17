using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public enum eMainMenuState
    {
        MainMenu,
        ConnectionPopup,
        SettingsPopup,
        None
    }

    private static MainMenuController _instance;
    public static MainMenuController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MainMenuController>(true);
            }

            return _instance;
        }
    }

    [SerializeField] private Button _serverBrowserButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    private eMainMenuState _currentState = eMainMenuState.None;

    private Popup _connectionPopup;
    private Popup _settingsPopup;

    private void Awake()
    {
        _connectionPopup = ConnectionPopup.Instance;
        _settingsPopup = SettingsPopup.Instance;
        
        SetState(eMainMenuState.MainMenu);
    }

    public void SetState(eMainMenuState state)
    {
        if (_currentState == state) return;

        EnableButtons(false);
        _connectionPopup.Hide();
        _settingsPopup.Hide();

        switch (state)
        {
            case eMainMenuState.MainMenu:
                EnableButtons(true);
                break;
            case eMainMenuState.ConnectionPopup:
                _connectionPopup.Show();
                break;
            case eMainMenuState.SettingsPopup:
                _settingsPopup.Show();
                break;
        }

        _currentState = state;
    }

    public void SetState(int state)
    {
        SetState((eMainMenuState)state);
    }

    private void EnableButtons(bool enabled)
    {
        _serverBrowserButton.interactable = enabled;
        _settingsButton.interactable = enabled;
        _quitButton.interactable = enabled;
    }

    public void OnServerBrowserButtonPressed()
    {
        SetState(eMainMenuState.ConnectionPopup);
    }

    public void OnSettingsButtonPressed()
    {
        SetState(eMainMenuState.SettingsPopup);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
