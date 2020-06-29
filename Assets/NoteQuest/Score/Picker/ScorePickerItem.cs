using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NoteQuest
{
    public class ScorePickerItem : MonoBehaviour
    {
        public enum Type
        {
            Directory, File
        }

        public Type type;

        public string text {
            get { return GetComponentInChildren<TextMeshProUGUI>().text; } 
            set { GetComponentInChildren<TextMeshProUGUI>().text = value; } 
        }
    }

}
