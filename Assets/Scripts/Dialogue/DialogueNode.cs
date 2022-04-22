using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PLAGUEV.Dialogue {

    [System.Serializable]
    public class DialogueNode { //: ScriptableObject {

        [SerializeField] SpeakerType speaker;
        [SerializeField] string characterName;
        [SerializeField] string uniqueID;
        [SerializeField] string text;
        [SerializeField] string[] children;
        [SerializeField] Rect rect = new Rect(0, 0, 300, 250);
        [SerializeField] Sprite sprite = null;

        // stats
        // bool isNegative for each

        [SerializeField] bool isCleared = false;    // implement
        [SerializeField] bool isChained;
        [SerializeField] bool isChainStarter = false;

        bool isRoot = false;


        public SpeakerType GetSpeaker() {
            return speaker;
        }

        public string GetCharacterName() {
            return characterName;
        }

        public string GetText() {
            return text;
        }

        public Sprite GetSprite() {
            return sprite;
        }

        public Rect GetRect() {
            return rect;
        }

        public bool IsCleared() {
            return isCleared;
        }

        public bool IsChained() {
            return isChained;
        }

        public bool IsChainStarter() {
            return isChainStarter;
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
