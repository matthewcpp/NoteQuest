using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NoteQuest.ScorePicker
{
    public enum ItemType
    {
        Directory, File
    }
    
    public class ItemList : MonoBehaviour
    {
        [SerializeField] GameObject directoryItemPrefab;
        [SerializeField] GameObject fileItemPrefab;
        
        List<ScorePickerItem> directoryItemPool = new List<ScorePickerItem>();
        List<ScorePickerItem> fileItemPool = new List<ScorePickerItem>();

        private Transform items;
        List<ScorePickerItem> activeItems = new List<ScorePickerItem>();
        
        public delegate void ItemPick(ScorePickerItem item);
        public event ItemPick onItemPick;
        
        void Awake()
        {
            items = this.transform;
        }
        
        public void Clear()
        {
            foreach (var item in activeItems)
            {
                item.gameObject.SetActive(false);

                if (item.type == ItemType.Directory)
                    directoryItemPool.Add(item);
                else
                    fileItemPool.Add(item);
            }

            activeItems.Clear();
        }
        
        public void AddItem(ItemType type, string text)
        {
            var item = GetItem(type);
            item.text = text;
            activeItems.Add(item);
        }
        
        ScorePickerItem GetItem(ItemType type)
        {
            List<ScorePickerItem> pool = null;
            GameObject prefab = null;

            if (type == ItemType.Directory)
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
                entry.callback.AddListener((data) => { onItemPick?.Invoke(pickerItem); });
                eventTrigger.triggers.Add(entry);

                return pickerItem;
            }
        }
    }
}

