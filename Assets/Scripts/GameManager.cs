using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Padrao.Core.Singleton;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    public InputActionReference inputAction;

    protected override void Awake()
    {
        base.Awake();
        SetInput();
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
}
