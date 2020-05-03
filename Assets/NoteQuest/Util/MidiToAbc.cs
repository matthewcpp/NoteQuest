using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoteQuest
{
    public struct NoteInfo
    {
        public ABC.Pitch pitch;
        public ABC.Accidental accidental;

        public NoteInfo(ABC.Pitch pitch, ABC.Accidental accidental)
        {
            this.pitch = pitch;
            this.accidental = accidental;
        }

        public static bool AreEquivalent(NoteInfo info, ABC.Note note)
        {
            return false;
        }
    }

    public static class MidiToABC
    {   
        public static NoteInfo Convert(int note)
        {
            switch(note)
            {
                case 21:
                    return new NoteInfo(ABC.Pitch.A0, ABC.Accidental.Unspecified);
                case 22:
                    return new NoteInfo(ABC.Pitch.A0, ABC.Accidental.Sharp);
                case 23:
                    return new NoteInfo(ABC.Pitch.B0, ABC.Accidental.Unspecified);


                case 24:
                    return new NoteInfo(ABC.Pitch.C1, ABC.Accidental.Unspecified);
                case 25:
                    return new NoteInfo(ABC.Pitch.C1, ABC.Accidental.Sharp);
                case 26:
                    return new NoteInfo(ABC.Pitch.D1, ABC.Accidental.Unspecified);
                case 27:
                    return new NoteInfo(ABC.Pitch.D1, ABC.Accidental.Sharp);
                case 28:
                    return new NoteInfo(ABC.Pitch.E1, ABC.Accidental.Unspecified);
                case 29:
                    return new NoteInfo(ABC.Pitch.F1, ABC.Accidental.Unspecified);
                case 30:
                    return new NoteInfo(ABC.Pitch.F1, ABC.Accidental.Sharp);
                case 31:
                    return new NoteInfo(ABC.Pitch.G1, ABC.Accidental.Unspecified);
                case 32:
                    return new NoteInfo(ABC.Pitch.G1, ABC.Accidental.Sharp);
                case 33:
                    return new NoteInfo(ABC.Pitch.A1, ABC.Accidental.Unspecified);
                case 34:
                    return new NoteInfo(ABC.Pitch.A1, ABC.Accidental.Sharp);
                case 35:
                    return new NoteInfo(ABC.Pitch.B1, ABC.Accidental.Unspecified);


                case 36:
                    return new NoteInfo(ABC.Pitch.C2, ABC.Accidental.Unspecified);
                case 37:
                    return new NoteInfo(ABC.Pitch.C2, ABC.Accidental.Sharp);
                case 38:
                    return new NoteInfo(ABC.Pitch.D2, ABC.Accidental.Unspecified);
                case 39:
                    return new NoteInfo(ABC.Pitch.D2, ABC.Accidental.Sharp);
                case 40:
                    return new NoteInfo(ABC.Pitch.E2, ABC.Accidental.Unspecified);
                case 41:
                    return new NoteInfo(ABC.Pitch.F2, ABC.Accidental.Unspecified);
                case 42:
                    return new NoteInfo(ABC.Pitch.F2, ABC.Accidental.Sharp);
                case 43:
                    return new NoteInfo(ABC.Pitch.G2, ABC.Accidental.Unspecified);
                case 44:
                    return new NoteInfo(ABC.Pitch.G2, ABC.Accidental.Sharp);
                case 45:
                    return new NoteInfo(ABC.Pitch.A2, ABC.Accidental.Unspecified);
                case 46:
                    return new NoteInfo(ABC.Pitch.A2, ABC.Accidental.Sharp);
                case 47:
                    return new NoteInfo(ABC.Pitch.B2, ABC.Accidental.Unspecified);


                case 48:
                    return new NoteInfo(ABC.Pitch.C3, ABC.Accidental.Unspecified);
                case 49:
                    return new NoteInfo(ABC.Pitch.C3, ABC.Accidental.Sharp);
                case 50:
                    return new NoteInfo(ABC.Pitch.D3, ABC.Accidental.Unspecified);
                case 51:
                    return new NoteInfo(ABC.Pitch.D3, ABC.Accidental.Sharp);
                case 52:
                    return new NoteInfo(ABC.Pitch.E3, ABC.Accidental.Unspecified);
                case 53:
                    return new NoteInfo(ABC.Pitch.F3, ABC.Accidental.Unspecified);
                case 54:
                    return new NoteInfo(ABC.Pitch.F3, ABC.Accidental.Sharp);
                case 55:
                    return new NoteInfo(ABC.Pitch.G3, ABC.Accidental.Unspecified);
                case 56:
                    return new NoteInfo(ABC.Pitch.G3, ABC.Accidental.Sharp);
                case 57:
                    return new NoteInfo(ABC.Pitch.A3, ABC.Accidental.Unspecified);
                case 58:
                    return new NoteInfo(ABC.Pitch.A3, ABC.Accidental.Sharp);
                case 59:
                    return new NoteInfo(ABC.Pitch.B3, ABC.Accidental.Unspecified);


                case 60:
                    return new NoteInfo(ABC.Pitch.C4, ABC.Accidental.Unspecified);
                case 61:
                    return new NoteInfo(ABC.Pitch.C4, ABC.Accidental.Sharp);
                case 62:
                    return new NoteInfo(ABC.Pitch.D4, ABC.Accidental.Unspecified);
                case 63:
                    return new NoteInfo(ABC.Pitch.D4, ABC.Accidental.Sharp);
                case 64:
                    return new NoteInfo(ABC.Pitch.E4, ABC.Accidental.Unspecified);
                case 65:
                    return new NoteInfo(ABC.Pitch.F4, ABC.Accidental.Unspecified);
                case 66:
                    return new NoteInfo(ABC.Pitch.F4, ABC.Accidental.Sharp);
                case 67:
                    return new NoteInfo(ABC.Pitch.G4, ABC.Accidental.Unspecified);
                case 68:
                    return new NoteInfo(ABC.Pitch.G4, ABC.Accidental.Sharp);
                case 69:
                    return new NoteInfo(ABC.Pitch.A4, ABC.Accidental.Unspecified);
                case 70:
                    return new NoteInfo(ABC.Pitch.A4, ABC.Accidental.Sharp);
                case 71:
                    return new NoteInfo(ABC.Pitch.B4, ABC.Accidental.Unspecified);


                case 72:
                    return new NoteInfo(ABC.Pitch.C5, ABC.Accidental.Unspecified);
                case 73:
                    return new NoteInfo(ABC.Pitch.C5, ABC.Accidental.Sharp);
                case 74:
                    return new NoteInfo(ABC.Pitch.D5, ABC.Accidental.Unspecified);
                case 75:
                    return new NoteInfo(ABC.Pitch.D5, ABC.Accidental.Sharp);
                case 76:
                    return new NoteInfo(ABC.Pitch.E5, ABC.Accidental.Unspecified);
                case 77:
                    return new NoteInfo(ABC.Pitch.F5, ABC.Accidental.Unspecified);
                case 78:
                    return new NoteInfo(ABC.Pitch.F5, ABC.Accidental.Sharp);
                case 79:
                    return new NoteInfo(ABC.Pitch.G5, ABC.Accidental.Unspecified);
                case 80:
                    return new NoteInfo(ABC.Pitch.G5, ABC.Accidental.Sharp);
                case 81:
                    return new NoteInfo(ABC.Pitch.A5, ABC.Accidental.Unspecified);
                case 82:
                    return new NoteInfo(ABC.Pitch.A5, ABC.Accidental.Sharp);
                case 83:
                    return new NoteInfo(ABC.Pitch.B5, ABC.Accidental.Unspecified);


                case 84:
                    return new NoteInfo(ABC.Pitch.C6, ABC.Accidental.Unspecified);
                case 85:
                    return new NoteInfo(ABC.Pitch.C6, ABC.Accidental.Sharp);
                case 86:
                    return new NoteInfo(ABC.Pitch.D6, ABC.Accidental.Unspecified);
                case 87:
                    return new NoteInfo(ABC.Pitch.D6, ABC.Accidental.Sharp);
                case 88:
                    return new NoteInfo(ABC.Pitch.E6, ABC.Accidental.Unspecified);
                case 89:
                    return new NoteInfo(ABC.Pitch.F6, ABC.Accidental.Unspecified);
                case 90:
                    return new NoteInfo(ABC.Pitch.F6, ABC.Accidental.Sharp);
                case 91:
                    return new NoteInfo(ABC.Pitch.G6, ABC.Accidental.Unspecified);
                case 92:
                    return new NoteInfo(ABC.Pitch.G6, ABC.Accidental.Sharp);
                case 93:
                    return new NoteInfo(ABC.Pitch.A6, ABC.Accidental.Unspecified);
                case 94:
                    return new NoteInfo(ABC.Pitch.A6, ABC.Accidental.Sharp);
                case 95:
                    return new NoteInfo(ABC.Pitch.B6, ABC.Accidental.Unspecified);



                case 96:
                    return new NoteInfo(ABC.Pitch.C7, ABC.Accidental.Unspecified);
                case 97:
                    return new NoteInfo(ABC.Pitch.C7, ABC.Accidental.Sharp);
                case 98:
                    return new NoteInfo(ABC.Pitch.D7, ABC.Accidental.Unspecified);
                case 99:
                    return new NoteInfo(ABC.Pitch.D7, ABC.Accidental.Sharp);
                case 100:
                    return new NoteInfo(ABC.Pitch.E7, ABC.Accidental.Unspecified);
                case 101:
                    return new NoteInfo(ABC.Pitch.F7, ABC.Accidental.Unspecified);
                case 102:
                    return new NoteInfo(ABC.Pitch.F7, ABC.Accidental.Sharp);
                case 103:
                    return new NoteInfo(ABC.Pitch.G7, ABC.Accidental.Unspecified);
                case 104:
                    return new NoteInfo(ABC.Pitch.G7, ABC.Accidental.Sharp);
                case 105:
                    return new NoteInfo(ABC.Pitch.A7, ABC.Accidental.Unspecified);
                case 106:
                    return new NoteInfo(ABC.Pitch.A7, ABC.Accidental.Sharp);
                case 107:
                    return new NoteInfo(ABC.Pitch.B7, ABC.Accidental.Unspecified);
                case 108:
                    return new NoteInfo(ABC.Pitch.C8, ABC.Accidental.Unspecified);

            }

            throw new NoteQuest.Exception($"Unknown Midi note: {note}");
        }
    }
}