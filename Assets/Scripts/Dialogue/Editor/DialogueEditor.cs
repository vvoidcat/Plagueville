using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace PLAGUEV.Dialogue.Editor {

    public class DialogueEditor : EditorWindow {

        DialogueTree selectedDialogue = null;

        // [NonSerialized] GUIStyle style = new GUIStyle();
        // [NonSerialized] GUIStyle cardNodeStyle = new GUIStyle();
        // [NonSerialized] GUIStyle playerNodeStyle = new GUIStyle();
        // [NonSerialized] GUIStyle rootLabelStyle = new GUIStyle(GUI.skin.label);
        // [NonSerialized] GUIStyle textStyle = new GUIStyle(GUI.skin.textField);

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
            SetGUIStyles();

            if (selectedDialogue == null) {
                EditorGUILayout.LabelField("dialogue selected: N/A", EditorStyles.boldLabel);
            } else {
                EditorGUILayout.LabelField("dialogue selected: " + selectedDialogue.name, EditorStyles.boldLabel);
            }
        }


        private void SetGUIStyles() {
            // // card nodes
            // cardNodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            // cardNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            // cardNodeStyle.border = new RectOffset(12, 12, 12, 12);

            // // player nodes
            // playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            // playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            // playerNodeStyle.border = new RectOffset(12, 12, 12, 12);

            // // <START> node
            // rootLabelStyle.alignment = TextAnchor.MiddleCenter;
            // rootLabelStyle.fontStyle = FontStyle.Bold;

            // // text blocks
            // textStyle.clipping = TextClipping.Clip;
            // textStyle.wordWrap = true;
            // textStyle.padding = new RectOffset(8, 8, 8, 8);
        }


        private void DrawDialogueSettings() {
            EditorGUILayout.LabelField("dialogue selected: " + selectedDialogue.name, EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();

            // EditorGUILayout.Toggle("Is Plot", selectedDialogue.isPlot);


            GUILayout.EndHorizontal();
        }

        private void ResetNodes() {

        }
    }
}