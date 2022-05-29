using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace PLAGUEV.Dialogue.Editor {

    public class DialogueEditor : EditorWindow {

        DialogueTree selectedDialogue = null;

        [NonSerialized] DialogueNode draggingNode = null;
        [NonSerialized] DialogueNode parentNode = null;
        [NonSerialized] DialogueNode deadNode = null;
        [NonSerialized] DialogueNode linkerNode = null;

        [NonSerialized] bool draggingCanvas = false;

        [NonSerialized] Vector2 draggingNodeOffset;
        [NonSerialized] Vector2 draggingCanvasOffset;
        [NonSerialized] Vector2 dlgSettingsOffset = new Vector2(0, -44);
        Vector2 scrollPosition;


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
                selectedDialogue.Initialize();
                Repaint();
            }
        }


        private void OnEnable() {
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void OnGUI() {
            DialogueGUIStyles.SetGUIStyles();

            if (selectedDialogue == null) {
                EditorGUILayout.LabelField("dialogue selected: N/A", EditorStyles.boldLabel);
            } else {
                DialogueGUILayout.DrawDialogueSettings(selectedDialogue);
                ProcessEvents();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                DialogueGUILayout.DrawBackground(selectedDialogue);

                foreach (DialogueNode node in selectedDialogue.GetAllNodes()) {
                    DrawNode(node);
                    DialogueGUILayout.DrawConnections(selectedDialogue, node);
                }

                EditorGUILayout.EndScrollView();

                if (parentNode != null) {
                    selectedDialogue.CreateNode(parentNode);
                    parentNode = null;
                }
                if (deadNode != null) {
                    selectedDialogue.DeleteNode(deadNode);
                    deadNode = null;
                }
            }
        }


        private void ProcessEvents() {
            if (Event.current.type == EventType.MouseDown && draggingNode == null) {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition + dlgSettingsOffset);

                if (draggingNode != null) {
                    draggingNodeOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                } else {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue;
                }
            } else if (Event.current.type == EventType.MouseDrag && draggingNode != null) {
                Rect newRect = draggingNode.GetRect();
                newRect.position = Event.current.mousePosition + draggingNodeOffset;
                draggingNode.SetRect(newRect);
                Repaint();
            } else if (Event.current.type == EventType.MouseDrag && draggingCanvas) {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                Repaint();
            } else if (Event.current.type == EventType.MouseUp && draggingNode != null) {
                draggingNode = null;
            } else if (Event.current.type == EventType.MouseUp && draggingCanvas) {
                draggingCanvas = false;
            }

            // deadzone rect ?
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
            draggingNode = null;
            parentNode = null;
            deadNode = null;
            linkerNode = null;
        }










        private void DrawNode(DialogueNode node) {
            DialogueGUIStyles.SetNodeStyle(node.GetSpeaker());

            GUILayout.BeginArea(node.GetRect(), DialogueGUIStyles.GetNodeStyle());

            if (!node.GetRootState()) {
                DrawRegularNode(node);
            } else {
                DrawRootNode(node);
            }

            GUILayout.EndArea();
        }

        private void DrawRegularNode(DialogueNode node) {
            DialogueGUILayout.DrawSpeakerField(node);

            if (node.GetSpeaker() == SpeakerType.CARD) {
                DialogueGUILayout.DrawCardToggles(selectedDialogue, node);
                DialogueGUILayout.DrawAdditionalFields(selectedDialogue, node);
                DialogueGUILayout.DrawQuestSettings(selectedDialogue, node);
            } else {
                DialogueGUILayout.DrawActionField(node);
                DialogueGUILayout.DrawLocationField(node);
                DialogueGUILayout.DrawStats(node);
                DialogueGUILayout.DrawQuestSettings(selectedDialogue, node);
            }

            DialogueGUILayout.DrawText(node);

            EditorGUILayout.BeginHorizontal();
            parentNode = DialogueGUILayout.DrawAddChildButton(node, parentNode);
            linkerNode = DialogueGUILayout.DrawLinkButtons(node, linkerNode);
            deadNode = DialogueGUILayout.DrawDeleteButton(node, deadNode);
            // DialogueGUILayout.DrawButtons(node, parentNode, deadNode, linkerNode);
            EditorGUILayout.EndHorizontal();

            DialogueGUILayout.ResetNodeHeight(selectedDialogue, node);
        }

        private void DrawRootNode(DialogueNode node) {
            EditorGUILayout.LabelField("<START>", DialogueGUIStyles.GetRootLabelStyle());

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            parentNode = DialogueGUILayout.DrawAddChildButton(node, parentNode);
            linkerNode = DialogueGUILayout.DrawLinkButtons(node, linkerNode);
            GUILayout.EndHorizontal();
        }
    }
}