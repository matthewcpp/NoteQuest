using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

namespace NoteQuest
{
    public class FileSystemScorePicker : MonoBehaviour
    {
        [SerializeField] GameObject directoryItemPrefab;
        [SerializeField] GameObject fileItemPrefab;

        const string rootDirectory = "D:/temp/Scores";

        private Transform items;
        private TextMeshProUGUI currentPathText;

        public string currentDirectory { get; private set; }

        List<ScorePickerItem> directoryItemPool = new List<ScorePickerItem>();
        List<ScorePickerItem> fileItemPool = new List<ScorePickerItem>();

        List<ScorePickerItem> activeItems = new List<ScorePickerItem>();

        public delegate void Selection(string path);
        public event Selection selection;

        void Awake()
        {
            items = this.transform.Find("Items");
            currentPathText = GetComponentInChildren<TextMeshProUGUI>();
        }

        void Start()
        {
            NavigateHome();
        }

        void ListCurrentDirectory()
        {
            currentPathText.text = currentDirectory;
            
            Clear();

            foreach (var directory in Directory.EnumerateDirectories(currentDirectory))
                AddItem(ScorePickerItem.Type.Directory, directory.Substring(directory.LastIndexOf(Path.DirectorySeparatorChar) + 1));

            foreach (var file in Directory.EnumerateFiles(currentDirectory, "*.abc"))
                AddItem(ScorePickerItem.Type.File, file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1));
        }

        public void NavigateUp()
        {
            if (currentDirectory == rootDirectory)
                return;

            currentDirectory = Path.GetFullPath(Path.Combine(currentDirectory, ".."));
            ListCurrentDirectory();
        }

        public void NavigateHome()
        {
            currentDirectory = rootDirectory;
            ListCurrentDirectory();
        }

        private void Clear()
        {
            foreach (var item in activeItems)
            {
                item.gameObject.SetActive(false);

                if (item.type == ScorePickerItem.Type.Directory)
                    directoryItemPool.Add(item);
                else
                    fileItemPool.Add(item);
            }

            activeItems.Clear();
        }

        void AddItem(ScorePickerItem.Type type, string text)
        {
            var item = GetItem(type);
            item.text = text;
            activeItems.Add(item);
        }

        ScorePickerItem GetItem(ScorePickerItem.Type type)
        {
            List<ScorePickerItem> pool = null;
            GameObject prefab = null;

            if (type == ScorePickerItem.Type.Directory)
            {
                pool = directoryItemPool;
                prefab = directoryItemPrefab;
            }
            else
            {
                pool = fileItemPool;
                prefab = fileItemPrefab;
            }
            
            if (pool.Count > 0)
            {
                var item = pool[pool.Count - 1];
                pool.RemoveAt(pool.Count - 1);

                item.gameObject.SetActive(true);

                return item;
            }
            else
            {
                var itemObj = GameObject.Instantiate(prefab, items.transform);
                var pickerItem = itemObj.GetComponent<ScorePickerItem>();

                var eventTrigger = itemObj.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((data) => { OnItemClick(pickerItem); });
                eventTrigger.triggers.Add(entry);

                return pickerItem;
            }
        }

        void OnItemClick(ScorePickerItem item)
        {
            string itemPath = Path.Combine(currentDirectory, item.text);
            if (item.type == ScorePickerItem.Type.Directory)
            {
                currentDirectory = itemPath;
                ListCurrentDirectory();
            }
            else
            {
                selection?.Invoke(itemPath);
            }
        }

    }
}


