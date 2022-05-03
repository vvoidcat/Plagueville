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
                // ResetNodes();
                Repaint();
            }
        }


        private void OnEnable() {
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void OnGUI() {
            DialogueGUIStyles.SetGUIStyles();
            EditorGUI.BeginChangeCheck();

            if (selectedDialogue == null) {
                EditorGUILayout.LabelField("dialogue selected: N/A", EditorStyles.boldLabel);
            } else {
                DrawDialogueSettings();
                ProcessDragging();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                Rect canvas = GUILayoutUtility.GetRect(selectedDialogue.GetCanvasWidth(), selectedDialogue.GetCanvasHeight());
                Texture2D bgTexture = Resources.Load("background") as Texture2D;
                Rect bgTextureCoords = new Rect(0, 0, selectedDialogue.GetCanvasWidth() / DialogueGUIStyles.bgTextureSize,
                                                      selectedDialogue.GetCanvasHeight() / DialogueGUIStyles.bgTextureSize);
                GUI.DrawTextureWithTexCoords(canvas, bgTexture, bgTextureCoords);

                foreach (DialogueNode node in selectedDialogue.GetAllNodes()) {
                    DrawNode(node);
                    //DrawConnections(node);
                    DialogueGUILayout.DrawConnections(selectedDialogue, node);
                }

                EditorGUILayout.EndScrollView();


                if (parentNode != null) {
                    Undo.RecordObject(selectedDialogue, "Undo Add Node");
                    selectedDialogue.CreateNode(parentNode);
                    parentNode = null;
                }
                if (deadNode != null) {
                    Undo.RecordObject(selectedDialogue, "Undo Delete Node");
                    selectedDialogue.DeleteNode(deadNode);
                    deadNode = null;
                }
            }
        }


        private void ProcessDragging() {
            if (Event.current.type == EventType.MouseDown && draggingNode == null) {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition + dlgSettingsOffset);

                if (draggingNode != null) {
                    draggingNodeOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                } else {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                }
            } else if (Event.current.type == EventType.MouseDrag && draggingNode != null) {
                Undo.RecordObject(selectedDialogue, "Undo Reposition Node");

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

            // deadzone rect

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
            draggingNode = null;
            parentNode = null;
            deadNode = null;
            linkerNode = null;
        }














        private void DrawDialogueSettings() {
            EditorGUILayout.LabelField("dialogue selected: " + selectedDialogue.name, EditorStyles.boldLabel);

            GUILayout.BeginHorizontal(GUILayout.Width(600));
            DialogueGUILayout.DrawLabel("Is Plot", 50);
            bool isPlot = EditorGUILayout.Toggle(selectedDialogue.GetPlotState());

            GUI.enabled = !selectedDialogue.GetPlotState();
            DialogueGUILayout.DrawLabel("Character Name", 115);
            string newName = EditorGUILayout.TextField(selectedDialogue.GetCharacterName(), GUILayout.Width(200));
            GUI.enabled = true;

            DialogueGUILayout.DrawLabel("   Canvas", 70);
            float newWidth = EditorGUILayout.FloatField(selectedDialogue.GetCanvasWidth(), GUILayout.Width(50));
            float newHeight = EditorGUILayout.FloatField(selectedDialogue.GetCanvasHeight(), GUILayout.Width(50));
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(4);

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(selectedDialogue, "Undo Update Dialogue Settings");
                selectedDialogue.SetPlotRelation(isPlot);
                selectedDialogue.SetCharacterName(newName);
                selectedDialogue.SetCanvasSize(newWidth, newHeight);
            }
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
            } else {
                DialogueGUILayout.DrawActionField(node);
                DialogueGUILayout.DrawStats(node);
                DialogueGUILayout.DrawLocationToggle(node);
            }

            DialogueGUILayout.DrawText(node);

            EditorGUILayout.BeginHorizontal();
            parentNode = DialogueGUILayout.DrawAddChildButton(node, parentNode);
            linkerNode = DialogueGUILayout.DrawLinkButtons(node, linkerNode);
            deadNode = DialogueGUILayout.DrawDeleteButton(node, deadNode);
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(selectedDialogue, "Update Node Settings");
            }

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