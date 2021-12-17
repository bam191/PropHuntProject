using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : Popup
{
    private static SettingsPopup _instance;
    public static SettingsPopup Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SettingsPopup>(true);
            }

            return _instance;
        }
    }

    [SerializeField] private Toggle _vsyncToggle;
    [SerializeField] private TMP_Dropdown _antialiasingDropdown;
    [SerializeField] private TMP_Dropdown _shadowDistanceDropdown;
    [SerializeField] private TMP_Dropdown _shadowResolutionDropdown;
    [SerializeField] private TMP_Dropdown _shadowQualityDropdown;
    [SerializeField] private TMP_Dropdown _textureQualityDropdown;

    private void Awake()
    {
        if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    public override void Show()
    {
        SetDefaultUIValues();

        gameObject.SetActive(true);
        base.Show();
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
        base.Hide();
    }

    private void SetDefaultUIValues()
    {
        _vsyncToggle.isOn = QualitySettings.vSyncCount == 1;
        _antialiasingDropdown.SetValueWithoutNotify(QualitySettings.antiAliasing / 2);
        _shadowDistanceDropdown.SetValueWithoutNotify(GetShadowDistance());
        _shadowResolutionDropdown.SetValueWithoutNotify((int)QualitySettings.shadowResolution);
        _shadowQualityDropdown.SetValueWithoutNotify((int)QualitySettings.shadows);
        _textureQualityDropdown.SetValueWithoutNotify(QualitySettings.masterTextureLimit);
    }

    private int GetShadowDistance()
    {
        float shadowDistance = QualitySettings.shadowDistance;

        if (shadowDistance > 190)
        {
            return 3;
        }
        
        if (shadowDistance > 90)
        {
            return 2;
        }

        if (shadowDistance > 49)
        {
            return 1;
        }

        return 0;
    }
}
