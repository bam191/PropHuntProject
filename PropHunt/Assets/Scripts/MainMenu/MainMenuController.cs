using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : Singleton<MainMenuController>
{
    public enum eMainMenuState
    {
        MainMenu,
        ConnectionPopup,
        SettingsPopup,
        None
    }

    [SerializeField] private Button _serverBrowserButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    private eMainMenuState _currentState = eMainMenuState.None;

    private void Awake()
    {
        SetInstance(this);

        SetState(eMainMenuState.MainMenu);
    }

    public void SetState(eMainMenuState state)
    {
        if (_currentState == state) return;

        EnableButtons(false);
        ConnectionPopup.Instance.Hide();
        SettingsPopup.Instance.Hide();

        switch (state)
        {
            case eMainMenuState.MainMenu:
                EnableButtons(true);
                break;
            case eMainMenuState.ConnectionPopup:
                ConnectionPopup.Instance.Show();
                break;
            case eMainMenuState.SettingsPopup:
                SettingsPopup.Instance.Show();
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
