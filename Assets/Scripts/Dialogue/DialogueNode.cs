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
            Undo.RecordObject(this, "Undo Update Node Speaker");
            speaker = newSpeaker;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText) {
            Undo.RecordObject(this, "Undo Update Node Text");
            text = newText;
            EditorUtility.SetDirty(this);
        }

        public void SetRect(Rect newRect) {
            Undo.RecordObject(this, "Undo Update Node Rect");
            rect = newRect;
            EditorUtility.SetDirty(this);
        }

        public void SetRoot(bool state) {
            isRoot = state;
        }

        public void AddChild(string childID) {
            Undo.RecordObject(this, "Undo Node Add Child");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID) {
            Undo.RecordObject(this, "Undo Node Remove Child");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }

        public void SetSprite(Sprite newSprite) {
            Undo.RecordObject(this, "Undo Update Node Sprite");
            sprite = newSprite;
            EditorUtility.SetDirty(this);
        }

        public void SetCharacterName(string newName) {
            Undo.RecordObject(this, "Undo Update Node Character Name");
            characterName = newName;
            EditorUtility.SetDirty(this);
        }

        public void SetChained(bool state) {
            Undo.RecordObject(this, "Undo Update Node Chained State");
            isChained = state;
            EditorUtility.SetDirty(this);
        }

        public void SetCustom(bool state) {
            Undo.RecordObject(this, "Undo Update Node Custom State");
            isCustom = state;
            EditorUtility.SetDirty(this);
        }

        public void SetAction(ActionType newAction) {
            Undo.RecordObject(this, "Undo Update Node Action");
            action = newAction;
            EditorUtility.SetDirty(this);
        }

        public void SetLocationChange(LocationType newLocation) {
            Undo.RecordObject(this, "Undo Update Node Location");
            location = newLocation;
            EditorUtility.SetDirty(this);
        }

        public void SetStatValues(int[] newValues) {
            Undo.RecordObject(this, "Undo Update Node Stat Values");

            money = newValues[0];
            knowledge = newValues[1];
            glory = newValues[2];
            faith = newValues[3];

            EditorUtility.SetDirty(this);
        }
#endif

        public void SetCleared(bool state) {
            isCleared = state;
        }
    }
}
