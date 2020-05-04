using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NoteQuest
{
    static class AbcToMidi
    {
        public static bool Convert(ABC.Duration item, ICollection<int> midiNotes)
        {
            midiNotes.Clear();

            switch (item.type)
            {
                case ABC.Item.Type.Note:
                    var note = item as ABC.Note;
                    midiNotes.Add(PitchToMidi(note.pitch) + AccidentalModifier(note.accidental));
                    return true;

                case ABC.Item.Type.Chord:
                    var chord = item as ABC.Chord;
                    foreach (var chordNote in chord.notes)
                        midiNotes.Add(PitchToMidi(chordNote.pitch) + AccidentalModifier(chordNote.accidental));
                    return true;

                default:
                    return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int AccidentalModifier(ABC.Accidental accidental)
        {
            switch (accidental)
            {
                case ABC.Accidental.Sharp:
                    return 1;
                case ABC.Accidental.Flat:
                    return -1;
                default:
                    return 0;
            }
        }

        static int PitchToMidi(ABC.Pitch pitch)
        {
            switch (pitch)
            {
                case ABC.Pitch.A0:
                    return 21;
                case ABC.Pitch.B0:
                    return 23;
                case ABC.Pitch.C1:
                    return 24;
                case ABC.Pitch.D1:
                    return 26;
                case ABC.Pitch.E1:
                    return 28;
                case ABC.Pitch.F1:
                    return 29;
                case ABC.Pitch.G1:
                    return 31;
                case ABC.Pitch.A1:
                    return 33;
                case ABC.Pitch.B1:
                    return 35;
                case ABC.Pitch.C2:
                    return 36;
                case ABC.Pitch.D2:
                    return 38;
                case ABC.Pitch.E2:
                    return 40;
                case ABC.Pitch.F2:
                    return 41;
                case ABC.Pitch.G2:
                    return 43;
                case ABC.Pitch.A2:
                    return 45;
                case ABC.Pitch.B2:
                    return 47;
                case ABC.Pitch.C3:
                    return 48;
                case ABC.Pitch.D3:
                    return 50;
                case ABC.Pitch.E3:
                    return 52;
                case ABC.Pitch.F3:
                    return 53;
                case ABC.Pitch.G3:
                    return 55;
                case ABC.Pitch.A3:
                    return 57;
                case ABC.Pitch.B3:
                    return 59;
                case ABC.Pitch.C4:
                    return 60;
                case ABC.Pitch.D4:
                    return 62;
                case ABC.Pitch.E4:
                    return 64;
                case ABC.Pitch.F4:
                    return 65;
                case ABC.Pitch.G4:
                    return 67;
                case ABC.Pitch.A4:
                    return 69;
                case ABC.Pitch.B4:
                    return 71;
                case ABC.Pitch.C5:
                    return 72;
                case ABC.Pitch.D5:
                    return 74;
                case ABC.Pitch.E5:
                    return 76;
                case ABC.Pitch.F5:
                    return 77;
                case ABC.Pitch.G5:
                    return 79;
                case ABC.Pitch.A5:
                    return 81;
                case ABC.Pitch.B5:
                    return 83;
                case ABC.Pitch.C6:
                    return 84;
                case ABC.Pitch.D6:
                    return 86;
                case ABC.Pitch.E6:
                    return 88;
                case ABC.Pitch.F6:
                    return 89;
                case ABC.Pitch.G6:
                    return 91;
                case ABC.Pitch.A6:
                    return 93;
                case ABC.Pitch.B6:
                    return 95;
                case ABC.Pitch.C7:
                    return 96;
                case ABC.Pitch.D7:
                    return 98;
                case ABC.Pitch.E7:
                    return 100;
                case ABC.Pitch.F7:
                    return 101;
                case ABC.Pitch.G7:
                    return 103;
                case ABC.Pitch.A7:
                    return 105;
                case ABC.Pitch.B7:
                    return 107;
                case ABC.Pitch.C8:
                    return 108;
                default:
                    throw new NoteQuest.Exception("Invalid pitch value");
            }
        }
    }
}