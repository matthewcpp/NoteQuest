using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace NoteQuest
{
    public class ScoreModeController : MonoBehaviour
    {
        [SerializeField] GameObject scoreModePrefab;
        [SerializeField] GameObject scorePickerPrefab;
        [SerializeField] GameObject layoutPrefab;

        TextMeshProUGUI noteText;
        TextMeshProUGUI titleText;
        TextMeshProUGUI activeNotes;

        EzMidi.Connection midi;
        GameObject filePicker;
        GameObject scoreMode;
        ABCUnity.Layout layout;
        ScoreStatus scoreStatus;

        enum State { Initial, Picker, Score}
        State activeState = State.Initial;

        void Awake()
        {
            ShowFilePicker();
        }

        void Update()
        {
            if (activeState == State.Score)
            {
                UpdateNoteText();
                UpdateActiveNotes();
            }
        }

        public void ShowFilePicker()
        {
            if (activeState == State.Picker) return;

            InitFilePicker();

            if (!filePicker.activeSelf)
                filePicker.SetActive(true);

            activeState = State.Picker;
        }

        void ReturnToPicker()
        {
            scoreMode.SetActive(false);
            layout.gameObject.SetActive(false);
            
            ShowFilePicker();
        }

        private void InitFilePicker()
        {
            if (filePicker == null)
            {
                filePicker = GameObject.Instantiate(scorePickerPrefab);
                var picker = filePicker.GetComponent<FileSystemScorePicker>();
                picker.selection += OnScoreSelected;
            }
        }

        private void InitScoreMode()
        {
            if (scoreMode != null)
                return;

            scoreMode = GameObject.Instantiate(scoreModePrefab);

            var scoreModeTransform = scoreMode.transform;
            noteText = scoreModeTransform.Find("CurrentNotes").GetComponent<TextMeshProUGUI>();
            activeNotes = scoreModeTransform.Find("ActiveNotes").GetComponent<TextMeshProUGUI>();
            titleText = scoreModeTransform.Find("Title").GetComponent<TextMeshProUGUI>();

            var eventTrigger = scoreModeTransform.Find("Reset").GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { ResetScore(); });
            eventTrigger.triggers.Add(entry);

            eventTrigger = scoreModeTransform.Find("Back").GetComponent<EventTrigger>();
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { ReturnToPicker(); });
            eventTrigger.triggers.Add(entry);

            var layoutObj = GameObject.Instantiate(layoutPrefab, this.transform);
            layout = layoutObj.GetComponent<ABCUnity.Layout>();

            midi = FindObjectOfType<EzMidi.Connection>();
            scoreStatus = new ScoreStatus(midi, layout);

            UpdateLayout();
        }

        public void OnScoreSelected(string selection)
        {
            Debug.Log($"ScoreSelected {selection}");
            ShowScore();
            layout.LoadFile(selection);
            titleText.text = layout.tune.title;
        }

        public void ShowScore()
        {
            if (activeState == State.Score) return;

            if (filePicker != null && filePicker.activeSelf)
                filePicker.SetActive(false);

            InitScoreMode();

            scoreMode.SetActive(true);
            layout.gameObject.SetActive(true);

            activeState = State.Score;
        }

        void UpdateLayout()
        {
            var orthoSize = Camera.main.orthographicSize;
            var aspect = Camera.main.aspect;
            var orthoHeight = (orthoSize * 2.0f);
            var orthoWidth = (orthoSize * 2.0f) * aspect;

            var layoutTransform = layout.GetComponent<RectTransform>();

            var layoutHeight = orthoHeight - (orthoHeight * 0.2f);
            var layoutWidth = orthoWidth - (orthoWidth * 0.2f);
            var layoutPosY = orthoSize - (orthoHeight - layoutHeight) / 2.0f;

            layoutTransform.position = new Vector3(0, layoutPosY, 0.0f);
            layoutTransform.sizeDelta = new Vector2(layoutWidth, layoutHeight);
        }

        private void UpdateNoteText()
        {
            if (layout.tune == null)
                return;

            int voiceCount = layout.tune.voices.Count;
            string noteStr = string.Empty;

            for (int i = 0; i < voiceCount; i++)
            {
                var activeItem = scoreStatus.GetActiveVoiceItem(i);
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
        public void ResetScore()
        {
            layout.ResetItemColors();
            scoreStatus.ResetScore();
        }
    }
}

