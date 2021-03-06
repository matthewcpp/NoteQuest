﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NoteQuest
{
    public class ArcadeUI : MonoBehaviour
    {
        ArcadeMode arcadeMode;
        Streak streak;
        TextMeshProUGUI noteText;
        TextMeshProUGUI statusText;

        private void Start()
        {
            arcadeMode = FindObjectOfType<ArcadeMode>();
            streak = transform.Find("Streak").GetComponent<Streak>();
            noteText = transform.Find("NoteText").GetComponent<TextMeshProUGUI>();
            statusText = transform.Find("StatusText").GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            SetNoteText();

            statusText.text = $"{arcadeMode.corret}/{arcadeMode.total}";
            streak.count = arcadeMode.streak;
        }

        void SetNoteText()
        {
            if (arcadeMode.currentItem == null)
            {
                noteText.text = string.Empty;
            }
            else if (arcadeMode.currentItem.type == ABC.Item.Type.Note)
            {
                var note = arcadeMode.currentItem as ABC.Note;
                string text = note.pitch.ToString();

                if (note.accidental == ABC.Accidental.Sharp)
                    text = "#" + text;
                else if (note.accidental == ABC.Accidental.Flat)
                    text = "b " + text;

                noteText.text = text;
            }
        }

        public void OnSpeedChange(float value)
        {
            arcadeMode.secondsToAnswer = value;
        }

        public void OnScrollingChange(bool scrollingEnabled)
        {
            var speedSlider = transform.Find("Speed").GetComponent<Slider>();
            var toggle = transform.Find("ScrollingEnabled").GetComponent<Toggle>();

            var enabled = toggle.isOn;
            speedSlider.value = 3.0f;
            speedSlider.gameObject.SetActive(enabled);
            
            arcadeMode.style = enabled ? ArcadeMode.Style.Scrolling : ArcadeMode.Style.Static;
            arcadeMode.ResetArcade();
        }
    }

}
