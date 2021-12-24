using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public enum ePauseMenuState
    {
        PauseMenu,
        SettingsPopup,
        None
    }

    private static PauseMenuController _instance;
    public static PauseMenuController Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    private ePauseMenuState _currentState = ePauseMenuState.PauseMenu;

    private Popup _settingsPopup;
    private Popup _pausePopup;

    private void Awake()
    {
        _instance = this;
        _settingsPopup = SettingsPopup.Instance;

        SetState(ePauseMenuState.None);
    }

    public void SetState(ePauseMenuState state)
    {
        if (_currentState == state) return;

        _pausePopup.Hide();
        _settingsPopup.Hide();

        switch (state)
        {
            case ePauseMenuState.PauseMenu:
                _pausePopup.Show();
                break;
            case ePauseMenuState.SettingsPopup:
                _settingsPopup.Show();
                break;
        }

        _currentState = state;
    }

    public void SetState(int state)
    {
        SetState((ePauseMenuState)state);
    }

    public void OnSettingsButtonPressed()
    {
        SetState(ePauseMenuState.SettingsPopup);
    }
    
    public void OnQuitButtonPressed()
    {

    }
}
