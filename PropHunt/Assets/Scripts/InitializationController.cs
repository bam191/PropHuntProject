using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializationController : MonoBehaviour
{
    private const string MAIN_MENU_SCENE = "MainMenu";

    private void Start()
    {
        _ = LaunchGame();
    }  

    private async Task LaunchGame()
    {
        await Task.Delay(1000);

        SceneManager.LoadScene(MAIN_MENU_SCENE, LoadSceneMode.Single);

        await Task.CompletedTask;
    }
}
