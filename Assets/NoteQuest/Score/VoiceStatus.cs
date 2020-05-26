using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Dynamic;

namespace NoteQuest
{
    class VoiceStatus
    {
        public ABCUnity.Alignment alignment { get; }

        private ABCUnity.Alignment.Measure measure;

        private ABCUnity.Alignment.Beat beat;
        public int beatItemIndex { get; private set; } = 0;


        /// Current note that needs to be pressed for this voice
        public ABC.Item beatNote { get; private set; }
        public int beatNoteIndex { get; set; } = 0;
        /// Remaining midi notes that need to be pressed for this voice
        public HashSet<int> remainingNotes { get; } = new HashSet<int>();
        public List<int> allNotes { get; } = new List<int>();


        public VoiceStatus(ABCUnity.Alignment alignment)
        {
            this.alignment = alignment;
        }

        public void NextMeasure(int measureIndex)
        {
            beatItemIndex = 0;
            measure = alignment.measures[measureIndex];
            beat = measure.beats[beatItemIndex];
            NextBeat(1);
        }

        /// <summary>
        /// Advances to the next beat in the measure.  Note that a voice may not have an item for each beat
        /// Beats that do not have any items will be ignored and the current beat object will be null
        /// </summary>
        public void NextBeat(int currentBeat)
        {
            if (beatItemIndex == measure.beats.Count)
                return;

            // if we started a new beat then populate the notes and beat item we need
            if (measure.beats[beatItemIndex].beatStart == currentBeat)
            {
                beat = measure.beats[beatItemIndex];
                beatItemIndex = Math.Min(beatItemIndex + 1, measure.beats.Count);
                beatNoteIndex = -1;
                NextBeatItem();
            }
        }

        /// <summary>
        /// Advances the beat item to the next note in the beat.
        /// If all the notes in the beat have been played then this method will do nothing.
        /// </summary>
        public void NextBeatItem()
        {
            beatNoteIndex = Math.Min(beatNoteIndex + 1, beat.items.Count);

            if (beatNoteIndex < beat.items.Count)
            {
                remainingNotes.Clear();
                allNotes.Clear();

                beatNote = beat.items[beatNoteIndex].item;
                AbcToMidi.Convert(beatNote as ABC.Duration, allNotes);

                foreach (var note in allNotes)
                    remainingNotes.Add(note);
            }
        }

        public bool isReadyForNextBeat { get { return remainingNotes.Count == 0 && beatNoteIndex == beat.items.Count; } }
    }
}