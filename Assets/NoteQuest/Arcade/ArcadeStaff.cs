using ABC;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

namespace NoteQuest
{
    public class ArcadeStaff : MonoBehaviour
    {
        [SerializeField]
        SpriteAtlas spriteAtlas;

        [SerializeField]
        TextMeshPro textPrefab;

        public ABC.Clef clef;

        private class Fade
        {
            public List<SpriteRenderer> spriteRenders;
            public float elapsed = 0.0f;
            public float time;
            public float opacity = 1.0f;

            public float progress { get { return elapsed / time; } }
        }

        public ABCUnity.CustomStaff staff { get; private set; }

        public bool scrollingEnabled { get; set; } = false;
        public float staffWidth { get { return staff.staffWidth; } set { SetWidth(value); } }
        public float speed { get; set; } = 0.0f;

        public Dictionary<GameObject, Vector3> activeNotes { get; } = new Dictionary<GameObject, Vector3>();
        private Dictionary<GameObject, Fade> fadingNotes = new Dictionary<GameObject, Fade>();
        List<GameObject> completedFades = new List<GameObject>();
        private List<GameObject> notesToShred = new List<GameObject>();

        public delegate void OnShredNote(GameObject noteObj);
        public OnShredNote onShredNote;

        public float shredPos { get; set; } = 1.5f;

        public void ResetStaff()
        {
            foreach (var activeNote in activeNotes)
                GameObject.Destroy(activeNote.Key);

            activeNotes.Clear();
            fadingNotes.Clear();
            completedFades.Clear();
            notesToShred.Clear();
        }

        void Awake()
        {
            staff = new ABCUnity.CustomStaff(this.gameObject, spriteAtlas, textPrefab);
            staff.Init(clef);
            SetWidth(10.0f);
        }

        private void SetWidth(float width)
        {
            staff.staffWidth = width;
        }

        public void Update()
        {
            if (!staff.isInit) 
                return;

            if (scrollingEnabled)
                UpdateActiveNotes();

            UpdateFades();
        }

        void UpdateActiveNotes()
        {
            foreach (var activeNote in activeNotes)
            {
                var localPos = activeNote.Key.transform.localPosition;
                var delta = activeNote.Value * (speed * Time.deltaTime);
                var localPosition = localPos + delta;

                if (localPosition.x < shredPos)
                    notesToShred.Add(activeNote.Key);
                else
                    activeNote.Key.transform.localPosition = localPosition;
            }

            if (notesToShred.Count > 0)
            {
                foreach (var noteToShred in notesToShred)
                {
                    onShredNote?.Invoke(noteToShred);

                    // no fade, destroy the note immediately, otherwise will be destroyed when fade completes
                    if (!fadingNotes.ContainsKey(noteToShred))
                    {
                        activeNotes.Remove(noteToShred);
                        Destroy(noteToShred);
                    }
                }

                notesToShred.Clear();
            }
        }

        public void SetDirection(GameObject noteObj, Vector3 direction)
        {
            if (activeNotes.ContainsKey(noteObj))
                activeNotes[noteObj] = direction;
        }

        public GameObject SpawnNote(ABC.Note note, Vector3 direction)
        {
            return SpawnNote(note, direction, staff.staffWidth - 2.0f);
        }

        public GameObject SpawnNote(ABC.Note note, Vector3 direction, float pos)
        {
            var newNote = staff.AppendNote(note, pos);
            activeNotes.Add(newNote, direction);

            return newNote;
        }

        public void RemoveActiveObject(GameObject noteObj)
        {
            if (activeNotes.ContainsKey(noteObj))
            {
                activeNotes.Remove(noteObj);
                fadingNotes.Remove(noteObj);
                Destroy(noteObj);
            }
        }

        public void FadeNoteOut(GameObject noteObj, float time)
        {
            if (activeNotes.ContainsKey(noteObj) && !fadingNotes.ContainsKey(noteObj))
            {
                Fade fade = new Fade();
                fade.time = time;
                fade.spriteRenders = ABCUnity.Util.GatherSpriteRenderers(noteObj);
                fadingNotes[noteObj] = fade;
            }
        }

        private void UpdateFades()
        {
            if (fadingNotes.Count == 0)
                return;

            foreach (var item in fadingNotes)
            {
                item.Value.elapsed += Time.deltaTime;
                var opacity = 1.0f - item.Value.progress;

                if (opacity <= 0.0f)
                {
                    completedFades.Add(item.Key);
                    continue;
                }
                
                foreach(var sprite in item.Value.spriteRenders)
                {
                    var color = sprite.color;
                    color.a = opacity;
                    sprite.color = color;
                }
            }

            foreach(var item in completedFades)
            {
                activeNotes.Remove(item);
                fadingNotes.Remove(item);
                Destroy(item);
            }
        }
    }
}
