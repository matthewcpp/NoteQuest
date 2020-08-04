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
        [Serializable]
        public class Settings
        {
            public string serverEndpoint;
            public string secretToken;
        }
        
        private readonly Settings settings;
        private readonly ItemList itemList;
        
        public NoteQuestServerSource(string settingsPath, ItemList itemList)
        {
            this.settings = JsonUtility.FromJson<NoteQuestServerSource.Settings>(File.ReadAllText(settingsPath));
            this.itemList = itemList;
        }

        [Serializable]
        class DirectoryListing
        {
            public string[] files;
            public string[] directories;
        }
        
        public IEnumerator ListDirectory(string path)
        {
            itemList.Clear();
            var urlPath = UnityWebRequest.EscapeURL(path);
            using (UnityWebRequest webRequest = UnityWebRequest.Get($"{settings.serverEndpoint}/directory?path={urlPath}"))
            {
                webRequest.SetRequestHeader("Authorization", $"Token {settings.secretToken}");
                yield return webRequest.SendWebRequest();

                if (webRequest.responseCode == 200)
                {
                    var directoryListing = JsonUtility.FromJson<DirectoryListing>(webRequest.downloadHandler.text);

                    foreach (var directory in directoryListing.directories)
                        itemList.AddItem(ItemType.Directory, directory);

                    foreach (var file in directoryListing.files)
                        itemList.AddItem(ItemType.File, file);
                }
                else
                {
                    Debug.Log($"Request Error {webRequest.responseCode}: ${webRequest.downloadHandler.text}");
                }
            }
        }
        
        public IEnumerator GetFileContents(string path, FilePickedEvent callbacks)
        {
            var urlPath = UnityWebRequest.EscapeURL(path);
            using (UnityWebRequest webRequest = UnityWebRequest.Get($"http://localhost:3000/file?path={urlPath}"))
            {
                webRequest.SetRequestHeader("Authorization", "Token test");
                yield return webRequest.SendWebRequest();

                if (webRequest.responseCode == 200)
                    callbacks?.Invoke(path, webRequest.downloadHandler.text);
                else
                    Debug.Log($"Request Error {webRequest.responseCode}: ${webRequest.downloadHandler.text}");
            }
        }
    }
}
