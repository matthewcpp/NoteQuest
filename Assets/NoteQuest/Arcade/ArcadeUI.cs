using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NoteQuest
{
    public class ArcadeUI : MonoBehaviour
    {
        [SerializeField] ArcadeMode arcadeMode;

        [SerializeField] TextMeshProUGUI noteText;

        [SerializeField] Streak streak;

        [SerializeField] TextMeshProUGUI statusText;

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
    }

}
