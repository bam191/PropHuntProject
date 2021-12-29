using System.Collections.Generic;
using UnityEngine;

public static class PlayerConstants
{
    public static float MoveSpeed = 6f;
    public static float NoClipMoveSpeed = 12f;
    public static float BackWardsMoveSpeedScale = 0.9f;
    public static float CrouchingMoveSpeed = 5f;
    public static float MaxVelocity = 60f;
    public static float MaxReasonableVelocity = 30f;
    public static float MaxFallSpeed = 40f;

    public static float Gravity = 15f;
    public static float JumpPower = 5.6f;
    public static float CrouchingJumpPower = 5f;

    public static float GroundAcceleration = 15f;
    public static float AirAcceleration = 1000f;
    public static float AirAccelerationCap = .7f;

    public static float StopSpeed = 6f;
    public static float Friction = 10f;
    public static float MinimumSpeedCutoff = 0.5f; // This is the speed after which the player is immediately stopped due to friction
    public static float NormalSurfaceFriction = 1f;

    public static float StandingPlayerHeight = 2f;
    public static Vector3 StandingCameraOffset = new Vector3(0, 1.638f, 0);

    public static float CrouchingPlayerHeight = 1f;
    public static Vector3 CrouchingCameraOffset = new Vector3(0, 1, 0);
    public static float PlayerColliderRadius = 0.5f;

    //HotKeys
    public static string Forward = "Forward";
    public static string ForwardDefault = "W";
    public static string ForwardTooltip = "Moves player forward.";

    public static string Back = "Back";
    public static string BackDefault = "S";
    public static string BackTooltip = "Moves player backward.";

    public static string Left = "Left";
    public static string LeftDefault = "A";
    public static string LeftTooltip = "Moves player left.";

    public static string Right = "Right";
    public static string RightDefault = "D";
    public static string RightTooltip = "Moves player right.";
    
    public static string Up = "Up";
    public static string UpDefault = "E";
    public static string UpTooltip = "Moves player camera up.";
    
    public static string Down = "Down";
    public static string DownDefault = "Q";
    public static string DownTooltip = "Moves player camera down.";

    public static string Jump = "Jump";
    public static string JumpDefault = "Space";
    public static string JumpTooltip = "Makes player jump.";
    
    public static string Crouch = "Crouch";
    public static string CrouchDefault = "LeftControl";
    public static string CrouchTooltip = "Makes player crouch.";
    
    public static string Interact = "Interact";
    public static string InteractDefault = "E";
    public static string InteractTooltip = "Allows props to become a prop.";
    
    public static string ToggleUI = "ToggleUI";
    public static string ToggleUIDefault = "Z";
    public static string ToggleUITooltip = "Toggles off all UI.";

    public static string ModifierKey = "Modifier Key";
    public static string ModifierKeyDefault = "LeftShift";
    public static string ModifierKeyTooltip = "When pressed at the same time, modifies the action of other keys";


    // Tooltips
    public static string SensitivityTooltip = "Mouse Sensitivity.";
    public static string MasterVolumeTooltip = "Master Volume.";
    public static string MusicVolumeTooltip = "Music Volume.";
    public static string SoundEffectVolumeTooltip = "Sound Effect Volume.";

    public static string ResolutionTooltip = "Screen Resolution.";
    public static string GraphicsTooltip = "Graphics Quality.";
    public static string FullscreenTooltip = "Toggle Fullscreen.";
    public static string FOVTooltip = "Player Camera Field of View.";

    public static string HotKeyPattern = "(?<=\\[)[^\\]]*(?=\\])";


    // Non-changeable hotkeys
    public static string MouseX = "Mouse X";
    public static string MouseY = "Mouse Y";
    public static KeyCode PauseMenu = KeyCode.Escape;

    // Game Constants
    public static int MainMenuSceneIndex = 0;

    public static Vector3 PlayerSpawnOffset = new Vector3(0, 1.1f, 0);
    public static int PlayerLayer = 12;

    // UI Constants
    public static Color activeColor = new Color(.58f, .93f, .76f);
    public static Color inactiveColor = new Color(0.5f, 0.5f, 0.5f);
    public static Color hoverColor = new Color(1, 1, 1);
}
