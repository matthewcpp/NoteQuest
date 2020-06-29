using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace NoteQuest.ScorePicker
{
    public delegate void FilePickedEvent(string path, string contents);

    interface Source
    {
        IEnumerator ListDirectory(string path);
        IEnumerator GetFileContents(string path, FilePickedEvent callbacks);
    }

    class FileSystemSource : Source
    {
        private readonly ItemList itemList;
        private readonly string basePath;

        public FileSystemSource(ItemList itemList)
        {
            this.itemList = itemList;
            basePath = Path.Combine(Application.streamingAssetsPath, "Scores");
        }
        
        public IEnumerator ListDirectory(string path)
        {
            itemList.Clear();
            var directoryPath = Path.Combine(basePath, path);
            foreach (var directory in Directory.EnumerateDirectories(directoryPath))
            {
                var directoryName = directory.Substring(directory.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                if (directoryName[0] == '.') continue;
                itemList.AddItem(ItemType.Directory, directoryName);
            }

            foreach (var file in Directory.EnumerateFiles(directoryPath, "*.abc"))
            {
                var fileName = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                if (fileName[0] == '.') continue;
                itemList.AddItem(ItemType.File, fileName);
            }
            
            yield return null;
        }

        public IEnumerator GetFileContents(string path, FilePickedEvent callbacks)
        {
            var filePath = Path.Combine(basePath, path);
            var contents = File.ReadAllText(filePath);
            callbacks?.Invoke(path, contents);
            yield return null;
        }
    }

    class NoteQuestServerSource : Source
    {
        private ItemList itemList;
        
        public NoteQuestServerSource(ItemList itemList)
        {
            this.itemList = itemList;
        }

        [Serializable]
        class DirectoryListing
        {
            public string path;
            public string[] files;
            public string[] directories;
        }
        
        public IEnumerator ListDirectory(string path)
        {
            itemList.Clear();
            var url = UnityWebRequest.EscapeURL($"http://localhost:3000/directory?path={path}");
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();
                var directoryListing = JsonUtility.FromJson<DirectoryListing>(webRequest.downloadHandler.text);
            }
        }
        
        public IEnumerator GetFileContents(string path, FilePickedEvent callbacks)
        {
            
            yield return null;
        }
    }
}
