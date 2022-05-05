using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PLAGUEV.Dialogue {

    public class DialogueNode : ScriptableObject {

        // general
        [SerializeField] SpeakerType speaker;
        [SerializeField] string text;
        [SerializeField] List<string> children = new List<string>();
        [SerializeField] Rect rect = new Rect(0, 0, 300, 250);
        bool isRoot = false;

        // card node settings
        [SerializeField] Sprite sprite = null;
        [SerializeField] string characterName;
        [SerializeField] bool isChained = false;
        [SerializeField] bool isCustom = false;
        [SerializeField] bool isCleared = false;    // implement
        // set timer

        // player node settings
        [SerializeField] ActionType action;
        [SerializeField] LocationType location;
        [Range(-100, 100)] [SerializeField] int money = 0;
        [Range(-100, 100)] [SerializeField] int knowledge = 0;
        [Range(-100, 100)] [SerializeField] int glory = 0;
        [Range(-100, 100)] [SerializeField] int faith = 0;


        public SpeakerType GetSpeaker() {
            return speaker;
        }
    
        public string GetText() {
            return text;
        }

        public List<string> GetChildren() {
            return children;
        }

        public Rect GetRect() {
            return rect;
        }

        public bool GetRootState() {
            return isRoot;
        }

        public Sprite GetSprite() {
            return sprite;
        }

        public string GetCharacterName() {
            return characterName;
        }

        public bool GetChainedState() {
            return isChained;
        }

        public bool GetCustomState() {
            return isCustom;
        }

        public bool GetClearedState() {
            return isCleared;
        }

        public ActionType GetAction() {
            return action;
        }

        public LocationType GetLocationChange() {
            return location;
        }

        public int[] GetStatValues() {
            return new int[] {money, knowledge, glory, faith};
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

        public void SetRoot(bool state) {
            isRoot = state;
        }

        public void AddChild(string childID) {
            children.Add(childID);
        }

        public void RemoveChild(string childID) {
            children.Remove(childID);
        }

        public void SetSprite(Sprite newSprite) {
            sprite = newSprite;
        }

        public void SetCharacterName(string newName) {
            characterName = newName;
        }

        public void SetChained(bool state) {
            isChained = state;
        }

        public void SetCustom(bool state) {
            isCustom = state;
        }

        public void SetAction(ActionType newAction) {
            action = newAction;
        }

        public void SetLocationChange(LocationType newLocation) {
            location = newLocation;
        }

        public void SetStatValues(int[] newValues) {
            money = newValues[0];
            knowledge = newValues[1];
            glory = newValues[2];
            faith = newValues[3];
        }
#endif

        public void SetCleared(bool state) {
            isCleared = state;
        }
    }
}
