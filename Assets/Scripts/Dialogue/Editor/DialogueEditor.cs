using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace PLAGUEV.Dialogue.Editor {

    public class DialogueEditor : EditorWindow {

        DialogueTree selectedDialogue = null;
        DialogueNode draggingNode = null;
        Vector2 draggingOffset;
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
            EditorGUI.BeginChangeCheck();

            if (selectedDialogue == null) {
                EditorGUILayout.LabelField("dialogue selected: N/A", EditorStyles.boldLabel);
            } else {
                DrawDialogueSettings();
                ProcessEvents();

                foreach (DialogueNode node in selectedDialogue.GetAllNodes()) {
                    DrawNode(node);
                }
            }
        }


        private void ProcessEvents() {
            if (Event.current.type == EventType.MouseDown && draggingNode == null) {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition);

                if (draggingNode != null) {
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                }
            } else if (Event.current.type == EventType.MouseDrag && draggingNode != null) {
                Undo.RecordObject(selectedDialogue, "Undo Reposition Node");

                Rect newRect = draggingNode.GetRect();
                newRect.position = Event.current.mousePosition + draggingOffset;
                draggingNode.SetRect(newRect);

                Repaint();
            } else if (Event.current.type == EventType.MouseUp && draggingNode != null) {
                draggingNode = null;
            }



            // if clicked on node = change selection to node
            // if clicked on bg = change selection to dialogue

            // Selection.activeObject = 
        }

        private DialogueNode GetNodeAtPoint(Vector2 mousePosition) {
            DialogueNode foundNode = null;

            foreach (DialogueNode node in selectedDialogue.GetAllNodes()) {
                if (node.GetRect().Contains(mousePosition)) {
                    foundNode = node;
                }
            }

            return foundNode;
        }

        private void ResetNodes() {

        }





        // DRAWING NODES AND THINGS

        private void DrawDialogueSettings() {
            EditorGUILayout.LabelField("dialogue selected: " + selectedDialogue.name, EditorStyles.boldLabel);

            GUILayout.BeginVertical();

            bool isPlot = EditorGUILayout.Toggle("Is Plot", selectedDialogue.IsPlot());
            string newName = "";

            if (!isPlot) {
                newName = EditorGUILayout.TextField("Character Name", selectedDialogue.GetCharacterName(), GUILayout.Width(400));
            }

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(selectedDialogue, "Undo Update Dialogue Settings");
                selectedDialogue.SetPlotRelation(isPlot);
                selectedDialogue.SetCharacterName(newName);
            }

            GUILayout.EndVertical();
        }

        private void DrawNode(DialogueNode node) {
            DialogueGUI.SetNodeStyle(node.GetSpeaker());

            GUILayout.BeginArea(node.GetRect(), DialogueGUI.GetNodeStyle());

            // if node == root
            // DrawRootNode(node);


            SpeakerType newSpeaker = DrawSpeakerPopup(node);

            if (node.GetSpeaker() == SpeakerType.CARD) {
                if (selectedDialogue.IsPlot()) {
                    DrawAdditionalNodeFields(node);
                }
                DrawToggles(node);
            }

            DrawLabel("Text", 80);
            string newText = EditorGUILayout.TextField(node.GetText(), DialogueGUI.GetTextStyle(), GUILayout.Height(70));

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(selectedDialogue, "Update Node Settings");

                node.SetText(newText);
                node.SetSpeaker(newSpeaker);
            }

            GUILayout.EndArea();
        }

        private void DrawRootNode(DialogueNode node) {

        }

        private void DrawAdditionalNodeFields(DialogueNode node) {
            GUILayout.BeginHorizontal();

            DrawLabel("Character", 80);
            EditorGUILayout.TextField(node.GetCharacterName());
            // set

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            DrawLabel("Sprite", 80);
            EditorGUILayout.ObjectField(node.GetSprite(), typeof(Sprite), false);
            // set

            GUILayout.EndHorizontal();
        }

        private void DrawToggles(DialogueNode node) {
            GUILayout.BeginHorizontal();

            DrawLabel("Is Chained", 80);
            EditorGUILayout.Toggle(node.IsChained());
            // set

            DrawLabel("Chain Starter", 100);
            EditorGUILayout.Toggle(node.IsChainStarter());
            // set

            GUILayout.EndHorizontal();
        }

        private SpeakerType DrawSpeakerPopup(DialogueNode node)
        {
            SpeakerType speaker;

            GUILayout.BeginHorizontal();

            DrawLabel("Speaker", 80);
            speaker = (SpeakerType)EditorGUILayout.EnumPopup(node.GetSpeaker());
            node.SetSpeaker(speaker);

            GUILayout.EndHorizontal();

            return speaker;
        }

        private void DrawLabel(string labelText, float width) {
            EditorGUILayout.LabelField(labelText + ":", EditorStyles.boldLabel, GUILayout.Width(width));
        }
    }
}