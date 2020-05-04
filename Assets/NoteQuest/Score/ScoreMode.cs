using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.IO;

namespace NoteQuest
{
    public class ScoreMode : MonoBehaviour
    {
        [SerializeField] GameObject layoutPrefab;
        [SerializeField] EzMidi.Connection midi;

        ABCUnity.Layout layout;

        List<VoiceStatus> voiceStatuses;

        // contains the notes that are allowed to be down while still advancing to the next beat item
        HashSet<int> allowedNotes = new HashSet<int>();

        void Awake()
        {
            var layoutObj = GameObject.Instantiate(layoutPrefab, this.transform);
            layout = layoutObj.GetComponent<ABCUnity.Layout>();

            layout.onLoaded += OnTuneLoaded;
        }

        private void OnTuneLoaded(ABC.Tune tune)
        {
            voiceStatuses = new List<VoiceStatus>();

            for (int i  = 0; i < tune.voices.Count; i++)
            {
                var voiceStatus = new VoiceStatus(layout.GetAlignment(i));
                voiceStatus.NextMeasure(0);
                voiceStatuses.Add(voiceStatus);
                AddAllowedNotes(voiceStatus);
            }
        }

        private void Start()
        {
            var orthoSize = Camera.main.orthographicSize;
            var aspect = Camera.main.aspect;
            var orthoWidth = (orthoSize * 2.0f) * aspect;

            var layoutTransform = layout.GetComponent<RectTransform>();
            layoutTransform.position = new Vector3(0.1f, orthoSize - 1.0f, 0.0f);
            layoutTransform.sizeDelta = new Vector2(orthoWidth, orthoSize * 2.0f - 0.2f);

            midi.NoteOn += OnKeyDown;

            //layout.LoadString("M:C\nL:1/4\nC[CEG]C[CEG]|");
            layout.LoadStream(File.OpenRead("D:/temp/notequest.abc"));
        }

        bool readyForNextBeat = false;

        private void Update()
        {
            if (ReadyForNextBeat())
            {
                if (currentBeat < beatCount)
                    AdvanceBeat();
                else
                    AdvanceMeasure();

                readyForNextBeat = false;
            }
        }

        const int beatCount = 4;
        int currentMeasure = 0;
        int currentBeat = 1;
        bool complete = false;

        private void OnKeyDown(int channel, int note, int velocity)
        {
            if (complete)
                return;

            bool noteCorrect = false;

            foreach (var voiceStatus in voiceStatuses)
            {
                if (voiceStatus.remainingNotes.Contains(note))
                {
                    noteCorrect = true;
                    voiceStatus.remainingNotes.Remove(note);

                    if (voiceStatus.remainingNotes.Count == 0)
                    {
                        layout.SetItemColor(voiceStatus.beatNote, Color.green);

                        RemoveAllowedNotes(voiceStatus);
                        voiceStatus.NextBeatItem();
                        AddAllowedNotes(voiceStatus);
                    }
                }
            }

            // the note hit was not correct
            if (!noteCorrect)
            {
                foreach (var voiceStatus in voiceStatuses)
                {
                    if (voiceStatus.beatNote != null)
                        layout.SetItemColor(voiceStatus.beatNote, Color.yellow);
                }
            }
        }

        bool ReadyForNextBeat()
        {
            foreach (var voiceStatus in voiceStatuses)
            {
                if (!voiceStatus.isReadyForNextBeat)
                    return false;
            }

            // This method will check that all notes that should be pressed for this beat item have been lifted and we can proceed to the next item
            foreach (var note in midi.notesOn)
            {
                if (!allowedNotes.Contains(note))
                    return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void RemoveAllowedNotes(VoiceStatus status)
        {
            foreach (var note in status.allNotes)
                allowedNotes.Remove(note);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void AddAllowedNotes(VoiceStatus status)
        {
            foreach (var note in status.allNotes)
                allowedNotes.Add(note);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void AdvanceBeat()
        {
            currentBeat += 1;
            foreach (var voiceStatus in voiceStatuses)
            {
                RemoveAllowedNotes(voiceStatus);
                voiceStatus.NextBeat(currentBeat);
                AddAllowedNotes(voiceStatus);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void AdvanceMeasure()
        {
            currentMeasure += 1;
            currentBeat = 1;
            if (currentMeasure < voiceStatuses[0].alignment.measures.Count)
            {
                allowedNotes.Clear();
                foreach (var voiceStatus in voiceStatuses)
                {
                    voiceStatus.NextMeasure(currentMeasure);
                    AddAllowedNotes(voiceStatus);
                }
            }
            else
            {
                complete = true;
            }
        }
    }
}
