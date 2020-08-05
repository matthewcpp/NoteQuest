using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.IO;

namespace NoteQuest
{
    public class ScoreStatus
    {

        EzMidi.Connection midi;
        ABCUnity.Layout layout;

        List<VoiceStatus> voiceStatuses;

        // contains the notes that are allowed to be down while still advancing to the next beat item
        HashSet<int> allowedNotes = new HashSet<int>();

        public int streak { get; private set; } = 0;

        public ScoreStatus(EzMidi.Connection midi, ABCUnity.Layout layout)
        {
            this.midi = midi;
            this.midi.NoteOn += OnKeyDown;

            this.layout = layout;
            this.layout.onLoaded += OnTuneLoaded;
        }

        public void UnbindCallbacks()
        {
            this.midi.NoteOn -= OnKeyDown;
            this.layout.onLoaded -= OnTuneLoaded;
        }

        public ABC.Item GetActiveVoiceItem(int index)
        {
            if (voiceStatuses != null)
                return voiceStatuses[index].beatNote;
            else
                return null;
        }

        private void OnTuneLoaded(ABC.Tune tune)
        {
            ResetScoreValues();

            voiceStatuses = new List<VoiceStatus>();

            for (int i  = 0; i < tune.voices.Count; i++)
            {
                var voiceStatus = new VoiceStatus(layout.GetAlignment(i));
                voiceStatus.NextMeasure(0);
                voiceStatuses.Add(voiceStatus);
                AddAllowedNotes(voiceStatus);
            }

            UpdateStatus();
        }

        private void UpdateStatus()
        {
            foreach (var voiceStatus in voiceStatuses)
            {
                if (voiceStatus.remainingNotes.Count == 0)
                    AdvanceBeatItem(voiceStatus);
            }

            while (ReadyForNextBeat())
            {
                if (currentBeat < beatsInMeasure)
                    AdvanceBeat();
                else
                    AdvanceMeasure();
            }
        }

        private void ResetScoreValues()
        {
            allowedNotes.Clear();
            complete = false;
            currentMeasure = 0;
            currentBeat = 1;
            streak = 0;
        }

        public void ResetScore()
        {
            ResetScoreValues();

            foreach (var voiceStatus in voiceStatuses)
            {
                voiceStatus.NextMeasure(currentMeasure);
                AddAllowedNotes(voiceStatus);
            }

            layout.ResetItemColors();
        }

        // TODO: Read Time Signature from layout
        const int beatsInMeasure = 4;
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
                        streak += 1;
                    }
                }
            }

            // the note hit was not correct
            if (!noteCorrect)
            {
                foreach (var voiceStatus in voiceStatuses)
                {
                    if (voiceStatus.beatNote != null)
                    {
                        layout.SetItemColor(voiceStatus.beatNote, Color.yellow);
                        streak = 0;
                    }
                        
                }
            }

            UpdateStatus();
        }

        bool ReadyForNextBeat()
        {
            if (complete)
                return false;

            foreach (var voiceStatus in voiceStatuses)
            {
                if (!voiceStatus.isReadyForNextBeat)
                    return false;
            }

            // This method will check that all notes that should be pressed for this beat item have been lifted and we can proceed to the next item
            foreach (var note in midi.notesOn)
            {
                if (allowedNotes.Count > 0 && !allowedNotes.Contains(note))
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
        void AdvanceBeatItem(VoiceStatus voiceStatus)
        {
            RemoveAllowedNotes(voiceStatus);
            voiceStatus.NextBeatItem();
            AddAllowedNotes(voiceStatus);
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
