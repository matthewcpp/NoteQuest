using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NoteQuest;
using UnityEngine.SocialPlatforms.Impl;
using ABC;
using ABCUnity;
using System;

namespace NoteQuest
{
    public class ScoreModeUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI noteText;
        [SerializeField] TextMeshProUGUI activeNotes;
        [SerializeField] EzMidi.Connection midi;
        [SerializeField] Streak streak;

        public ScoreMode scoreMode;

        private void Update()
        {
            UpdateNoteText();
            UpdateActiveNotes();
            streak.count = scoreMode.streak;
        }

        public void ShowScore(bool show)
        {
            scoreMode.gameObject.SetActive(show);
        }

        private void UpdateNoteText()
        {
            if (scoreMode.tune == null)
                return;

            int voiceCount = scoreMode.tune.voices.Count;
            string noteStr = string.Empty;

            for (int i = 0; i < voiceCount; i++)
            {
                var activeItem = scoreMode.GetActiveVoiceItem(i);
                if (i > 0)
                    noteStr += '\n';

                if (activeItem != null)
                    noteStr += Util.GetTextString(activeItem);
            }

            noteText.text = noteStr;
        }

        private void UpdateActiveNotes()
        {
            string text = string.Empty;
            int i = 0;
            foreach (var activeNote in midi.notesOn)
            {
                if (i > 0)
                    text += '\n';

                var noteInfo = MidiToABC.Convert(activeNote);
                text += Util.NoteText(noteInfo.pitch, noteInfo.accidental);
                i += 1;
            }

            activeNotes.text = text;
        }
    }
}
