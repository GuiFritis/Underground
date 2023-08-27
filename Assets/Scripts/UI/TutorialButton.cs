using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    private string _playerPrefTutorial = "TutorialDone";

    void Start()
    {
        if(PlayerPrefs.GetInt(_playerPrefTutorial, 0) == 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void RemakeTutorial()
    {
        PlayerPrefs.SetInt(_playerPrefTutorial, 0);
    }
}
