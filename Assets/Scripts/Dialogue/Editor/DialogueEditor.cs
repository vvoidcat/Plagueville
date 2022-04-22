using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace PLAGUEV.Dialogue.Editor {

    public class DialogueEditor : EditorWindow {

        DialogueTree selectedDialogue = null;
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
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void OnGUI() {
            DialogueGUI.SetGUIStyles();

            if (selectedDialogue == null) {
                EditorGUILayout.LabelField("dialogue selected: N/A", EditorStyles.boldLabel);
            } else {
                DrawDialogueSettings();

                foreach (DialogueNode node in selectedDialogue.GetAllNodes()) {
                    DrawNode(node);
                }
            }
        }


        private void DrawDialogueSettings() {
            EditorGUILayout.LabelField("dialogue selected: " + selectedDialogue.name, EditorStyles.boldLabel);

            GUILayout.BeginVertical();
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

            GUILayout.EndVertical();
        }

        private void DrawNode(DialogueNode node) {
            DialogueGUI.SetNodeStyle(node.GetSpeaker());

            GUILayout.BeginArea(node.GetRect(), DialogueGUI.GetNodeStyle());

            // if node == root
            // DrawRootNode(node);

            EditorGUI.BeginChangeCheck();

            SpeakerType newSpeaker = (SpeakerType)EditorGUILayout.EnumPopup(node.GetSpeaker());
            string newText = EditorGUILayout.TextField(node.GetText(), DialogueGUI.GetTextStyle(), GUILayout.Height(100));

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(selectedDialogue, "Update Node Settings");

                node.SetText(newText);
                node.SetSpeaker(newSpeaker);
            }

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