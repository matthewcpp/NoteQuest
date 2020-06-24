using NoteQuest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] ArcadeModeController arcadeModeController;
    [SerializeField] ScoreModeController scoreModeController;
    [SerializeField] GameObject mainMenu;

    public void EnterScoreMode()
    {
        mainMenu.SetActive(false);
        scoreModeController.gameObject.SetActive(true);
    }

    public void EnterArcadeMode()
    {
        mainMenu.SetActive(false);
        arcadeModeController.gameObject.SetActive(true);
    }

    public void Update()
    {
        if (mainMenu.activeSelf)
            return;

        if (!arcadeModeController.gameObject.activeSelf && !scoreModeController.gameObject.activeSelf)
            mainMenu.SetActive(true);
    }
}
