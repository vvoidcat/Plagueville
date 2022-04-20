using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PLAGUEV.Dialogue {

    [System.Serializable]
    public class DialogueNode { //: ScriptableObject {

        [SerializeField] SpeakerType speaker;
        [SerializeField] string uniqueID;
        [SerializeField] string text;
        [SerializeField] string[] children;
        [SerializeField] Rect rect = new Rect(0, 0, 300, 250);
        bool isRoot = false;


        public SpeakerType GetSpeaker() {
            return speaker;
        }

        public string GetText() {
            return text;
        }

        public Rect GetRect() {
            return rect;
        }

        public bool IsRootNode() {
            return isRoot;
        }


#if UNITY_EDITOR
        public void SetSpeaker(SpeakerType newSpeaker) {
            speaker = newSpeaker;
        }

        public void SetText(string newText) {
            text = newText;
        }

        public void SetRect(Rect newRect) {
            rect = newRect;
        }

        public void SetRootNode(bool state) {
            isRoot = state;
        }
#endif

    }
}
