using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

namespace NoteQuest.ScorePicker
{
    public class Picker : MonoBehaviour
    {
        private ItemList itemList;

        private Transform items;
        private TextMeshProUGUI currentPathText;
        private Source source;

        public string currentDirectory { get; private set; }
        
        public event FilePickedEvent filePicked;

        void Start()
        {
            itemList = GetComponentInChildren<ItemList>();
            itemList.onItemPick += OnItemClick;
            currentPathText = GetComponentInChildren<TextMeshProUGUI>();
            source = new FileSystemSource(itemList);
            NavigateHome();
        }

        void ListCurrentDirectory()
        {
            currentPathText.text = currentDirectory;
            StartCoroutine(source.ListDirectory(currentDirectory));
        }

        public void NavigateUp()
        {
            if (currentDirectory == string.Empty)
                return;

            currentDirectory = Path.GetFullPath(Path.Combine(currentDirectory, ".."));
            ListCurrentDirectory();
        }

        public void NavigateHome()
        {
            currentDirectory = string.Empty;
            ListCurrentDirectory();
        }

        void OnItemClick(ScorePickerItem item)
        {
            string itemPath = Path.Combine(currentDirectory, item.text);
            if (item.type == ItemType.Directory)
            {
                currentDirectory = itemPath;
                ListCurrentDirectory();
            }
            else
            {
                var filePath = Path.Combine(currentDirectory, item.text);
                StartCoroutine(source.GetFileContents(filePath, filePicked));
            }
        }

    }
}


