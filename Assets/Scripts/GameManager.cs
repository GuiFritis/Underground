using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Padrao.Core.Singleton;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    public InputActionReference inputAction;
    public Player player;

    protected override void Awake()
    {
        base.Awake();
        SetInput();
        player.healthBase.OnDeath += h => GameOver();
    }

    private void SetInput()
    {
        inputAction.action.Enable();

        inputAction.action.started += ctx => CallMenu();
    }

    public void CallMenu()
    {
        bool screenState = !ScreenManager.Instance.GetScreenStateByType(GameplayScreenType.MENU);
        ScreenManager.Instance.ShowScreen(GameplayScreenType.MENU, screenState);
        Time.timeScale = screenState ? 0 : 1;
    }

    private void GameOver()
    {
        ScreenManager.Instance.HideAllScreens();
        ScreenManager.Instance.ShowScreen(GameplayScreenType.GAME_OVER);
        Time.timeScale = 0;
    }
}
