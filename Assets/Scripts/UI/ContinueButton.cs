using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public void HideMenu()
    {
        ScreenManager.Instance.ShowScreen(GameplayScreenType.MENU, false);
        Time.timeScale = 1;
    }
}
