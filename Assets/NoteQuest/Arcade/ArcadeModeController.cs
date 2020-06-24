using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NoteQuest
{
    public class ArcadeModeController : MonoBehaviour
    {
        [SerializeField] GameObject arcadeModePrefab;
        [SerializeField] GameObject arcadeModeUIPrefab;

        ArcadeMode arcadeMode;
        GameObject arcadeModeUI;


        EzMidi.Connection midi;
        void Start()
        {
            arcadeMode = GameObject.Instantiate(arcadeModePrefab, this.transform).GetComponent<ArcadeMode>();
            arcadeModeUI = GameObject.Instantiate(arcadeModeUIPrefab, this.transform);
        }
    }

}

