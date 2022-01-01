using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
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
    
    [SerializeField] private ConnectionPopup _connectionPopup;
    
    private Popup _settingsPopup;
    [SerializeField] private Transform _settingsParent;
    [SerializeField] private GameObject _settingsPopupPrefab;

    private eMainMenuState _currentState = eMainMenuState.None;

    public override void Initialize()
    {
        InitSettings();
        SetState(eMainMenuState.MainMenu);
    }

    private void Update()
    {
        if (Input.GetKeyDown(PlayerConstants.PauseMenu))
        {
            switch (_currentState)
            {
                case eMainMenuState.ConnectionPopup:
                    SetState(eMainMenuState.MainMenu);
                    break;
                case eMainMenuState.SettingsPopup:
                    SetState(eMainMenuState.MainMenu);
                    break;
            }
        }
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
    
    private void InitSettings()
    {
        GameObject settingsInstance = Instantiate(_settingsPopupPrefab, _settingsParent);
        _settingsPopup = settingsInstance.GetComponent<SettingsPopup>();
        _settingsPopup.OnHide += () => SetState(eMainMenuState.MainMenu);
    }

    public void TryHost()
    {
        NetworkManager.Singleton.StartHost();
    }
}
