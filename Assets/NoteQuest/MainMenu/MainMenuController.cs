using NoteQuest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] ArcadeModeController arcadeModeController;
    [SerializeField] ScoreModeController scoreModeController;
    [SerializeField] SettingsController settingsController;
    [SerializeField] GameObject mainMenu;

    private GameObject activeController;
    
    public void EnterScoreMode()
    {
        mainMenu.SetActive(false);
        activeController = scoreModeController.gameObject;
        activeController.SetActive(true);
    }

    public void EnterArcadeMode()
    {
        mainMenu.SetActive(false);
        activeController = arcadeModeController.gameObject;
        activeController.SetActive(true);
    }

    public void EnterSettingsMode()
    {
        mainMenu.SetActive(false);
        activeController = settingsController.gameObject;
        activeController.SetActive(true);
    }

    public void Update()
    {
        if (activeController != null && !activeController.activeSelf)
        {
            mainMenu.SetActive(true);
            activeController = null;
        }
    }
}
