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
        
        [NonSerialized] Vector2 draggingOffset;
        [NonSerialized] Vector3 controlPointOffset = new Vector2(20, 0);
        [NonSerialized] Color chainColor = new Color(0.3f, 0.5f, 1f);       // move to DialogueGUI
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
                // ResetNodes();
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
                ProcessNodeDragging();

                foreach (DialogueNode node in selectedDialogue.GetAllNodes()) {
                    DrawNode(node);
                    DrawConnections(node);
                }

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


        private void ProcessNodeDragging() {
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

            bool newChainedState = node.GetChainedState();
            bool newCustomState = node.GetCustomState();
            string newText = node.GetText();
            SpeakerType newSpeaker = node.GetSpeaker();

            newSpeaker = DrawSpeakerField(node);

            if (newSpeaker == SpeakerType.CARD) {
                if (selectedDialogue.IsPlot() || newCustomState) {
                    DrawCharacterField(node);
                    DrawSpriteField(node);
                }
                DrawStats(node);
            }

            DrawLabel("Text", 80);
            newText = EditorGUILayout.TextField(node.GetText(), DialogueGUI.GetTextStyle(), GUILayout.Height(70));

            if (newSpeaker == SpeakerType.CARD) {
                GUILayout.BeginHorizontal();
                DrawLabel("Chained", 80);
                newChainedState = EditorGUILayout.Toggle(node.GetChainedState());

                DrawLabel("Custom", 80);
                newCustomState = EditorGUILayout.Toggle(node.GetCustomState());
                GUILayout.EndHorizontal();
            }

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(selectedDialogue, "Update Node Settings");

                node.SetText(newText);
                node.SetSpeaker(newSpeaker);
                node.SetChained(newChainedState);
                node.SetCustom(newCustomState);
            }

            if (selectedDialogue.GetAllChildren(node) != null) {
                //Debug.Log("node has children");
            }

            EditorGUILayout.BeginHorizontal();
            DrawAddChildButton(node);
            DrawLinkButtons(node);
            DrawDeleteButton(node);
            EditorGUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawRootNode(DialogueNode node) {
            // GUILayout.BeginArea(node.GetRect(), DialogueGUI.GetNodeStyle());

            // EditorGUILayout.LabelField("<START>", rootLabelStyle);

            // GUILayout.FlexibleSpace();
            // GUILayout.BeginHorizontal();
            // DrawLinkButtons(node);
            // DrawAddChildButton(node);
            // GUILayout.EndHorizontal();
            
            // GUILayout.EndArea();
        }

        private void DrawConnections(DialogueNode node) {
            Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);

            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node)) {
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
                Color lineColor = Color.white;

                if (childNode.GetChainedState()) {
                    lineColor = chainColor;
                }

                Handles.DrawBezier(startPosition, endPosition,
                                   startPosition + controlPointOffset,
                                   endPosition - controlPointOffset,
                                   lineColor, null, 4f);
            }
        }





        private SpeakerType DrawSpeakerField(DialogueNode node)
        {
            SpeakerType speaker;

            GUILayout.BeginHorizontal();
            DrawLabel("Speaker", 80);
            speaker = (SpeakerType)EditorGUILayout.EnumPopup(node.GetSpeaker());
            node.SetSpeaker(speaker);
            GUILayout.EndHorizontal();

            return speaker;
        }

        private void DrawCharacterField(DialogueNode node) {
            GUILayout.BeginHorizontal();
            DrawLabel("Character", 80);
            string newName = EditorGUILayout.TextField(node.GetCharacterName());
            node.SetCharacterName(newName);
            GUILayout.EndHorizontal();
        }

        private void DrawSpriteField(DialogueNode node) {
            GUILayout.BeginHorizontal();
            DrawLabel("Sprite", 80);
            Sprite newSprite = (Sprite)EditorGUILayout.ObjectField(node.GetSprite(), typeof(Sprite), false);
            node.SetSprite(newSprite);
            GUILayout.EndHorizontal();
        }

        private void DrawStats(DialogueNode node) {
            int[] nodeValues = node.GetStatValues();
            int[] newValues = new int[nodeValues.Length];
            string[] labels = new string[] {"Money", "Knowledge", "Glory", "Faith"};

            for (int i = 0; i < nodeValues.Length; i++) {
                GUILayout.BeginHorizontal();
                DrawLabel(labels[i], 80);
                newValues[i] = EditorGUILayout.IntSlider(nodeValues[i], -100, 100);
                GUILayout.EndHorizontal();
            }

            node.SetStatValues(newValues);
        }

        private void DrawLabel(string labelText, float width) {
            EditorGUILayout.LabelField(labelText + ":", EditorStyles.boldLabel, GUILayout.Width(width));
        }









        private void DrawAddChildButton(DialogueNode node) {
            if (GUILayout.Button("+")) {
                parentNode = node;
            }
        }

        private void DrawDeleteButton(DialogueNode node) {
            if (GUILayout.Button("x")) {
                deadNode = node;
            }
        }

        private void DrawLinkButtons(DialogueNode node) {
            bool isLinkable = true;

            if (linkerNode == null) {
                if (GUILayout.Button("link")) {
                    linkerNode = node;
                }
            } else if (linkerNode == node) {
                if (GUILayout.Button("cancel")) {
                    linkerNode = null;
                }
            } else if (linkerNode.GetChildren().Contains(node.GetID())) {
                if (GUILayout.Button("unchild")) {
                    linkerNode.RemoveChild(node.GetID());
                    linkerNode = null;
                }
            } else {
                if (node.GetChildren().Contains(linkerNode.GetID())) {
                    isLinkable = false;
                }

                GUI.enabled = isLinkable;
                
                if (GUILayout.Button("child")) {
                    linkerNode.AddChild(node.GetID());
                    linkerNode = null;
                }
                
                GUI.enabled = true;
            }
        }
    }
}