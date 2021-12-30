using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbySettingsController : MonoBehaviour
{
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private TMP_Dropdown _mapDropdown;
    [SerializeField] private Slider _propsPerSeekerSlider;
    [SerializeField] private TMP_Text _propsPerSeekerText;
    [SerializeField] private LobbyMapsSO _lobbyMapsSO;

    
    void Start()
    {
        SetupButtons();
        SetupPropsPerSeekerSlider();
        SetupMapDropdown();
    }

    private void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void SetupButtons()
    {
        _startButton.gameObject.SetActive(NetworkManager.Singleton.IsHost);
        _startButton.onClick.AddListener(StartGame);
        _quitButton.onClick.AddListener(QuitLobby);
    }

    private void SetupMapDropdown()
    {
        _mapDropdown.options.Clear();
        _mapDropdown.AddOptions(_lobbyMapsSO.lobbyMaps);
        _mapDropdown.onValueChanged.AddListener((mapIndex) => SetMap(_lobbyMapsSO.lobbyMaps[mapIndex]));
        _mapDropdown.value = 0;
    }

    private void SetupPropsPerSeekerSlider()
    {
        _propsPerSeekerSlider.onValueChanged.AddListener((value) => SetPropsPerSeeker( (int) value) );
        _propsPerSeekerSlider.onValueChanged.AddListener((value) => UpdatePropsPerSeekerText( (int) value) );
        _propsPerSeekerSlider.value = 1;
    }

    public void SetPropsPerSeeker(int propsPerSeeker)
    {
        LobbyController.Instance.LobbyData.SetPropsPerSeeker(propsPerSeeker);
    }

    private void UpdatePropsPerSeekerText(int propsPerSeeker)
    {
        _propsPerSeekerText.text = propsPerSeeker.ToString();
    }

    public void SetMap(string mapName)
    {
        LobbyController.Instance.LobbyData.SetMap(mapName);
    }

    public void QuitLobby()
    {
        LoadingController.Instance.LoadMainMenu();
    }

    public void StartGame()
    {
        GameController.Instance.SetState(eGameState.PreRound);
    }
}
