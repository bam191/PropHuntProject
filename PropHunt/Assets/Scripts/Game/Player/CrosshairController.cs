using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : Singleton<CrosshairController>
{
    private struct CrosshairInfo
    {
        public float CrosshairSize;
        public float CrosshairSeparation;
    }

    private readonly Dictionary<eGunType, CrosshairInfo> _baseCrosshairSizes = new Dictionary<eGunType, CrosshairInfo>()
    {
        {eGunType.None, new CrosshairInfo(){CrosshairSize = 10, CrosshairSeparation = 10}},
        {eGunType.Pistol, new CrosshairInfo(){CrosshairSize = 10, CrosshairSeparation = 10}},
        {eGunType.Rifle, new CrosshairInfo(){CrosshairSize = 15, CrosshairSeparation = 20}},
        {eGunType.Sniper, new CrosshairInfo(){CrosshairSize = 10, CrosshairSeparation = 10}},
        {eGunType.Shotgun, new CrosshairInfo(){CrosshairSize = 25, CrosshairSeparation = 40}},
        {eGunType.RPG, new CrosshairInfo(){CrosshairSize = 30, CrosshairSeparation = 35}}
    };

    [SerializeField] List<RectTransform> _crosshairArms = new List<RectTransform>();

    private RectTransform _rectTransform;

    private float _baseSize;
    private float _baseSeparation;

    private float _separationMultiplier;

    private bool _isShown = true;

    public override void Initialize()
    {
        _rectTransform = GetComponent<RectTransform>();
        SetCrosshairType(eGunType.None);
        base.Initialize();
    }

    public void ShowCrosshair(bool isShown)
    {
        _isShown = isShown;
        gameObject.SetActive(isShown);
    }

    public void SetCrosshairType(eGunType gunType)
    {
        _baseSeparation = _baseCrosshairSizes[gunType].CrosshairSeparation;
        _baseSize = _baseCrosshairSizes[gunType].CrosshairSize;

        _rectTransform.sizeDelta = new Vector2(_baseSeparation, _baseSeparation);

        foreach(RectTransform rectTransform in _crosshairArms)
        {
            rectTransform.sizeDelta = new Vector2(_baseSize, _baseSize);
        }
    }

    public void SetSeparationMultiplier(float multiplier)
    {
        _separationMultiplier = Mathf.Clamp(multiplier, 1, 5);

        _rectTransform.sizeDelta = new Vector2(_baseSeparation, _baseSeparation) * _separationMultiplier;
    }

}
