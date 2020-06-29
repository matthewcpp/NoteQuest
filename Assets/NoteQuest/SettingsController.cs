using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NoteQuest
{
    public class SettingsController : MonoBehaviour
    {
        private EzMidi.Connection midiConnection;
        private MidiController midiController;
        private TsfUnity.Soundfont soundfont;
        private TMP_Dropdown midiDevices;
        private TMP_Dropdown presets;

        private void Start()
        {
            midiConnection = FindObjectOfType<EzMidi.Connection>();
            midiController = FindObjectOfType<MidiController>();
            soundfont = FindObjectOfType<TsfUnity.TinySoundFont>().soundfont;

            midiDevices = this.transform.Find("MidiDevices").GetComponent<TMP_Dropdown>();
            presets = this.transform.Find("Presets").GetComponent<TMP_Dropdown>();

            RefreshMidiDevices();
            RefreshSoundFontPresets();
        }

        void RefreshMidiDevices()
        {
            var devices = midiConnection.GetSourceNames();
            var options = new List<TMP_Dropdown.OptionData>();
            
            foreach (var device in devices)
                options.Add(new TMP_Dropdown.OptionData(device));

            midiDevices.options = options;
        }

        void RefreshSoundFontPresets()
        {
            var soundfontPresets = soundfont.presets;
            var options = new List<TMP_Dropdown.OptionData>();
            
            foreach (var preset in soundfontPresets)
                options.Add(new TMP_Dropdown.OptionData(preset));

            presets.options = options;
        }

        public void OnBackButton()
        {
            this.gameObject.SetActive(false);
        }
        
        public void OnMidiSourceSelected()
        {
            var deviceName = midiDevices.options[midiDevices.value].text;
            Debug.Log($"Connecting to midi device: ${deviceName}");
            
            if (midiConnection.isConnected)
                midiConnection.DisconnectSource();
            
            midiConnection.ConnectSource(midiDevices.value);
        }

        public void OnSoundfontPresetSelected()
        {
            var presetName = presets.options[presets.value].text;
            Debug.Log($"Setting soundfont preset: {presetName}");
            
            midiController.soundfontPreset = presets.value;
        }
    }
}

