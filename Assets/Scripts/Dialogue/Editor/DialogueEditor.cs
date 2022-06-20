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
        Vector2 mousePosition;
        
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
                // ProcessScrolling();
                ProcessEvents();

                DrawView();
                // ProcessEvents();

                // scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                // DialogueGUILayout.DrawBackground(selectedDialogue);

                // foreach (DialogueNode node in selectedDialogue.GetAllNodes()) {
                //     DialogueNode[] upd = DialogueGUILayout.DrawNode(selectedDialogue, node,
                //                                                     new DialogueNode[] {parentNode, linkerNode, deadNode});
                //     UpdateNodes(upd);
                //     DialogueGUILayout.DrawConnections(selectedDialogue, node);
                // }

                //EditorGUILayout.EndScrollView();

                //DialogueGUILayout.DrawDialogueSettings(selectedDialogue);

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


        // private void ProcessScrolling() {
        //     mousePosition = (Event.current.mousePosition + scrollPosition / scaling);

        //     if (Event.current.type == EventType.ScrollWheel && Event.current.control) {
        //         float shiftMultiplier = Event.current.shift ? 4 : 1;
        //         scaling = Mathf.Clamp(scaling - Event.current.delta.y * 0.01f * shiftMultiplier, 0.5f, 2f);
        //         Event.current.Use();
        //     }
        // }

        private void ScaleWindowGroup() {
            GUI.EndGroup();

            groupRect.x = 0;
            groupRect.y = 120;
            groupRect.width = (maxGraphSize + scrollPosition.x) / scaling;
            groupRect.height = (maxGraphSize + scrollPosition.y) / scaling;

            GUI.BeginGroup(groupRect);
        }

        private void ScaleScrollGroup() {
            GUI.EndGroup();

            groupRect.x = -scrollPosition.x / scaling;
            groupRect.y = -scrollPosition.y / scaling;
            groupRect.width = (position.width + scrollPosition.x - GUI.skin.verticalScrollbar.fixedWidth) / scaling;
            groupRect.height = (position.height + scrollPosition.y - 21 - GUI.skin.horizontalScrollbar.fixedHeight) / scaling;

            GUI.BeginGroup(groupRect);
        }

        private void DrawView() {
            Debug.Log("start; scaling = " + scaling);

            ScaleWindowGroup();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, true, true);
            ScaleScrollGroup();

            Matrix4x4 old = GUI.matrix;
            Matrix4x4 translation = Matrix4x4.TRS(new Vector3(0, 21, 1), Quaternion.identity, Vector3.one);
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(scaling, scaling, scaling));
            GUI.matrix = translation * scale * translation.inverse;

            GUILayout.BeginArea(new Rect(0, 0, maxGraphSize * scaling, maxGraphSize * scaling));
    
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
            mousePosition = (Event.current.mousePosition + scrollPosition) / scaling;

            if (Event.current.type == EventType.ScrollWheel && Event.current.control) {
                float shiftMultiplier = Event.current.shift ? 4 : 1;
                scaling = Mathf.Clamp(scaling - Event.current.delta.y * 0.01f * shiftMultiplier, 0.5f, 2f);
                Event.current.Use();
            }

            if (Event.current.type == EventType.MouseDown && draggingNode == null) {
                draggingNode = GetNodeAtPoint((Event.current.mousePosition + scrollPosition + dlgSettingsOffset) / scaling);        // CHANGE

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
                newRect.position = Event.current.mousePosition + draggingNodeOffset;        // CHANGE
                draggingNode.SetRect(newRect);
                Repaint();
            } else if (Event.current.type == EventType.MouseDrag && draggingCanvas) {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;        // CHANGE
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