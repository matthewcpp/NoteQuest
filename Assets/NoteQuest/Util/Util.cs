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
    }
}
