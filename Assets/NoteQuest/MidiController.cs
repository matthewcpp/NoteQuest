using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoteQuest
{
    public class MidiController : MonoBehaviour
    {
        public EzMidi.Connection midiConnection;
        public TsfUnity.TinySoundFont tsf;

        public int soundfontPreset { get; set; } = 0;

        private void Start()
        {
            midiConnection.NoteOn += MidiNoteOn;
            midiConnection.NoteOff += MidiNoteOff;

            midiConnection.ConnectSource(0);
        }

        void MidiNoteOn(int channel, int note, int velocity)
        {
            tsf.soundfont.NoteOn(soundfontPreset, note, velocity / 127.0f);
        }

        void MidiNoteOff(int channel, int note, int velocity)
        {
            tsf.soundfont.NoteOff(soundfontPreset, note);
        }
    }
}
