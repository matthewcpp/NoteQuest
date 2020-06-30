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

        private static readonly Color activeColor = new Color(1.0f, 0.78f, 0.01f, 1.0f);

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

            var directories = currentDirectory.Split(Path.PathSeparator);
            if (directories.Length > 1)
            {
                var parts = new string[directories.Length - 1];
                Array.Copy(directories, parts, parts.Length);
                currentDirectory = string.Join(new string(Path.PathSeparator, 1), parts);
            }
            else
                currentDirectory = string.Empty;

            ListCurrentDirectory();
        }

        public void NavigateHome()
        {
            currentDirectory = string.Empty;
            ListCurrentDirectory();
        }

        public void SetFileSystemSource()
        {
            var fileSystem = this.transform.Find("FileSystemSource").GetComponent<Image>();
            if (fileSystem.color == activeColor)
                return;
            
            fileSystem.color = activeColor;
            
            var server = this.transform.Find("ServerSource").GetComponent<Image>();
            server.color = Color.black;
            
            source = new FileSystemSource(this.itemList);
            NavigateHome();
        }

        public void SetServerSource()
        {
            var server = this.transform.Find("ServerSource").GetComponent<Image>();
            if (server.color == activeColor)
                return;
            
            server.color = activeColor;
            
            var fileSystem = this.transform.Find("FileSystemSource").GetComponent<Image>();
            fileSystem.color = Color.black;
            
            source = new NoteQuestServerSource(this.itemList);
            NavigateHome();
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


