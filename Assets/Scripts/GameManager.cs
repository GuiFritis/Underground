using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Padrao.Core.Singleton;
using UnityEngine.InputSystem;
using Sounds;

public class GameManager : Singleton<GameManager>
{
    public InputActionReference inputAction;
    public Player player;
    [Space]
    public AudioClip gameOverSFX;
    public AudioClip menuSFX;

    protected override void Awake()
    {
        base.Awake();
        SetInput();
        player.healthBase.OnDeath += h => GameOver();
    }

    void OnDisable() 
    {
        inputAction.action.Disable();

        inputAction.action.started -= CallMenu;
    }

    private void SetInput()
    {
        inputAction.action.Enable();

        inputAction.action.started += CallMenu;
    }

    public void CallMenu(InputAction.CallbackContext ctx)
    {
        if(menuSFX != null)
        {
            SFX_Pool.Instance.Play(menuSFX);
        }
        bool screenState = !ScreenManager.Instance.GetScreenStateByType(GameplayScreenType.MENU);
        Debug.Log(screenState?"true":"false");        
        ScreenManager.Instance.ShowScreen(GameplayScreenType.MENU, screenState);
        Time.timeScale = screenState ? 0 : 1;
    }

    private void GameOver()
    {
        if(gameOverSFX != null)
        {
            MusicPlayer.Instance.StopMusic();
            SFX_Pool.Instance.Play(gameOverSFX);
        }
        inputAction.action.Disable();
        ScreenManager.Instance.HideAllScreens();
        ScreenManager.Instance.ShowScreen(GameplayScreenType.GAME_OVER);
        Time.timeScale = 0;
    }
}
