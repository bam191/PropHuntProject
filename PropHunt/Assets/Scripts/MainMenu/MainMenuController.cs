using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private enum eMainMenuState
    {
        MainMenu,
        ConnectionPopup,
        SettingsPopup
    }

    [SerializeField] private Button _serverBrowserButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    private eMainMenuState _currentState;

    private IPopup _connectionPopup;
    private IPopup _settingsPopup;

    private void Awake()
    {
        _connectionPopup = ConnectionPopup.Instance;
        _settingsPopup = SettingsPopup.Instance;
        
        SetState(eMainMenuState.MainMenu);
    }

    private void SetState(eMainMenuState state)
    {
        if (_currentState == state) return;
        
        EnableButtons(false);
        _connectionPopup.Hide();
        _settingsPopup.Hide();

        switch(state)
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

    private void EnableButtons(bool enabled)
    {
        _serverBrowserButton.interactable = enabled;
        _settingsButton.interactable = enabled;
        _quitButton.interactable = enabled;
    }

    private void OnServerBrowserButtonPressed()
    {
        _connectionPopup.Show();
    }

    private void OnSettingsButtonPressed()
    {
        _settingsPopup.Show();
    }

    private void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
