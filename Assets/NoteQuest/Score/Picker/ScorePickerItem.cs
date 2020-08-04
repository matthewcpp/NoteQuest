using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NoteQuest.ScorePicker
{
    public class ScorePickerItem : MonoBehaviour
    {

        public ItemType type;

        public string text {
            get { return GetComponentInChildren<TextMeshProUGUI>().text; } 
            set { GetComponentInChildren<TextMeshProUGUI>().text = value; } 
        }
    }

}
