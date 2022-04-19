using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PLAGUEV.Dialogue {

    public class DialogueNode : ScriptableObject {

        [SerializeField] SpeakerType speaker;
        [SerializeField] string uniqueID;
        [SerializeField] string text;
        [SerializeField] string[] children;
        bool isRoot = false;
    }
}
