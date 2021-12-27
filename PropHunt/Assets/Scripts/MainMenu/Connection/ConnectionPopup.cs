using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConnectionPopup : Popup
{
    [SerializeField] private TMP_InputField _ipInput;
    [SerializeField] private TMP_Text _ipValidationText;
    [SerializeField] private Button _connectButton;

    private ConnectionController _connectionController;
    private void Awake()
    {
        _connectionController = GetComponent<ConnectionController>();
        _ipInput.onValueChanged.AddListener(delegate { OnIPChanged(_ipInput.text); });
    }

    public override void Show()
    {
        gameObject.SetActive(true);
        _connectButton.interactable = false;
        base.Show();
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
        base.Hide();
    }

    public void TryConnect()
    {
        _connectionController.ConnectToServer(_ipInput.text);
    }

    public void TryHost()
    {
        _connectionController.HostServer();
    }
    
    private void OnIPChanged(string newValue)
    {
        if (IPAddress.TryParse(newValue, out var ipAddress))
        {
            _connectButton.interactable = true;
            _ipValidationText.gameObject.SetActive(false);
        }
        else
        {
            _ipValidationText.gameObject.SetActive(true);
            _connectButton.interactable = false;
        }
    }
}
