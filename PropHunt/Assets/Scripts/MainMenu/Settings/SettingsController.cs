using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public void SetAntiAliasing(int aaLevel)
    {
        QualitySettings.antiAliasing = aaLevel*2;
    }

    public void SetShadowDistance(int shadowDistance)
    {
        float shadowDistanceFloat = 0;
        switch(shadowDistance)
        {
            case 0:
                shadowDistanceFloat = 10;
                break;
            case 1:
                shadowDistanceFloat = 50;
                break;
            case 2:
                shadowDistanceFloat = 100;
                break;
            case 3:
                shadowDistanceFloat = 200;
                break;
        }
        QualitySettings.shadowDistance = shadowDistanceFloat;
    }

    public void SetShadowQuality(int shadowQuality)
    {
        QualitySettings.shadows = (ShadowQuality)shadowQuality;
    }

    public void SetShadowResolution(int shadowResolution)
    {
        QualitySettings.shadowResolution = (ShadowResolution)shadowResolution;
    }

    public void EnableVsync(bool enabled)
    {
        switch(enabled)
        {
            case true:
                QualitySettings.vSyncCount = 1;
                break;
            case false:
                QualitySettings.vSyncCount = 0;
                break;
        }
    }

    public void SetTextureQuality(int qualityLevel)
    {
        QualitySettings.masterTextureLimit = qualityLevel;
    }
}
