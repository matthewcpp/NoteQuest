using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace NoteQuest
{
    public class PickerController : MonoBehaviour
    {
        [SerializeField] FileSystemScorePicker filePicker;
        [SerializeField] ScoreModeUI scoreModeUI;

        void Start()
        {
            filePicker.selection += OnFileSelection;
            ShowPicker(true); // temp
        }

        public void ShowPicker(bool show)
        {
            filePicker.gameObject.SetActive(show);
        }


        void OnFileSelection(string path)
        {
            filePicker.gameObject.SetActive(false);
            try
            {
                scoreModeUI.gameObject.SetActive(true);
                scoreModeUI.ShowScore(true);
                scoreModeUI.scoreMode.layout.LoadFile(path);

                this.gameObject.SetActive(false);
            }
            catch (FileNotFoundException)
            {
                Debug.Log($"Unable to load file: ${path}");
            }
        }
    }

}
