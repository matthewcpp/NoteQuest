using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEditor.UI;

namespace NoteQuest
{
    public class ArcadeModeController : MonoBehaviour
    {
        [SerializeField] GameObject arcadeModePrefab;
        [SerializeField] GameObject arcadeModeUIPrefab;

        ArcadeMode arcadeMode;
        GameObject arcadeModeUI;


        EzMidi.Connection midi;
        void Start()
        {
            arcadeMode = GameObject.Instantiate(arcadeModePrefab, this.transform).GetComponent<ArcadeMode>();
            arcadeMode.ResetArcade();
            arcadeModeUI = GameObject.Instantiate(arcadeModeUIPrefab, this.transform);

            var backButton = arcadeModeUI.transform.Find("Back").GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { ReturnToMainMenu(); });
            backButton.triggers.Add(entry);
        }

        void ReturnToMainMenu()
        {
            this.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            Debug.Log("Enter Arcade mode!");
            if (arcadeMode != null)
                arcadeMode.ResetArcade();
        }
    }

}

