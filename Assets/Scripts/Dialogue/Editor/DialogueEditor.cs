using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace PLAGUEV.Dialogue.Editor {

    public class DialogueEditor : EditorWindow {

        DialogueTree selectedDialogue = null;

        [NonSerialized] GUIStyle style;
        [NonSerialized] GUIStyle cardNodeStyle;
        [NonSerialized] GUIStyle playerNodeStyle;
        [NonSerialized] GUIStyle rootLabelStyle;
        [NonSerialized] GUIStyle textStyle;

        const float backgroundTextureSize = 100f;


        [MenuItem("Window/Dialogue Editor")]
        public static void OpenEditorWindow() {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenDialogueAsset(int instanceID, int line) {
            bool isDialogueAsset = false;

            DialogueTree dialogue = EditorUtility.InstanceIDToObject(instanceID) as DialogueTree;

            if (dialogue != null) {
                isDialogueAsset = true;
                OpenEditorWindow();
            }
            
            return isDialogueAsset;
        }

        private void OnSelectionChanged() {
            DialogueTree newDialogue = Selection.activeObject as DialogueTree;

            if (newDialogue != null) {
                selectedDialogue = newDialogue;
                ResetNodes();
                Repaint();
            }
        }


        private void OnEnable() {
            //SetGUIStyles();
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void OnGUI() {
            SetGUIStyles();

            if (selectedDialogue == null) {
                EditorGUILayout.LabelField("dialogue selected: N/A", EditorStyles.boldLabel);
            } else {
                DrawDialogueSettings();

                foreach (DialogueNode node in selectedDialogue.GetAllNodes()) {
                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.LabelField("node:");
                    string newText = EditorGUILayout.TextField(node.GetText());
                    SpeakerType newSpeaker = (SpeakerType)EditorGUILayout.EnumPopup(node.GetSpeaker());

                    if (EditorGUI.EndChangeCheck()) {
                        Undo.RecordObject(selectedDialogue, "Update Dialogue Text");

                        node.SetText(newText);
                        node.SetSpeaker(newSpeaker);
                    }
                }
            }
        }


        private void SetGUIStyles() {           // move ui methods to a separate static class?
            style = new GUIStyle();

            // card nodes
            cardNodeStyle = new GUIStyle();
            cardNodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            cardNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            cardNodeStyle.border = new RectOffset(12, 12, 12, 12);

            // player nodes
            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);

            // <START> node
            rootLabelStyle = new GUIStyle(GUI.skin.label);
            rootLabelStyle.alignment = TextAnchor.MiddleCenter;
            rootLabelStyle.fontStyle = FontStyle.Bold;

            // text blocks
            textStyle = new GUIStyle(GUI.skin.textField);
            textStyle.clipping = TextClipping.Clip;
            textStyle.wordWrap = true;
            textStyle.padding = new RectOffset(8, 8, 8, 8);
        }

        private void SetNodeStyle(SpeakerType speaker) {
            if (speaker == SpeakerType.PLAYER) {
                style = playerNodeStyle;
            } else {
                style = cardNodeStyle;
            }
        }


        private void DrawDialogueSettings() {
            EditorGUILayout.LabelField("dialogue selected: " + selectedDialogue.name, EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();

            bool newState = EditorGUILayout.Toggle("Is Plot", selectedDialogue.IsPlot());
            string newName = "";

            if (!newState) {
                newName = EditorGUILayout.TextField("Character Name", selectedDialogue.GetCharacterName());
            } else {
                // display customizable speaker name and assignable card sprite for every node
            }

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(selectedDialogue, "Undo Update Dialogue Settings");
                selectedDialogue.SetPlotRelation(newState);
                selectedDialogue.SetCharacterName(newName);
            }

            GUILayout.EndHorizontal();
        }

        private void DrawNode(DialogueNode node) {
            GUILayout.BeginArea(node.GetRect(), style);

            // if node == root
            // DrawRootNode(node);

            GUILayout.EndArea();
        }

        private void DrawRootNode(DialogueNode node) {

        }




        private void ResetNodes() {

        }




        private void ProcessEvents() {

            // if clicked on node = change selection to node
            // if clicked on bg = change selection to dialogue

            // Selection.activeObject = 
        }
    }
}