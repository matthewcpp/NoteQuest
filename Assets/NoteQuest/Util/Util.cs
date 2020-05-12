using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoteQuest
{
    public static class Util
    {
        public static void AdjustNote(ABC.Note note)
        {
            var pitchName = note.pitch.ToString();

            if ((pitchName[0] == 'C'  || pitchName[0] == 'F' ) && note.accidental == ABC.Accidental.Flat)
            {
                note.accidental = ABC.Accidental.Unspecified;
                note.pitch -= 1;
            }
            else if ((pitchName[0] == 'B' || pitchName[0] == 'E') && note.accidental == ABC.Accidental.Sharp)
            {
                note.accidental = ABC.Accidental.Unspecified;
                note.pitch += 1;
            }
            else if (note.pitch == ABC.Pitch.A0 && note.accidental == ABC.Accidental.Flat)
            {
                note.accidental = ABC.Accidental.Unspecified;
            }
            else if (note.pitch == ABC.Pitch.C8 && note.accidental == ABC.Accidental.Sharp)
            {
                note.accidental = ABC.Accidental.Unspecified;
            }
        }

        public static string GetTextString(ABC.Item item)
        {
            switch (item.type)
            {
                case ABC.Item.Type.Note:
                    var note = item as ABC.Note;
                    return NoteText(note.pitch, note.accidental);

                case ABC.Item.Type.Chord:
                    var chord = item as ABC.Chord;
                    string chordText = "[";

                    for (int i = 0; i < chord.notes.Length; i++)
                    {
                        if (i > 0)
                            chordText += ' ';

                        chordText += NoteText(chord.notes[i].pitch, chord.notes[i].accidental);
                    }

                    chordText += "]";

                    return chordText;

                default:
                    return string.Empty;
            }
        }

        public static string NoteText(ABC.Pitch pitch, ABC.Accidental accidental)
        {
            string text = pitch.ToString();

            if (accidental == ABC.Accidental.Sharp)
                text = "#" + text;
            else if (accidental == ABC.Accidental.Flat)
                text = "b " + text;

            return text;
        }
    }
}
