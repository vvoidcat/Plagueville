using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PLAGUEV.Dialogue {

    [System.Serializable]
    public class DialogueNode { //: ScriptableObject {

        [SerializeField] SpeakerType speaker;
        [SerializeField] Sprite sprite = null;
        [SerializeField] string characterName;
        [SerializeField] string text;
        [SerializeField] bool isChained;


        // stats
        // bool isNegative for each
        [SerializeField] bool isCleared = false;    // implement

        [SerializeField] string uniqueID;
        [SerializeField] string[] children;
        [SerializeField] Rect rect = new Rect(0, 0, 300, 250);
        bool isRoot = false;


        public SpeakerType GetSpeaker() {
            return speaker;
        }

        public Sprite GetSprite() {
            return sprite;
        }

        public string GetCharacterName() {
            return characterName;
        }

        public string GetText() {
            return text;
        }

        public bool IsChained() {
            return isChained;
        }

        public bool IsCleared() {
            return isCleared;
        }

        public string GetID() {
            return uniqueID;
        }
        
        public IEnumerable GetChildren() {
            return children;
        }

        public Rect GetRect() {
            return rect;
        }

        public bool IsRoot() {
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

        public void SetChained(bool state) {
            isChained = state;
        }

        public void SetRootNode(bool state) {
            isRoot = state;
        }
#endif

        public void SetCleared(bool state) {
            isCleared = state;
        }

    }
}
