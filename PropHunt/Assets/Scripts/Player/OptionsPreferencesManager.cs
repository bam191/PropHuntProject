using UnityEngine;

public class OptionsPreferencesManager
{
    #region Controls
    public const string sensitivityKey = "Sensitivity";
    public const float defaultSensitivity = 0.2f;
    #endregion

    #region Audio
    public const string masterVolumeKey = "MasterVolume";
    public const int defaultMasterVolume = -10;

    public const string musicVolumeKey = "Volume";
    public const int defaultMusicVolume = -20;

    public const string soundEffectVolumeKey = "SoundEffects";
    public const int defaultSoundEffectVolume = -80;
    #endregion

    #region Video
    public const string resolutionWidthKey = "ResolutionWidth";
    public const int defaultResolutionWidth = 1920;

    public const string resolutionHeightKey = "ResolutionHeight";
    public const int defaultResolutionHeight = 1080;

    public const string qualityKey = "Quality";
    public const int defaultQuality = 0;

    public const string fullScreenKey = "IsFullScreen";
    public const int defaultIsFullScreen = 0;

    public const string cameraFOVKey = "CameraFOV";
    public const int defaultCameraFOV = 90;

    public const string vsyncKey = "vsync";
    public const int defaultVsync = 0;
    #endregion


    public static int GetResolutionWidth()
    {
        return PlayerPrefs.GetInt(resolutionWidthKey, defaultResolutionWidth);
    }

    public static int GetResolutionHeight()
    {
        return PlayerPrefs.GetInt(resolutionHeightKey, defaultResolutionHeight);
    }

    public static void SetResolution(int width, int height)
    {
        PlayerPrefs.SetInt(resolutionWidthKey, width);
        PlayerPrefs.SetInt(resolutionHeightKey, height);
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(musicVolumeKey, defaultMusicVolume);
    }

    public static void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat(musicVolumeKey, volume);
    }

    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(masterVolumeKey, defaultMasterVolume);
    }

    public static void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat(masterVolumeKey, volume);
    }

    public static float GetSoundEffectVolume()
    {
        return PlayerPrefs.GetFloat(soundEffectVolumeKey, defaultSoundEffectVolume);
    }

    public static void SetSoundEffectVolume(float volume)
    {
        PlayerPrefs.SetFloat(soundEffectVolumeKey, volume);
    }

    public static int GetQuality()
    {
        return PlayerPrefs.GetInt(qualityKey, defaultQuality);
    }

    public static void SetQuality(int quality)
    {
        PlayerPrefs.SetInt(qualityKey, quality);
    }

    public static bool GetFullScreen()
    {
        int isFullScreen = PlayerPrefs.GetInt(fullScreenKey, defaultIsFullScreen);
        return isFullScreen != 0;
    }

    public static void SetFullScreen(bool isFullScreen)
    {
        PlayerPrefs.SetInt(fullScreenKey, isFullScreen ? 1 : 0);
    }

    public static float GetSensitivity()
    {
        return PlayerPrefs.GetFloat(sensitivityKey, defaultSensitivity);
    }

    public static void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat(sensitivityKey, sensitivity);
    }

    public static int GetCameraFOV()
    {
        return PlayerPrefs.GetInt(cameraFOVKey, defaultCameraFOV);
    }

    public static void SetCameraFOV(int fieldOfViewLevel)
    {
        PlayerPrefs.SetInt(cameraFOVKey, fieldOfViewLevel);
    }

    public static int GetVsync()
    {
        return PlayerPrefs.GetInt(vsyncKey, defaultVsync);
    }

    public static void SetVsync(int vsyncCount)
    {
        PlayerPrefs.SetInt(vsyncKey, vsyncCount);
    }
}
