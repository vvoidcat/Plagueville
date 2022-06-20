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
        
        float scaling = 1f;
        Rect groupRect;

        const float maxGraphSize = 16000.0f;


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
                DrawView();

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

        private void ScaleWindowGroup() {
            GUI.EndGroup();

            groupRect.x = 0;
            groupRect.y = 70 / scaling;
            groupRect.width = (maxGraphSize + scrollPosition.x) / scaling;
            groupRect.height = (maxGraphSize + scrollPosition.y) / scaling;

            GUI.BeginGroup(groupRect);
        }

        private void ScaleScrollGroup() {
            GUI.EndGroup();

            groupRect.x = -scrollPosition.x / scaling;
            groupRect.y = -scrollPosition.y / scaling;
            groupRect.width = (position.width + scrollPosition.x) / scaling;
            groupRect.height = (position.height + scrollPosition.y) / scaling;

            GUI.BeginGroup(groupRect);
        }

        private void DrawView() {
            ScaleWindowGroup();
            EditorGUILayout.BeginScrollView(scrollPosition, true, true);
            ScaleScrollGroup();

            Matrix4x4 old = GUI.matrix;
            Matrix4x4 translation = Matrix4x4.TRS(new Vector3(0, 0, 1), Quaternion.identity, Vector3.one);
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(scaling, scaling, scaling));
            GUI.matrix = translation * scale * translation.inverse;

            GUILayout.BeginArea(new Rect(0, 120, maxGraphSize * scaling, maxGraphSize * scaling));
    
            DialogueGUILayout.DrawBackground(selectedDialogue);
            foreach (DialogueNode node in selectedDialogue.GetAllNodes()) {
                DialogueNode[] upd = DialogueGUILayout.DrawNode(selectedDialogue, node,
                                                                new DialogueNode[] {parentNode, linkerNode, deadNode});
                UpdateNodes(upd);
                DialogueGUILayout.DrawConnections(selectedDialogue, node);
            }

            GUILayout.EndArea();

            GUI.matrix = old;
            EditorGUILayout.EndScrollView();
        }

        private void ProcessEvents() {
            if (Event.current.type == EventType.ScrollWheel) {
                float shiftMultiplier = Event.current.shift ? 4 : 1;
                scaling = Mathf.Clamp(scaling - Event.current.delta.y * 0.01f * shiftMultiplier, 0.5f, 1f);
                Event.current.Use();
            }

            if (Event.current.type == EventType.MouseDown && draggingNode == null) {
                float temp = CalculateScalingOffset();
                draggingNode = GetNodeAtPoint((Event.current.mousePosition + scrollPosition + dlgSettingsOffset * temp)
                                               / scaling);

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
        }

        private DialogueNode GetNodeAtPoint(Vector2 mousePos) {
            DialogueNode foundNode = null;

            foreach (DialogueNode node in selectedDialogue.GetAllNodes()) {
                if (node.GetRect().Contains(mousePos)) {
                    foundNode = node;
                }
            }

            return foundNode;
        }

        private float CalculateScalingOffset() {
            float result = 0f;

            if (scaling >= 0.85f && scaling <= 1f) {
                result = 4f * scaling;
            } else if (scaling >= 0.65f && scaling <= 0.84f) {
                result = 4.2f * scaling;
            } else {
                result = 5f * scaling;
            }

            return result;
        }

        private void ResetNodes() {
            draggingNode = null;
            parentNode = null;
            deadNode = null;
            linkerNode = null;
        }

        private void UpdateNodes(DialogueNode[] nodesUpdated) {
            parentNode = nodesUpdated[0];
            linkerNode = nodesUpdated[1];
            deadNode = nodesUpdated[2];
        }
    }
}