using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoteQuest
{
    public class ArcadeMode : MonoBehaviour
    {
        EzMidi.Connection midi;
        ArcadeStaff arcadeStaff;

        private void Awake()
        {
            midi = FindObjectOfType<EzMidi.Connection>();
            arcadeStaff = transform.Find("Staff").GetComponent<ArcadeStaff>();
        }

        GameObject activeObject;
        public int attemptsAllowed { get; set; } = 1;
        public int attemptsRemaining { get; private set; }

        public int total { get; private set; } = 0;
        public int corret { get; private set; } = 0;
        public int streak { get; private set; } = 0;

        HashSet<int> targetNotes = new HashSet<int>();

        public enum Style { Scrolling, Static}

        public Style style { get => _style; set { SetStyle(value); } }
        private Style _style;

        const float fadeOutTime = 0.4f;

        private void SetStyle(Style s)
        {
            _style = s;
            SetStaffSize();

            arcadeStaff.scrollingEnabled = _style == Style.Scrolling;
        }

        private class PitchMinMax
        {
            public ABC.Pitch min { get; }
            public ABC.Pitch max { get; }

            public PitchMinMax(ABC.Pitch min, ABC.Pitch max)
            {
                this.min = min;
                this.max = max;
            }
        }

        private Dictionary<ABC.Clef, PitchMinMax> defaultMinMax = new Dictionary<ABC.Clef, PitchMinMax>()
        {
            { ABC.Clef.Treble, new PitchMinMax(ABC.Pitch.C4, ABC.Pitch.C6)},
            { ABC.Clef.Bass, new PitchMinMax(ABC.Pitch.C2, ABC.Pitch.C4)},
        };

        const float defaultSecondsToAnswer = 3.0f;
        public float secondsToAnswer { get => _secondsToAnswer; set { SetSecondsToAnswer(value); } }
        private float _secondsToAnswer = defaultSecondsToAnswer;
        private void SetSecondsToAnswer(float time)
        {
            _secondsToAnswer = time;
            arcadeStaff.speed = arcadeStaff.staffWidth / _secondsToAnswer;
        }

        public ABC.Duration currentItem { get; private set; }

        void Start()
        {
            style = Style.Static;
            ResetArcade();
        }

        void OnDisable()
        {
            midi.NoteOn -= OnKeyDown;
        }

        public void ResetArcade()
        {
            currentItem = null;
            total = 0;
            corret = 0;
            streak = 0;

            arcadeStaff.ResetStaff();
            secondsToAnswer = defaultSecondsToAnswer;

            arcadeStaff.onShredNote += OnShredNote;
            activeObject = arcadeStaff.SpawnNote(GetNextNote(), Vector3.left);

            midi.NoteOn += OnKeyDown;
        }

        private void OnShredNote(GameObject noteObj)
        {
            if (noteObj != activeObject)
                return;

            if (targetNotes.Count > 0) // They did not hit all the required notes in time
            {
                streak = 0;
                ABCUnity.Util.SetObjectColor(noteObj, Color.red);
                arcadeStaff.FadeNoteOut(noteObj, 1.0f);
                arcadeStaff.SetDirection(noteObj, Vector3.up);
            }

            activeObject = arcadeStaff.SpawnNote(GetNextNote(), Vector3.left);
        }

        ABC.Note GetNextNote()
        {
            var range = defaultMinMax[arcadeStaff.clef];
            var pitch = (ABC.Pitch)UnityEngine.Random.Range((int)range.min, (int)range.max);

            var random = UnityEngine.Random.Range(0.0f, 1.0f);
            var accidental = ABC.Accidental.Unspecified;

            if (random < 0.2f)
                accidental = pitch != ABC.Pitch.A0 ? ABC.Accidental.Flat : ABC.Accidental.Unspecified;
            else if (random < 0.4f)
                accidental = pitch != ABC.Pitch.C8 ? ABC.Accidental.Sharp : ABC.Accidental.Unspecified;

            var note = new ABC.Note(pitch, ABC.Length.Quarter, accidental);
            Util.AdjustNote(note);

            attemptsRemaining = attemptsAllowed;
            AbcToMidi.Convert(note, targetNotes);
            currentItem = note;
            total += 1;

            return note;
        }

        void OnKeyDown(int channel, int note, int velocity)
        {
            if (targetNotes.Contains(note))
            {
                targetNotes.Remove(note);

                if (targetNotes.Count == 0)
                {
                    ABCUnity.Util.SetObjectColor(activeObject, Color.green);
                    corret += 1;
                    streak += 1;

                    arcadeStaff.FadeNoteOut(activeObject, fadeOutTime);
                    SpawnNextNote();
                }
            }
            else
            {
                attemptsRemaining -= 1;
                streak = 0;

                if (attemptsRemaining >= 0)
                {
                    var noteInfo = MidiToABC.Convert(note);
                    var incorrectNote = new ABC.Note(noteInfo.pitch, ABC.Length.Quarter, noteInfo.accidental);
                    var incorrectObj = arcadeStaff.SpawnNote(incorrectNote, Vector3.left, activeObject.transform.localPosition.x);
                    ABCUnity.Util.SetObjectColor(incorrectObj, Color.yellow);
                    arcadeStaff.FadeNoteOut(incorrectObj, fadeOutTime);
                }
                else
                {
                    ABCUnity.Util.SetObjectColor(activeObject, Color.red);
                    arcadeStaff.FadeNoteOut(activeObject, fadeOutTime);
                    arcadeStaff.SetDirection(activeObject, Vector3.up);
                    SpawnNextNote();
                }
            }
        }

        void SpawnNextNote()
        {
            if (style == Style.Scrolling)
                activeObject = arcadeStaff.SpawnNote(GetNextNote(), Vector3.left);
            else
            {
                activeObject = null;
                StartCoroutine(SpawnNoteAfter(fadeOutTime));
            }
        }

        IEnumerator SpawnNoteAfter(float time)
        {
            yield return new WaitForSeconds(time);
            activeObject = arcadeStaff.SpawnNote(GetNextNote(), Vector3.left);
        }

        void SetStaffSize()
        {
            var rectTransform = arcadeStaff.GetComponent<RectTransform>();

            var orthoSize = Camera.main.orthographicSize;
            var aspect = Camera.main.aspect;

            var viewHeight = orthoSize * 2.0f;
            var viewWidth = viewHeight * aspect;
            var minX = -viewWidth / 2.0f;

            const float referenceStaffHeight = 2.25f;
            float scaleValue = rectTransform.rect.height / referenceStaffHeight;

            if (style == Style.Scrolling)
            {
                arcadeStaff.transform.position = new Vector3(minX + (viewWidth * 0.1f), 0.0f, 0.0f);
                arcadeStaff.staffWidth = (viewWidth * 0.8f) * (1.0f / scaleValue);
                arcadeStaff.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            }
            else
            {
                const float staticWidth = 5.0f;
                arcadeStaff.transform.position = new Vector3(-staticWidth / 2.0f, 0.0f, 0.0f);
                arcadeStaff.staffWidth = staticWidth * (1.0f / scaleValue);
                arcadeStaff.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            }
        }
    }

}
