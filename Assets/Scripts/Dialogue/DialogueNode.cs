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
        [SerializeField] List<string> children = new List<string>();
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

        public List<string> GetChildren() {
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

        public void SetSprite(Sprite newSprite) {
            sprite = newSprite;
        }

        public void SetCharacterName(string newName) {
            characterName = newName;
        }

        public void SetText(string newText) {
            text = newText;
        }

        public void SetChained(bool state) {
            isChained = state;
        }

        public void SetID(string id) {
            uniqueID = id;
        }

        public void SetRect(Rect newRect) {
            rect = newRect;
        }

        public void SetRootNode(bool state) {
            isRoot = state;
        }

        public void AddChild(string childID) {
            children.Add(childID);
        }

        public void RemoveChild(string childID) {
            children.Remove(childID);
        }
#endif

        public void SetCleared(bool state) {
            isCleared = state;
        }

    }
}
