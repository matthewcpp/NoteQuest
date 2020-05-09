using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Dynamic;

namespace NoteQuest
{
    class VoiceStatus
    {
        public ABCUnity.BeatAlignment alignment { get; }

        private ABCUnity.BeatAlignment.MeasureInfo measure;

        private ABCUnity.BeatAlignment.BeatItem beat;
        public int beatItemIndex { get; private set; } = 0;


        /// Current note that needs to be pressed for this voice
        public ABC.Item beatNote { get; private set; }
        public int beatNoteIndex { get; set; } = 0;
        /// Remaining midi notes that need to be pressed for this voice
        public HashSet<int> remainingNotes { get; } = new HashSet<int>();
        public List<int> allNotes { get; } = new List<int>();


        public VoiceStatus(ABCUnity.BeatAlignment alignment)
        {
            this.alignment = alignment;
        }

        public void NextMeasure(int measureIndex)
        {
            beatItemIndex = 0;
            measure = alignment.measures[measureIndex];
            beat = measure.beatItems[beatItemIndex];
            NextBeat(1);
        }

        /// <summary>
        /// Advances to the next beat in the measure.  Note that a voice may not have an item for each beat
        /// Beats that do not have any items will be ignored and the current beat object will be null
        /// </summary>
        public void NextBeat(int currentBeat)
        {
            if (beatItemIndex == measure.beatItems.Count)
                return;

            // if we started a new beat then populate the notes and beat item we need
            if (measure.beatItems[beatItemIndex].beatStart == currentBeat)
            {
                beat = measure.beatItems[beatItemIndex];
                beatItemIndex = Math.Min(beatItemIndex + 1, measure.beatItems.Count);
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

                beatNote = beat.items[beatNoteIndex];
                AbcToMidi.Convert(beatNote as ABC.Duration, allNotes);

                foreach (var note in allNotes)
                    remainingNotes.Add(note);
            }
        }

        public bool isRest { get { return beatNote != null && beatNote.type == ABC.Item.Type.Rest; } }
        public bool isReadyForNextBeat { get { return remainingNotes.Count == 0 && beatNoteIndex == beat.items.Count; } }
    }
}